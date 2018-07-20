using GameBrowser.Library.Utils;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Resolvers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    /// <summary>
    /// Class GameResolver
    /// </summary>
    public class GameResolver : ItemResolver<Game>
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;

        public GameResolver(ILogger logger, IFileSystem fileSystem)
        {
            _logger = logger;
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Run before any core resolvers
        /// </summary>
        public override ResolverPriority Priority
        {
            get
            {
                return ResolverPriority.First;
            }
        }

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>Game.</returns>
        protected override Game Resolve(ItemResolveArgs args)
        {
            var collectionType = args.GetCollectionType();

            if (!string.Equals(collectionType, CollectionType.Games, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            var platform = ResolverHelper.AttemptGetGamePlatformTypeFromPath(_fileSystem, args.Path);

            if (string.IsNullOrEmpty(platform)) return null;

            if (args.IsDirectory)
            {
                return GetGame(args, platform);
            }

            // For MAME we will allow all games in the same dir
            if (string.Equals(platform, "Arcade"))
            {
                var extension = Path.GetExtension(args.Path);

                if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    // ignore zips that are bios roms.
                    if (MameUtils.IsBiosRom(args.Path)) return null;

                    var game = new Game
                    {
                        Name = MameUtils.GetFullNameFromPath(args.Path, _logger),
                        Path = args.Path,
                        GameSystem = "Arcade",
                        IsInMixedFolder = true
                    };
                    return game;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified path is game.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="consoleType">The type of gamesystem this game belongs too</param>
        /// <returns>A Game</returns>
        private Game GetGame(ItemResolveArgs args, string consoleType)
        {
            var gameSystemDefinition = ResolverHelper.GetExtendedInfoFromConsoleType(consoleType);
            var validExtensions = gameSystemDefinition?.Extensions;

            var gameFiles = args.FileSystemChildren.Where(f => validExtensions.Contains(Path.GetExtension(f.FullName), StringComparer.OrdinalIgnoreCase)).ToArray();

            if (gameFiles.Length == 0)
            {
                _logger.Error($"gameFiles is 0 for {args.Path}. Expected: {string.Join(";", validExtensions)}. Found: {string.Join(";", args.FileSystemChildren.Where(f => !f.IsDirectory).Select(f => Path.GetExtension(f.FullName)).Distinct())}.");
                return null;
            }

            var game = new Game
            {
                Path = gameFiles[0].FullName,
                GameSystem = gameSystemDefinition?.Name
            };

            game.IsPlaceHolder = false;
                
            if (gameFiles.Length > 1)
            {
                game.MultiPartGameFiles = gameFiles.Select(i => i.FullName).ToArray();
                game.IsMultiPart = true;
            }

            return game;
        }
    }
}
