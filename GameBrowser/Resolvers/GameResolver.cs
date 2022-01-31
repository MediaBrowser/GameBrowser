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
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Configuration;

namespace GameBrowser.Resolvers
{
    /// <summary>
    /// Class GameResolver
    /// </summary>
    public class GameResolver : ItemResolver<Game>, IMultiItemResolver
    {
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly ILibraryManager _libraryManager;

        public GameResolver(ILogger logger, IFileSystem fileSystem, ILibraryManager libraryManager)
        {
            _logger = logger;
            _fileSystem = fileSystem;
            _libraryManager = libraryManager;
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

        public MultiItemResolverResult ResolveMultiple(Folder parent,
            List<FileSystemMetadata> files,
            LibraryOptions libraryOptions,
            IDirectoryService directoryService)
        {
            var result = ResolveMultipleInternal(parent, files, libraryOptions.ContentType, libraryOptions, directoryService);

            if (result != null)
            {
                foreach (var item in result.Items)
                {
                    SetInitialItemValues((Game)item, null);
                }
            }

            return result;
        }

        private MultiItemResolverResult ResolveMultipleInternal(Folder parent,
            List<FileSystemMetadata> files,
            string collectionType,
            LibraryOptions libraryOptions,
            IDirectoryService directoryService)
        {
            if (collectionType.AsSpan().Equals(CollectionType.Games.Span, StringComparison.OrdinalIgnoreCase))
            {
                return ResolveMultiple<Game>(parent, files, directoryService, libraryOptions, false, false, true);
            }

            return null;
        }

        private bool LeaveFolderInLeftOverFiles(FileSystemMetadata folder)
        {
            var filename = folder.Name;

            if (BaseItem.ExtrasSubFolders.ContainsKey(filename))
            {
                return false;
            }

            return true;
        }

        private bool ContainsFile(List<Game> result, FileSystemMetadata file)
        {
            return result.Any(i => ContainsFile(i, file));
        }

        private bool ContainsFile(Game result, FileSystemMetadata file)
        {
            return string.Equals(result.Path, file.FullName, StringComparison.OrdinalIgnoreCase);
        }

        private MultiItemResolverResult ResolveMultiple<T>(Folder parent, IEnumerable<FileSystemMetadata> fileSystemEntries, IDirectoryService directoryService, LibraryOptions libraryOptions, bool parseName, bool enforceIgnore, bool checkSubFolders)
            where T : Game, new()
        {
            var gameSystem = parent as GameSystem ?? parent.FindParent<GameSystem>();

            if (gameSystem == null)
            {
                return null;
            }

            var files = new List<FileSystemMetadata>();
            var games = new List<BaseItem>();
            var leftOver = new List<FileSystemMetadata>();

            var setIsInMixedFolder = false;
            var isInMixedFolderValue = false;

            // Loop through each child file/folder and see if we find a video
            foreach (var child in fileSystemEntries)
            {
                if (child.IsDirectory)
                {
                    leftOver.Add(child);
                }
                else
                {
                    if (!enforceIgnore || !_libraryManager.IgnoreFile(child, parent, libraryOptions))
                    {
                        files.Add(child);
                    }
                }
            }

            var resolverResult = files.Select(i => ResolveGame(i, gameSystem)).Where(i => i != null).ToList();

            var result = new MultiItemResolverResult
            {
                ExtraFiles = leftOver
            };

            foreach (var game in resolverResult)
            {
                if (parent != null && parent.IsTopParent)
                {
                    isInMixedFolderValue = true;
                }

                if (isInMixedFolderValue)
                {
                    setIsInMixedFolder = true;
                }

                result.Items.Add(game);
            }

            if (setIsInMixedFolder)
            {
                foreach (var item in result.Items)
                {
                    item.IsInMixedFolder = isInMixedFolderValue;
                }
            }

            // do this after to prevent setting IsInMixedFolder
            result.Items.InsertRange(0, games);

            result.IsInMixedFolderSet = setIsInMixedFolder;

            if (result.Items.Count == 1 || (setIsInMixedFolder && !isInMixedFolderValue))
            {
                result.ExtraFiles = result.ExtraFiles.Where(LeaveFolderInLeftOverFiles).ToList();
            }

            result.ExtraFiles.AddRange(files.Where(i => !ContainsFile(resolverResult, i)));

            return result;
        }

        private Game ResolveGame(FileSystemMetadata file, GameSystem gameSystem)
        {
            var path = file.FullName;

            var platform = ResolverHelper.AttemptGetGamePlatformTypeFromPath(_fileSystem, path);

            if (string.IsNullOrEmpty(platform)) return null;

            // For MAME we will allow all games in the same dir
            if (string.Equals(platform, "Arcade", StringComparison.OrdinalIgnoreCase))
            {
                var extension = Path.GetExtension(path);

                if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".7z", StringComparison.OrdinalIgnoreCase))
                {
                    // ignore zips that are bios roms.
                    if (MameUtils.IsBiosRom(path)) return null;

                    var game = new Game
                    {
                        Name = MameUtils.GetFullNameFromPath(path, _logger),
                        Path = path,
                        IsInMixedFolder = true,
                        Album = gameSystem.Name,
                        AlbumId = gameSystem.InternalId,
                        Container = extension.TrimStart('.')
                    };
                    return game;
                }
            }
            else
            {
                var validExtensions = GetExtensions(platform);
                var fileExtension = Path.GetExtension(path);

                if (!validExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
                {
                    return null;
                }

                var game = new Game
                {
                    Path = path,
                    Album = gameSystem.Name,
                    AlbumId = gameSystem.InternalId,
                    Container = fileExtension.TrimStart('.')
                };

                return game;
            }

            return null;
        }

