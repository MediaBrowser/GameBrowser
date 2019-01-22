using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    class ResolverHelper
    {
        public static string AttemptGetGamePlatformTypeFromPath(IFileSystem fileSystem, string path)
        {
            var system = Plugin.Instance.Configuration.GameSystems.FirstOrDefault(s => fileSystem.ContainsSubPath(s.Path, path) || string.Equals(s.Path, path, StringComparison.OrdinalIgnoreCase));

            return system != null ? system.ConsoleType : null;
        }

        public static string GetGameSystemFromPlatform(string platform)
        {
            if (string.IsNullOrEmpty(platform))
            {
                throw new ArgumentNullException("platform");
            }

            switch (platform)
            {
                case "3DO":
                    return "Panasonic3DO";

                case "Amiga":
                    return "Amiga";

                case "Arcade":
                    return "Arcade";

                case "Atari 2600":
                    return "Atari2600";

                case "Atari 5200":
                    return "Atari5200";

                case "Atari 7800":
                    return "Atari7800";

                case "Atari XE":
                    return "AtariXE";

                case "Atari Jaguar":
                    return "AtariJaguar";

                case "Atari Jaguar CD":
                    return "AtariJaguarCD";

                case "Colecovision":
                    return "Colecovision";

                case "Commodore 64":
                    return "Commodore64";

                case "Commodore Vic-20":
                    return "CommodoreVic20";

                case "Intellivision":
                    return "Intellivision";

                case "Xbox":
                    return "MicrosoftXBox";

                case "Xbox 360":
                    return "MicrosoftXBox360";

                case "Xbox One":
                    return "MicrosoftXBoxOne";

                case "Neo Geo":
                    return "NeoGeo";

                case "Nintendo 64":
                    return "Nintendo64";

                case "Nintendo DS":
                    return "NintendoDS";

                case "Nintendo":
                    return "Nintendo";

                case "Game Boy":
                    return "NintendoGameBoy";

                case "Game Boy Advance":
                    return "NintendoGameBoyAdvance";

                case "Game Boy Color":
                    return "NintendoGameBoyColor";

                case "Gamecube":
                    return "NintendoGameCube";

                case "Super Nintendo":
                    return "SuperNintendo";

                case "Virtual Boy":
                    return "VirtualBoy";

                case "Nintendo Wii":
                    return "Wii";

                case "Nintendo Wii U":
                    return "WiiU";

                case "DOS":
                    return "DOS";

                case "Windows":
                    return "Windows";

                case "Sega 32X":
                    return "Sega32X";

                case "Sega CD":
                    return "SegaCD";

                case "Dreamcast":
                    return "SegaDreamcast";

                case "Game Gear":
                    return "SegaGameGear";

                case "Sega Genesis":
                    return "SegaGenesis";

                case "Sega Master System":
                    return "SegaMasterSystem";

                case "Sega Mega Drive":
                    return "SegaMegaDrive";

                case "Sega Saturn":
                    return "SegaSaturn";

                case "Sony Playstation":
                    return "SonyPlaystation";

                case "PS2":
                    return "SonyPlaystation2";

                case "PS3":
                    return "SonyPlaystation3";

                case "PS4":
                    return "SonyPlaystation4";

                case "PSP":
                    return "SonyPSP";

                case "TurboGrafx 16":
                    return "TurboGrafx16";

                case "TurboGrafx CD":
                    return "TurboGrafxCD";

                case "ZX Spectrum":
                    return "ZxSpectrum";

            }
            return null;
        }
    }
}
