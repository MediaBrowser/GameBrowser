using GameBrowser.Resolvers;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace GameBrowser.Providers.EmuMovies
{
    public class EmuMoviesImageProvider : IRemoteImageProvider, IHasOrder
    {
        private readonly IHttpClient _httpClient;

        public EmuMoviesImageProvider(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
        {
            var list = new List<RemoteImageInfo>();

            foreach (var image in GetSupportedImages(item))
            {
                var sublist = await GetImages(item, image, cancellationToken).ConfigureAwait(false);

                list.AddRange(sublist);
            }

            return list;
        }

        public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            // TODO: Call GetEmuMoviesToken and replace the sessionId in the incoming url with the latest value. 

            return _httpClient.GetResponse(new HttpRequestOptions
            {
                CancellationToken = cancellationToken,
                Url = url,
                ResourcePool = Plugin.Instance.EmuMoviesSemiphore
            });
        }

        public Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, ImageType imageType, CancellationToken cancellationToken)
        {
            var game = (Game)item;

            switch (imageType)
            {
                case ImageType.Backdrop:
                    return FetchImages(game, EmuMoviesMediaTypes.Cabinet, imageType, cancellationToken);
                case ImageType.Banner:
                    return FetchImages(game, EmuMoviesMediaTypes.Banner, imageType, cancellationToken);
                case ImageType.Primary:
                    return FetchImages(game, EmuMoviesMediaTypes.Box, imageType, cancellationToken);
                case ImageType.BoxRear:
                    return FetchImages(game, EmuMoviesMediaTypes.BoxBack, imageType, cancellationToken);
                case ImageType.Disc:
                    return FetchImages(game, EmuMoviesMediaTypes.Cart, imageType, cancellationToken);
                case ImageType.Menu:
                    return FetchImages(game, EmuMoviesMediaTypes.Title, imageType, cancellationToken);
                case ImageType.Screenshot:
                    return FetchImages(game, EmuMoviesMediaTypes.Snap, imageType, cancellationToken);

                case ImageType.Art:
                case ImageType.Box:
                case ImageType.Logo:
                case ImageType.Thumb:
                case ImageType.Chapter:
                default:
                    throw new ArgumentException("Unrecognized image type");
            }
        }

        /// <summary>
        /// Fetches the images.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="type">The type.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task{IEnumerable{RemoteImageInfo}}.</returns>
        private async Task<IEnumerable<RemoteImageInfo>> FetchImages(Game game, EmuMoviesMediaTypes mediaType, ImageType type, CancellationToken cancellationToken)
        {
            var sessionId = await Plugin.Instance.GetEmuMoviesToken(cancellationToken);

            var list = new List<RemoteImageInfo>();

            if (sessionId == null) return list;

            var emuMoviesPlatform = ResolverHelper.GetExtendedInfoFromGameSystem(game.GameSystem)?.EmuMoviesPlatform;
            if (string.IsNullOrEmpty(emuMoviesPlatform)) return list;
            var url = string.Format(EmuMoviesUrls.Search, WebUtility.UrlEncode(game.Name), emuMoviesPlatform, mediaType, sessionId);

            using (var response = await _httpClient.SendAsync(new HttpRequestOptions
            {

                Url = url,
                CancellationToken = cancellationToken,
                ResourcePool = Plugin.Instance.EmuMoviesSemiphore

            }, "GET").ConfigureAwait(false))
            {
                using (var stream = response.Content)
                {
                    var doc = new XmlDocument();
                    doc.Load(stream);

                    if (doc.HasChildNodes)
                    {
                        var nodes = doc.SelectNodes("Results/Result");

                        if (nodes != null)
                        {
                            foreach (XmlNode node in nodes)
                            {
                                if (node != null && node.Attributes != null)
                                {
                                    var urlAttribute = node.Attributes["URL"];

                                    if (urlAttribute != null && !string.IsNullOrEmpty(urlAttribute.Value))
                                    {
                                        list.Add(new RemoteImageInfo
                                        {
                                            ProviderName = Name,
                                            Type = type,
                                            Url = urlAttribute.Value
                                        });
                                    }
                                }
                            }
                        }

                    }
                }
            }

            return list;
        }

        public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
        {
            return new[] { ImageType.Primary, ImageType.Backdrop, ImageType.Banner, ImageType.BoxRear, ImageType.Disc, ImageType.Menu, ImageType.Screenshot };
        }

        public string Name
        {
            get { return "Emu Movies"; }
        }

        public bool Supports(BaseItem item)
        {
            return item is Game;
        }

        public int Order
        {
            get
            {
                // Make sure it runs after games db since these images are lower resolution
                return 1;
            }
        }
    }
}
