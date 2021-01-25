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

        public MultiItemResolverResult ResolveMultiple(Folder parent,
            List<FileSystemMetadata> files,
            LibraryOptions libraryOptions,
            IDirectoryService directoryService)
        {
            var result = ResolveMultipleInternal(parent, files, libraryOptions.ContentType, directoryService);

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
            IDirectoryService directoryService)
        {
            if (collectionType.AsSpan().Equals(CollectionType.Games.Span, StringComparison.OrdinalIgnoreCase))
            {
                return ResolveMultiple<Game>(parent, files, directoryService, collectionType);
            }

            return null;
        }

        private MultiItemResolverResult ResolveMultiple<T>(Folder parent, IEnumerable<FileSystemMetadata> fileSystemEntries, IDirectoryService directoryService, string collectionType)
            where T : Game, new()
        {
            var gameSystem = parent as GameSystem ?? parent.FindParent<GameSystem>();

            if (gameSystem == null)
            {
                return null;
            }

            var files = new List<FileSystemMetadata>();
            var items = new List<BaseItem>();
            var leftOver = new List<FileSystemMetadata>();

            // Loop through each child file/folder and see if we find a video
            foreach (var child in fileSystemEntries)
            {
                if (child.IsDirectory)
                {
                    leftOver.Add(child);
                }
                else if (IsIgnored(child.Name))
                {

                }
                else
                {
                    var game = ResolveGame(child, gameSystem);
                    if (game != null)
                    {
                        items.Add(game);
                    }
                    else
                    {
                        leftOver.Add(child);
                    }
                }
            }

            var result = new MultiItemResolverResult
            {
                ExtraFiles = leftOver,
                Items = items
            };

            return result;
        }

        private bool IsIgnored(string filename)
        {
            return false;
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

                //if (gameFiles.Count > 1)
                //{
                //    game.MultiPartGameFiles = gameFiles.Select(i => i.FullName).ToArray();
                //    game.IsMultiPart = true;
                //}

                return game;
            }

            return null;
        }

        /// <summary>
        /// Resolves the specified args.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>Game.</returns>
        protected override Game Resolve(ItemResolveArgs args)
        {
            return null;
        }

        private IEnumerable<string> GetExtensions(string consoleType)
        {
            switch (consoleType)
            {
                case "3DO":
                    return new[] { ".iso", ".cue" };

                case "Amiga":
                    return new[] { ".iso", ".adf", ".dms", ".exe", ".adz", ".rp9" };

                case "Arcade":
                    return new[] { ".zip" };

                case "Atari 2600":
                    return new[] { ".bin", ".a26", ".gz", ".rom", ".zip", ".7z" };

                case "Atari 5200":
                    return new[] { ".bin", ".a52", ".bas", ".car", ".cas", ".xex", ".atr", ".xfd", ".dcm", ".gz", ".zip" };

                case "Atari 7800":
                    return new[] { ".a78", ".bin", ".7z", ".zip" };

                case "Atari XE":
                    return new[] { ".rom" };

                case "Atari Jaguar":
                    return new[] { ".j64", ".jag", ".rom", ".abs", ".cof", ".bin", ".prg", ".zip" };

                case "Atari Jaguar CD": // still need to verify
                    return new[] { ".iso" };

                case "Colecovision":
                    return new[] { ".col", ".rom", ".bin", ".zip" };

                case "Commodore 64":
                    return new[] { ".d64", ".g64", ".prg", ".tap", ".t64", ".crt" };

                case "Commodore Vic-20":
                    return new[] { ".prg" };

                case "Intellivision":
                    return new[] { ".int", ".rom", ".bin" };

                case "Xbox":
                    return new[] { ".disc", ".iso" };

                case "Xbox 360":
                    return new[] { ".disc", ".iso" };

                case "Xbox One":
                    return new[] { ".disc", ".iso" };

                case "Neo Geo":
                    return new[] { ".zip", ".iso", ".7z" };

                case "Nintendo 64":
                    return new[] { ".z64", ".v64", ".usa", ".jap", ".pal", ".rom", ".n64", ".zip", ".u1", ".bin", ".ndd" };

                case "Nintendo DS":
                    return new[] { ".nds", ".zip", ".bin" };

                case "Nintendo":
                    return new[] { ".nes", ".zip", ".fds", ".fig", ".mgd", ".sfc", ".smc", ".swc", ".7z", ".bin", ".rom", ".unif", ".unf" };

                case "Game Boy":
                    return new[] { ".gb", ".bin", ".rom", ".7z", ".zip" };

                case "Game Boy Advance":
                    return new[] { ".gba", ".agb", ".zip", ".7z" };

                case "Game Boy Color":
                    return new[] { ".gbc", ".bin", ".rom", ".dmg", ".sgb", ".cgb", ".zip", ".7z" };

                case "Gamecube":
                    return new[] { ".iso", ".bin", ".img", ".gcm", ".gcz" };

                case "Super Nintendo":
                    return new[] { ".smc", ".zip", ".fam", ".rom", ".sfc", ".fig", ".swc", ".7z", ".bin", ".mgd", ".bs", ".st", ".bml", ".bsx" };

                case "Virtual Boy":
                    return new[] { ".vb", ".vboy", ".bin", ".7z", ".zip" };

                case "Nintendo Wii":
                    return new[] { ".iso", ".dol", ".ciso", ".wbfs", ".wad", ".gcz" };

                case "Nintendo Wii U":
                    return new[] { ".disc", ".wud", ".wux", ".iso", ".wad", ".rpx" };

                case "DOS":
                    return new[] { ".gbdos", ".disc", ".exe", ".com", ".bat" };

                case "Windows":
                    return new[] { ".gbwin", ".disc" };

                case "Sega 32X":
                    return new[] { ".iso", ".bin", ".img", ".zip", ".32x", ".7z", ".md", ".smd" };

                case "Sega CD":
                    return new[] { ".iso", ".bin", ".img" };

                case "Dreamcast":
                    return new[] { ".chd", ".gdi", ".cdi", ".cue" };

                case "Game Gear":
                    return new[] { ".gg", ".zip", ".sms", ".bin", ".7z" };

                case "Sega Genesis":
                    return new[] { ".smd", ".bin", ".gen", ".zip", ".md", ".sg", ".sc", ".7z" };

                case "Sega Master System":
                    return new[] { ".sms", ".sg", ".sc", ".bin", ".rom", ".zip", ".7z" };

                case "Sega Mega Drive":
                    return new[] { ".smd", ".bin", ".gen", ".zip", ".md", ".sg", ".sc", ".7z" };

                case "Sega Saturn":
                    return new[] { ".iso", ".bin", ".img", ".cue", ".mdf", ".chd", ".ccd", ".toc" };

                case "Sony Playstation":
                    return new[] { ".iso", ".cue", ".img", ".ps1", ".pbp", ".cbn", ".mdf", ".pbp", ".toc", ".z", ".znx" };

                case "PS2":
                    return new[] { ".iso", ".bin", ".img", ".mdf", ".z", ".z2", ".bz2", ".dump", ".cso", ".ima", ".gz" };

                case "PS3":
                    return new[] { ".disc", ".iso" };

                case "PS4":
                    return new[] { ".disc", ".iso" };

                case "PSP":
                    return new[] { ".iso", ".cso", ".pbp", ".elf", ".prx" };

                case "TurboGrafx 16":
                    return new[] { ".pce", ".sgx", ".cue", ".ccd", ".chd", ".zip", ".7z" };

                case "TurboGrafx CD":
                    return new[] { ".bin", ".iso" };

                case "ZX Spectrum":
                    return new[] { ".sna", ".szx", ".z80", ".tap", ".tzx", ".gz", ".udi", ".mgt", ".img", ".trd", ".scl", ".dsk" };

                default:
                    return new string[] { };
            }

        }
    }
}
