using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

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

            var emuMoviesPlatform = GetEmuMoviesPlatformFromGameSystem(game.GameSystem);
            // TODO Add a setting to search or not image from other game system
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

        private string GetEmuMoviesPlatformFromGameSystem(string gameSystem)
        {
            string emuMoviesPlatform = null;

            switch (gameSystem)
            {
                case "Panasonic3DO":
                    emuMoviesPlatform = "Panasonic_3DO";

                    break;

                case "Amiga":
                    emuMoviesPlatform = "";

                    break;

                case "Arcade":
                    emuMoviesPlatform = "MAME";

                    break;

                case "Atari2600":
                    emuMoviesPlatform = "Atari_2600";

                    break;

                case "Atari5200":
                    emuMoviesPlatform = "Atari_5200";

                    break;

                case "Atari7800":
                    emuMoviesPlatform = "Atari_7800";

                    break;

                case "AtariST":
                    emuMoviesPlatform = "";

                    break;

                case "AtariXE":
                    emuMoviesPlatform = "Atari_8_bit";

                    break;

                case "AtariJaguar":
                    emuMoviesPlatform = "Atari_Jaguar";

                    break;

                case "AtariJaguarCD":
                    emuMoviesPlatform = "Atari_Jaguar";

                    break;

                case "AtariLynx":
                    emuMoviesPlatform = "";

                    break;

                case "Colecovision":
                    emuMoviesPlatform = "Coleco_Vision";

                    break;

                case "Commodore64":
                    emuMoviesPlatform = "Commodore_64";

                    break;

                case "CommodoreVic20":
                    emuMoviesPlatform = "";

                    break;

                case "Intellivision":
                    emuMoviesPlatform = "Mattel_Intellivision";

                    break;

                case "MicrosoftXBox":
                    emuMoviesPlatform = "Microsoft_Xbox";

                    break;

                case "NeoGeo":
                    emuMoviesPlatform = "SNK_Neo_Geo_AES";

                    break;

                case "NeoGeoPocket":
                    emuMoviesPlatform = "";

                    break;

                case "NeoGeoPocketColor":
                    emuMoviesPlatform = "";

                    break;

                case "Nintendo64":
                    emuMoviesPlatform = "Nintendo_N64";

                    break;

                case "NintendoDS":
                    emuMoviesPlatform = "Nintendo_DS";

                    break;

                case "Nintendo":
                    emuMoviesPlatform = "Nintendo_NES";

                    break;

                case "NintendoGameBoy":
                    emuMoviesPlatform = "Nintendo_Game_Boy";

                    break;

                case "NintendoGameBoyAdvance":
                    emuMoviesPlatform = "Nintendo_Game_Boy_Advance";

                    break;

                case "NintendoGameBoyColor":
                    emuMoviesPlatform = "Nintendo_Game_Boy_Color";

                    break;

                case "NintendoGameCube":
                    emuMoviesPlatform = "Nintendo_GameCube";

                    break;

                case "SuperNintendo":
                    emuMoviesPlatform = "Nintendo_SNES";

                    break;

                case "VirtualBoy":
                    emuMoviesPlatform = "";

                    break;

                case "Wii":
                    emuMoviesPlatform = "";

                    break;

                case "DOS":
                    emuMoviesPlatform = "";

                    break;

                case "Windows":
                    emuMoviesPlatform = "";

                    break;

                case "Sega32X":
                    emuMoviesPlatform = "Sega_Genesis";

                    break;

                case "SegaCD":
                    emuMoviesPlatform = "Sega_Genesis";

                    break;

                case "SegaDreamcast":
                    emuMoviesPlatform = "Sega_Dreamcast";

                    break;

                case "SegaGameGear":
                    emuMoviesPlatform = "Sega_Game_Gear";

                    break;

                case "SegaGenesis":
                    emuMoviesPlatform = "Sega_Genesis";

                    break;

                case "SegaMasterSystem":
                    emuMoviesPlatform = "Sega_Master_System";

                    break;

                case "SegaMegaDrive":
                    emuMoviesPlatform = "Sega_Genesis";

                    break;

                case "SegaSaturn":
                    emuMoviesPlatform = "Sega_Saturn";

                    break;

                case "SonyPlaystation":
                    emuMoviesPlatform = "Sony_Playstation";

                    break;

                case "SonyPlaystation2":
                    emuMoviesPlatform = "Sony_Playstation_2";

                    break;

                case "SonyPSP":
                    emuMoviesPlatform = "Sony_PSP";

                    break;

                case "TurboGrafx16":
                    emuMoviesPlatform = "NEC_TurboGrafx_16";

                    break;

                case "TurboGrafxCD":
                    emuMoviesPlatform = "NEC_TurboGrafx_16";
                    break;

                case "ZxSpectrum":
                    emuMoviesPlatform = "";
                    break;

#if DEBUG
                default:
                    throw new ArgumentException($"Unrecognized game system: {gameSystem}.");
#endif
            }

            return emuMoviesPlatform;

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
