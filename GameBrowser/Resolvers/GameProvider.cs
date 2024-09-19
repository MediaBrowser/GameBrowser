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
    public class GameProvider : ICustomMetadataProvider<Game>, IForcedProvider, IHasItemChangeMonitor
    {
        public string Name => "Game Provider";

        private IFileSystem _fileSystem;
        private ILogger _logger;

        public GameProvider(IFileSystem fileSystem, ILogger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }
        public bool HasChanged(BaseItem item, LibraryOptions libraryOptions, IDirectoryService directoryService)
        {
            var path = item.Path;

            if (!string.IsNullOrEmpty(path) && item.IsFileProtocol && !item.IsResolvedToFolder)
            {
                var file = directoryService.GetFile(path);
                if (file != null && item.HasDateModifiedChanged(file.LastWriteTimeUtc))
                {
                    _logger.Debug("Refreshing {0} due to date modified change {1} - {2}.", item.Path, item.DateModified.ToUnixTimeSeconds(), file.LastWriteTimeUtc.ToUnixTimeSeconds());
                    return true;
                }
            }

            return false;
        }


        public Task<ItemUpdateType> FetchAsync(MetadataResult<Game> itemResult, MetadataRefreshOptions options, LibraryOptions libraryOptions, CancellationToken cancellationToken)
        {
            var updateType = ItemUpdateType.None;

            var item = itemResult.Item;

            var path = item.Path;

            if (string.IsNullOrEmpty(item.Album))
            {
                if (!string.IsNullOrEmpty(path))
                {
                    var platform = ResolverHelper.GetGameSystemFromPath(_fileSystem, path);

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

            if (!string.IsNullOrEmpty(path) && item.IsFileProtocol && !item.IsResolvedToFolder)
            {
                var file = options.DirectoryService.GetFile(path);
                if (file != null)
                {
                    item.Size = file.Length;

                    if (item.HasDateModifiedChanged(file.LastWriteTimeUtc))
                    {
                        item.DateModified = file.LastWriteTimeUtc;
                    }
                }
            }

            return Task.FromResult(updateType);
        }
    }
}
