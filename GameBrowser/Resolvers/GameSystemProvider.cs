using MediaBrowser.Controller.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using MediaBrowser.Controller.Entities;
using System.Threading.Tasks;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Configuration;
using System.Threading;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Dto;
using MediaBrowser.Model.Entities;
using System.IO;
using MediaBrowser.Model.Logging;

namespace GameBrowser.Resolvers
{
    public class GameSystemProvider : ICustomMetadataProvider<Game>, IForcedProvider
    {
        public string Name => "Game System Provider";

        private IFileSystem _fileSystem;

        public GameSystemProvider(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<ItemUpdateType> FetchAsync(MetadataResult<Game> itemResult, MetadataRefreshOptions options, LibraryOptions libraryOptions, CancellationToken cancellationToken)
        {
            var updateType = ItemUpdateType.None;

            var item = itemResult.Item;

            if (string.IsNullOrEmpty(item.Album))
            {
                var path = item.Path;

                if (!string.IsNullOrEmpty(path))
                {
                    var platform = ResolverHelper.GetGamePlatformFromPath(_fileSystem, path);

                    if (platform == null)
                    {
                        //Logger.Warn("Platform not found for game {0}", path);
                        return Task.FromResult(updateType);
                    }

                    var gameSystem = new LinkedItemInfo
                    {
                        Name = Path.GetFileName(platform.Path),
                        ProviderIds = new ProviderIdDictionary()
                    };
                    gameSystem.ProviderIds["console"] = platform.ConsoleType;

                    item.AlbumItem = gameSystem;

                    updateType = ItemUpdateType.MetadataImport;
                }
            }

            return Task.FromResult(updateType);
        }
    }
}