        protected override Game Resolve(ItemResolveArgs args)
        {
            // handled by multi-item resolving
            return null;
        }

        private IEnumerable<string> GetExtensions(string consoleType)
        {
            switch (consoleType)
            {
                case "3DO":
                    return new[] { ".iso", ".cue" };

                case "Amiga":
                    return new[] { ".iso", ".adf" };

                case "Arcade":
                    return new[] { ".zip" };

                case "Atari 2600":
                    return new[] { ".bin", ".a26" };

                case "Atari 5200":
                    return new[] { ".bin", ".a52" };

                case "Atari 7800":
                    return new[] { ".a78" };

                case "Atari XE":
                    return new[] { ".rom" };

                case "Atari Jaguar":
                    return new[] { ".j64", ".zip" };

                case "Atari Jaguar CD": // still need to verify
                    return new[] { ".iso" };

                case "Colecovision":
                    return new[] { ".col", ".rom" };

                case "Commodore 64":
                    return new[] { ".d64", ".g64", ".prg", ".tap", ".t64" };

                case "Commodore Vic-20":
                    return new[] { ".prg" };

                case "Intellivision":
                    return new[] { ".int", ".rom" };

                case "Xbox":
                    return new[] { ".disc", ".iso" };

                case "Xbox 360":
                    return new[] { ".disc", ".iso" };

                case "Xbox One":
                    return new[] { ".disc" };

                case "Neo Geo":
                    return new[] { ".zip", ".iso" };

                case "Nintendo 64":
                    return new[] { ".z64", ".v64", ".usa", ".jap", ".pal", ".rom", ".n64", ".zip" };

                case "Nintendo DS":
                    return new[] { ".nds", ".zip" };

                case "Nintendo 3DS":
                    return new[] { ".3ds", ".cia" };

                case "Nintendo":
                    return new[] { ".nes", ".zip" };

                case "Nintendo Switch":
                    return new[] { ".xci", ".nsp" };

                case "Game Boy":
                    return new[] { ".gb", ".zip" };

                case "Game Boy Advance":
                    return new[] { ".gba", ".zip" };

                case "Game Boy Color":
                    return new[] { ".gbc", ".zip" };

                case "Gamecube":
                    return new[] { ".iso", ".bin", ".img", ".gcm", ".gcz", ".rvz" };

                case "Super Nintendo":
                    return new[] { ".smc", ".zip", ".fam", ".rom", ".sfc", ".fig" };

                case "Virtual Boy":
                    return new[] { ".vb" };

                case "Nintendo Wii":
                    return new[] { ".iso", ".dol", ".ciso", ".wbfs", ".wad", ".gcz", ".rvz" };

                case "Nintendo Wii U":
                    return new[] { ".disc", ".wud", ".wux" };

                case "DOS":
                    return new[] { ".gbdos", ".disc" };

                case "Windows":
                    return new[] { ".gbwin", ".disc" };

                case "Sega 32X":
                    return new[] { ".iso", ".bin", ".img", ".zip", ".32x" };

                case "Sega CD":
                    return new[] { ".iso", ".bin", ".img", ".chd" };

                case "Dreamcast":
                    return new[] { ".chd", ".gdi", ".cdi", ".bin", ".cue" };

                case "Game Gear":
                    return new[] { ".gg", ".zip" };

                case "Sega Genesis":
                    return new[] { ".smd", ".bin", ".gen", ".zip", ".md" };

                case "Sega Master System":
                    return new[] { ".sms", ".sg", ".sc", ".zip" };

                case "Sega Mega Drive":
                    return new[] { ".smd", ".zip", ".md" };

                case "Sega Saturn":
                    return new[] { ".iso", ".bin", ".img", ".chd" };

                case "Sony Playstation":
                    return new[] { ".iso", ".cue", ".img", ".ps1", ".pbp", ".chd" };

                case "PS2":
                    return new[] { ".iso", ".bin", ".chd" };

                case "PS3":
                    return new[] { ".disc" };

                case "PS4":
                    return new[] { ".disc" };

                case "PSP":
                    return new[] { ".iso", ".cso" };

                case "TurboGrafx 16":
                    return new[] { ".pce", ".zip" };

                case "TurboGrafx CD":
                    return new[] { ".bin", ".iso" };

                case "ZX Spectrum":
                    return new[] { ".z80", ".tap", ".tzx" };

                default:
                    return new string[] { };
            }

        }
    }
}
