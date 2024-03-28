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
using MediaBrowser.Model.Dto;

namespace GameBrowser.Resolvers
{
    /// <summary>
    /// Class GameResolver
    /// </summary>
    public class GameResolver : BaseItemResolver<Game>
    {
        private IFileSystem _fileSystem;

        public GameResolver(ILibraryManager libraryManager, ILogger logger, IFileSystem fileSystem)
            : base(libraryManager, logger)
        {
            _fileSystem = fileSystem;
        }

        protected override bool IsSupportedFile(FileSystemMetadata file, LibraryOptions libraryOptions)
        {
            if (!file.IsDirectory)
            {
                var path = file.FullName;

                var platform = ResolverHelper.GetGamePlatformFromPath(_fileSystem, path);

                if (platform == null) return false;

                var extension = file.Extension;

                // For MAME we will allow all games in the same dir
                if (string.Equals(platform.ConsoleType, "Arcade", StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".7z", StringComparison.OrdinalIgnoreCase))
                    {
                        // ignore zips that are bios roms.
                        if (MameUtils.IsBiosRom(path)) return false;

                        return true;
                    }
                }
                else
                {
                    var validExtensions = GetExtensions(platform.ConsoleType);

                    if (!validExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }

            return false;
        }

        protected override void OnItemFound<T>(T item, Folder parent)
        {
            base.OnItemFound(item, parent);

            var path = item.Path;
            var platform = ResolverHelper.GetGamePlatformFromPath(_fileSystem, path);

            var gameSystem = new LinkedItemInfo
            {
                Name = Path.GetFileName(platform.Path)
            };
            gameSystem.ProviderIds["console"] = platform.ConsoleType;

            item.SetAlbumItem(gameSystem);

            var extension = Path.GetExtension(path);

            if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase) || string.Equals(extension, ".7z", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(platform.ConsoleType, "Arcade", StringComparison.OrdinalIgnoreCase))
                {
                    var name = MameUtils.GetFullNameFromPath(path, Logger);
                    if (!string.IsNullOrEmpty(name))
                    {
                        item.Name = name;
                    }
                }
            }

            item.Container = extension.TrimStart('.');
        }

        protected override bool SupportsLibrary(LibraryOptions libraryOptions)
        {
            var contentType = libraryOptions.ContentType;

            return contentType.AsSpan().Equals(CollectionType.Games.Span, StringComparison.OrdinalIgnoreCase);
        }

        private string[] GetExtensions(string consoleType)
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
                    return new[] { ".disc", ".wud", ".wux", ".wua" };

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

                case "PSVita":
                    return new[] { ".psv" };

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
