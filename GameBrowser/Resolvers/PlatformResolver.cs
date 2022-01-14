using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Entities;
using System;
using System.Linq;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    /// <summary>
    /// Class ConsoleFolderResolver
    /// </summary>
    public class PlatformResolver : ItemResolver<GameSystem>
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly ILibraryManager _libraryManager;

        public PlatformResolver(ILogger logger, IFileSystem fileSystem, ILibraryManager libraryManager)
        {
            _logger = logger;
            _fileSystem = fileSystem;
            _libraryManager = libraryManager;
        }

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>ConsoleFolder.</returns>
        protected override GameSystem Resolve(ItemResolveArgs args)
        {
            if (args.IsDirectory)
            {
                var collectionType = args.GetCollectionType();

                if (!collectionType.AsSpan().Equals(CollectionType.Games.Span, StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                var configuredSystems = Plugin.Instance.Configuration.GameSystems;

                if (configuredSystems == null)
                {
                    return null;
                }

                var path = args.Path;

                var system =
                    configuredSystems.FirstOrDefault(
                        s => string.Equals(args.Path, s.Path, StringComparison.OrdinalIgnoreCase));

                if (system != null)
                {
                    var platform = ResolverHelper.AttemptGetGamePlatformTypeFromPath(_fileSystem, path);

                    return new GameSystem
                    {
                        Container = platform
                    };
                };
            }

            return null;
        }
    }
}
