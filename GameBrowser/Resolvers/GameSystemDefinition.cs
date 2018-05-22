namespace GameBrowser.Resolvers
{
    public class GameSystemDefinition
    {
        public string Name { get; internal set; }
        public string ConsoleType { get; internal set; }
        public string[] Extensions { get; internal set; }

        public int TgbdId { get; internal set; }
        public string TgbdPlatform { get; internal set; }
        public string EmuMoviesPlatform { get; internal set; }

        public static GameSystemDefinition[] All = new[]
        {
                // Arcade
                new GameSystemDefinition
                {
                    ConsoleType = "Arcade",
                    Name = "Arcade",
                    Extensions = new[] { ".zip" },
                    TgbdId = 23,
                    TgbdPlatform = "Arcade",
                    EmuMoviesPlatform = "MAME"
                },

                // Atari - 2600
                new GameSystemDefinition
                {
                    ConsoleType = "Atari 2600",
                    Name = "Atari2600",
                    Extensions = new[] { ".bin", ".a26" },
                    TgbdId = 22,
                    TgbdPlatform = "Atari 2600",
                    EmuMoviesPlatform = "Atari_2600"
                },
                // Atari - 5200
                new GameSystemDefinition
                {
                    ConsoleType = "Atari 5200",
                    Name = "Atari5200",
                    Extensions = new[] { ".bin", ".a52" },
                    TgbdId = 26,
                    TgbdPlatform = "Atari 5200",
                    EmuMoviesPlatform = "Atari_5200"
                },
                // Atari - 7800
                new GameSystemDefinition
                {
                    ConsoleType = "Atari 7800",
                    Name = "Atari7800",
                    Extensions = new[] { ".a78" },
                    TgbdId = 27,
                    TgbdPlatform = "Atari 7800",
                    EmuMoviesPlatform = "Atari_7800"
                },
                // Atari - Jaguar
                new GameSystemDefinition
                {
                    ConsoleType = "Atari Jaguar",
                    Name = "AtariJaguar",
                    Extensions = new[] { ".j64", ".zip" },
                    TgbdId = 28,
                    TgbdPlatform = "Atari Jaguar",
                    EmuMoviesPlatform = "Atari_Jaguar"
                },
                // Atari - Jaguar CD
                new GameSystemDefinition
                {
                    ConsoleType = "Atari Jaguar CD",
                    Name = "AtariJaguarCD",
                    Extensions = new[] { ".iso" },
                    TgbdId = 29,
                    TgbdPlatform = "Atari Jaguar CD",
                    EmuMoviesPlatform = "Atari_Jaguar"
                },
                // Atari - Lynx
                new GameSystemDefinition
                {
                    ConsoleType = "Atari Lynx",
                    Name = "AtariLynx",
                    Extensions = new[] { ".lnx" },
                    TgbdId = 4924,
                    TgbdPlatform = "Atari Lynx",
                    EmuMoviesPlatform = null
                },
                // Atari - ST
                new GameSystemDefinition
                {
                    ConsoleType = "Atari ST",
                    Name = "AtariST",
                    Extensions = new[] { ".ipf" },
                    TgbdId = 4937,
                    TgbdPlatform = "Atari ST",
                    EmuMoviesPlatform = null
                },
                // Atari - XE
                new GameSystemDefinition
                {
                    ConsoleType = "Atari XE",
                    Name = "AtariXE",
                    Extensions = new[] { ".rom" },
                    TgbdId = 30,
                    TgbdPlatform = "Atari XE",
                    EmuMoviesPlatform = "Atari_8_bit"
                },

                // Coleco - Colecovision
                new GameSystemDefinition
                {
                    ConsoleType = "Colecovision",
                    Name = "Colecovision",
                    Extensions = new[] { ".col", ".rom" },
                    TgbdId = 31,
                    TgbdPlatform = "Colecovision",
                    EmuMoviesPlatform = "Coleco_Vision"
                },
                
                // Commodore - 64
                new GameSystemDefinition
                {
                    ConsoleType = "Commodore 64",
                    Name = "Commodore64",
                    Extensions = new[] { ".crt", ".d64", ".g64", ".prg", ".tap", ".t64" },
                    TgbdId = 40,
                    TgbdPlatform = "Commodore 64",
                    EmuMoviesPlatform = "Commodore_64"
                },
                // Commodore - Amiga
                new GameSystemDefinition
                {
                    ConsoleType = "Amiga",
                    Name = "Amiga",
                    Extensions = new[] { ".iso", ".adf", ".ipf" },
                    TgbdId = 4911,
                    TgbdPlatform = "Amiga",
                    EmuMoviesPlatform = null
                },
                // Commodore - Vic-20
                new GameSystemDefinition
                {
                    ConsoleType = "Commodore Vic-20",
                    Name = "CommodoreVic20",
                    Extensions = new[] { ".prg" },
                    TgbdId = 4945,
                    TgbdPlatform = "Commodore Vic-20",
                    EmuMoviesPlatform = null
                },

                // Matel - Intellivision
                new GameSystemDefinition
                {
                    ConsoleType = "Intellivision",
                    Name = "Intellivision",
                    Extensions = new[] { ".int", ".rom" },
                    TgbdId = 32,
                    TgbdPlatform = "Intellivision",
                    EmuMoviesPlatform = "Mattel_Intellivision"
                },

                // Microsoft - DOS
                new GameSystemDefinition
                {
                    ConsoleType = "DOS",
                    Name = "DOS",
                    Extensions = new[] { ".gbdos", ".disc" },
                    TgbdId = 1,
                    TgbdPlatform = "PC",
                    EmuMoviesPlatform = null
                },
                // Microsoft - PC
                new GameSystemDefinition
                {
                    ConsoleType = "PC",
                    Name = "PC",
                    Extensions = new string[] { },
                    TgbdId = 1,
                    TgbdPlatform = "PC",
                    EmuMoviesPlatform = null
                },
                // Microsoft - Xbox
                new GameSystemDefinition
                {
                    ConsoleType = "Xbox",
                    Name = "MicrosoftXBox",
                    Extensions = new[] { ".disc", ".iso" },
                    TgbdId = 14,
                    TgbdPlatform = "Microsoft Xbox",
                    EmuMoviesPlatform = "Microsoft_Xbox"
                },
                // Microsoft - Xbox 360
                new GameSystemDefinition
                {
                    ConsoleType = "Xbox 360",
                    Name = "MicrosoftXBox360",
                    Extensions = new[] { ".disc" },
                    TgbdId = 15,
                    TgbdPlatform = "Microsoft Xbox 360",
                    EmuMoviesPlatform = null
                },
                // Microsoft - Xbox One
                new GameSystemDefinition
                {
                    ConsoleType = "Xbox One",
                    Name = "MicrosoftXBoxOne",
                    Extensions = new[] { ".disc" },
                    TgbdId = 4920,
                    TgbdPlatform = "Microsoft Xbox One",
                    EmuMoviesPlatform = null
                },
                // Microsoft - Windows
                new GameSystemDefinition
                {
                    ConsoleType = "Windows",
                    Name = "Windows",
                    Extensions = new[] { ".gbwin", ".disc" },
                    TgbdId = 1,
                    TgbdPlatform = "PC",
                    EmuMoviesPlatform = null
                },

                // NEC - PC Engine - TurboGrafx 16
                new GameSystemDefinition
                {
                    ConsoleType = "TurboGrafx 16",
                    Name = "TurboGrafx16",
                    Extensions = new[] { ".pce", ".zip" },
                    TgbdId = 34,
                    TgbdPlatform = "TurboGrafx 16",
                    EmuMoviesPlatform = "NEC_TurboGrafx_16"
                },
                // NEC - PC Engine - TurboGrafx CD
                new GameSystemDefinition
                {
                    ConsoleType = "TurboGrafx CD",
                    Name = "TurboGrafxCD",
                    Extensions = new[] { ".bin", ".iso" },
                    TgbdId = 34,
                    TgbdPlatform = "TurboGrafx CD",
                    EmuMoviesPlatform = "NEC_TurboGrafx_16"
                },

                // Nintendo - Game Boy
                new GameSystemDefinition
                {
                    ConsoleType = "Game Boy",
                    Name = "NintendoGameBoy",
                    Extensions = new[] { ".gb", ".zip" },
                    TgbdId = 4,
                    TgbdPlatform = "Nintendo Game Boy",
                    EmuMoviesPlatform = "Nintendo_Game_Boy"
                },
                // Nintendo - Game Boy Advance
                new GameSystemDefinition
                {
                    ConsoleType = "Game Boy Advance",
                    Name = "NintendoGameBoyAdvance",
                    Extensions = new[] { ".gba", ".zip" },
                    TgbdId = 5,
                    TgbdPlatform = "Nintendo Game Boy Advance",
                    EmuMoviesPlatform = "Nintendo_Game_Boy_Advance"
                },
                // Nintendo - Game Boy Color
                new GameSystemDefinition
                {
                    ConsoleType = "Game Boy Color",
                    Name = "NintendoGameBoyColor",
                    Extensions = new[] { ".gbc", ".zip" },
                    TgbdId = 41,
                    TgbdPlatform = "Nintendo Game Boy Color",
                    EmuMoviesPlatform = "Nintendo_Game_Boy_Color"
                },
                // Nintendo - GameCube
                new GameSystemDefinition
                {
                    ConsoleType = "Gamecube",
                    Name = "NintendoGameCube",
                    Extensions = new[] { ".iso", ".bin", ".img", ".gcm", ".gcz" },
                    TgbdId = 2,
                    TgbdPlatform = "Nintendo GameCube",
                    EmuMoviesPlatform = "Nintendo_GameCube"
                },
                // Nintendo - Nintendo 64
                new GameSystemDefinition
                {
                    ConsoleType = "Nintendo 64",
                    Name = "Nintendo64",
                    Extensions = new[] { ".z64", ".v64", ".usa", ".jap", ".pal", ".rom", ".n64", ".zip" },
                    TgbdId = 3,
                    TgbdPlatform = "Nintendo 64",
                    EmuMoviesPlatform = "Nintendo_N64"
                },
                // Nintendo - Nintendo DS
                new GameSystemDefinition
                {
                    ConsoleType = "Nintendo DS",
                    Name = "NintendoDS",
                    Extensions = new[] { ".nds", ".zip" },
                    TgbdId = 8,
                    TgbdPlatform = "Nintendo DS",
                    EmuMoviesPlatform = "Nintendo_DS"
                },
                // Nintendo - Nintendo Entertainment System
                new GameSystemDefinition
                {
                    ConsoleType = "Nintendo",
                    Name = "Nintendo",
                    Extensions = new[] { ".nes", ".zip" },
                    TgbdId = 7,
                    TgbdPlatform = "Nintendo Entertainment System (NES)",
                    EmuMoviesPlatform = "Nintendo_NES"
                },
                // Nintendo - Super Nintendo Entertainment System
                new GameSystemDefinition
                {
                    ConsoleType = "Super Nintendo",
                    Name = "SuperNintendo",
                    Extensions = new[] { ".smc", ".zip", ".fam", ".rom", ".sfc", ".fig" },
                    TgbdId = 6,
                    TgbdPlatform = "Super Nintendo (SNES)",
                    EmuMoviesPlatform = "Nintendo_SNES"
                },
                // Nintendo - Virtual Boy 
                new GameSystemDefinition
                {
                    ConsoleType = "Virtual Boy",
                    Name = "VirtualBoy",
                    Extensions = new[] { ".vb" },
                    TgbdId = 4918,
                    TgbdPlatform = "Nintendo Virtual Boy",
                    EmuMoviesPlatform = null
                },
                // Nintendo - Wii
                new GameSystemDefinition
                {
                    ConsoleType = "Nintendo Wii",
                    Name = "Wii",
                    Extensions = new[] { ".iso", ".dol", ".ciso", ".wbfs", ".wad", ".gcz" },
                    TgbdId = 9,
                    TgbdPlatform = "Nintendo Wii",
                    EmuMoviesPlatform = null
                },
                // Nintendo - Wii U
                new GameSystemDefinition
                {
                    ConsoleType = "Nintendo Wii U",
                    Name = "WiiU",
                    Extensions = new[] { ".disc", ".wud" },
                    TgbdId = 38,
                    TgbdPlatform = "Nintendo Wii U",
                    EmuMoviesPlatform = ""
                },

                // Panasonic - 3DO
                new GameSystemDefinition
                {
                    ConsoleType = "3DO",
                    Name = "Panasonic3DO",
                    Extensions = new[] { ".iso", ".cue" },
                    TgbdId = 25,
                    TgbdPlatform = "3DO",
                    EmuMoviesPlatform = "Panasonic_3DO"
                },

                // Sega - 32X
                new GameSystemDefinition
                {
                    ConsoleType = "Sega 32X",
                    Name = "Sega32X",
                    Extensions = new[] { ".iso", ".bin", ".img", ".zip", ".32x" },
                    TgbdId = 33,
                    TgbdPlatform = "Sega 32X",
                    EmuMoviesPlatform = "Sega_Genesis"
                },
                // Sega - CD
                new GameSystemDefinition
                {
                    ConsoleType = "Sega CD",
                    Name = "SegaCD",
                    Extensions = new[] { ".iso", ".bin", ".img" },
                    TgbdId = 21,
                    TgbdPlatform = "Sega CD",
                    EmuMoviesPlatform = "Sega_Genesis"
                },
                // Sega - Dreamcast
                new GameSystemDefinition
                {
                    ConsoleType = "Dreamcast",
                    Name = "SegaDreamcast",
                    Extensions = new[] { ".chd", ".gdi", ".cdi" },
                    TgbdId = 16,
                    TgbdPlatform = "Sega Dreamcast",
                    EmuMoviesPlatform = "Sega_Dreamcast"
                },
                // Sega - Game Gear
                new GameSystemDefinition
                {
                    ConsoleType = "Game Gear",
                    Name = "SegaGameGear",
                    Extensions = new[] { ".gg", ".zip" },
                    TgbdId = 20,
                    TgbdPlatform = "Sega Game Gear",
                    EmuMoviesPlatform = "Sega_Game_Gear"
                },
                // Sega - Mega Drive
                new GameSystemDefinition
                {
                    ConsoleType = "Sega Mega Drive",
                    Name = "SegaMegaDrive",
                    Extensions = new[] { ".smd", ".zip", ".md" },
                    TgbdId = 36,
                    TgbdPlatform = "Sega Genesis",
                    EmuMoviesPlatform = "Sega_Genesis"
                },
                // Sega - Mega Drive - Genesis
                new GameSystemDefinition
                {
                    ConsoleType = "Sega Genesis",
                    Name = "SegaGenesis",
                    Extensions = new[] { ".smd", ".bin", ".gen", ".zip", ".md" },
                    TgbdId = 18,
                    TgbdPlatform = "Sega Genesis",
                    EmuMoviesPlatform = "Sega_Genesis"
                },
                // Sega - Master System - Mark III
                new GameSystemDefinition
                {
                    ConsoleType = "Sega Master System",
                    Name = "SegaMasterSystem",
                    Extensions = new[] { ".sms", ".sg", ".sc", ".zip" },
                    TgbdId = 35,
                    TgbdPlatform = "Sega Master System",
                    EmuMoviesPlatform = "Sega_Master_System"
                },
                // Sega - Saturn
                new GameSystemDefinition
                {
                    ConsoleType = "Sega Saturn",
                    Name = "SegaSaturn",
                    Extensions = new[] { ".iso", ".bin", ".img" },
                    TgbdId = 17,
                    TgbdPlatform = "Sega Saturn",
                    EmuMoviesPlatform = "Sega_Saturn"
                },

                // Sinclair - ZX Spectrum 
                new GameSystemDefinition
                {
                    ConsoleType = "ZX Spectrum",
                    Name = "ZxSpectrum",
                    Extensions = new[] { ".z80", ".tap", ".tzx" },
                    TgbdId = 4913,
                    TgbdPlatform = "ZX Spectrum",
                    EmuMoviesPlatform = null
                },

                // SNK - Neo Geo
                new GameSystemDefinition
                {
                    ConsoleType = "Neo Geo",
                    Name = "NeoGeo",
                    Extensions = new[] { ".zip", ".iso" },
                    TgbdId = 24,
                    TgbdPlatform = "NeoGeo",
                    EmuMoviesPlatform = "SNK_Neo_Geo_AES"
                },
                // SNK - Neo Geo Pocket
                new GameSystemDefinition
                {
                    ConsoleType = "Neo Geo Pocket",
                    Name = "NeoGeoPocket",
                    Extensions = new[] { ".ngp" },
                    TgbdId = 4922,
                    TgbdPlatform = "Neo Geo Pocket",
                    EmuMoviesPlatform = null
                },
                // SNK - Neo Geo Pocket Color 
                new GameSystemDefinition
                {
                    ConsoleType = "Neo Geo Pocket Color",
                    Name = "NeoGeoPocketColor",
                    Extensions = new[] { ".ngc" },
                    TgbdId = 4923,
                    TgbdPlatform = "Neo Geo Pocket Color",
                    EmuMoviesPlatform = null
                },

                // Sony - Playstation
                new GameSystemDefinition
                {
                    ConsoleType = "Sony Playstation",
                    Name = "SonyPlaystation",
                    Extensions = new[] { ".iso", ".cue", ".img", ".ps1", ".pbp" },
                    TgbdId = 10,
                    TgbdPlatform = "Sony Playstation",
                    EmuMoviesPlatform = "Sony_Playstation"
                },
                // Sony - Playstation 2
                new GameSystemDefinition
                {
                    ConsoleType = "PS2",
                    Name = "SonyPlaystation2",
                    Extensions = new[] { ".iso", ".bin" },
                    TgbdId = 11,
                    TgbdPlatform = "Sony Playstation 2",
                    EmuMoviesPlatform = "Sony_Playstation_2"
                },
                // Sony - Playstation 3
                new GameSystemDefinition
                {
                    ConsoleType = "PS3",
                    Name = "SonyPlaystation3",
                    Extensions = new[] { ".disc" },
                    TgbdId = 12,
                    TgbdPlatform = "Sony Playstation 3",
                    EmuMoviesPlatform = ""
                },
                // Sony - Playstation 4
                new GameSystemDefinition
                {
                    ConsoleType = "PS4",
                    Name = "SonyPlaystation4",
                    Extensions = new[] { ".disc" },
                    TgbdId = 4919,
                    TgbdPlatform = "Sony Playstation 4",
                    EmuMoviesPlatform = ""
                },
                // Sony - PlayStation Portable
                new GameSystemDefinition
                {
                    ConsoleType = "PSP",
                    Name = "SonyPSP",
                    Extensions = new[] { ".iso", ".cso" },
                    TgbdId = 13,
                    TgbdPlatform = "Sony PSP",
                    EmuMoviesPlatform = "Sony_PSP"
                }
        };

        // This method was not used. Making a commit to have it in history then deleting.
        // public static string GetDisplayMediaTypeFromPlatform(string platform)
        //     "3DO" => "Panasonic3doGame";
        //     "Atari XE" => "AtariXeGame";
        //     "Atari Jaguar" => "JaguarGame";
        //     "Atari Jaguar CD"=>"JaguarGame";
        //     "Commodore 64" => "C64Game";
        //     "Commodore Vic-20" => "Vic20Game";
        //     "Xbox" => "XboxGame";
        //     "Xbox 360" => "Xbox360Game";
        //     "Xbox One" => "XboxOneGame";
        //     "Nintendo 64" => "N64Game";
        //     "Nintendo DS" => "NesGame";
        //     "Nintendo" => "NesGame";
        //     "Game Boy" => "GameBoyGame";
        //     "Game Boy Advance" => "GameBoyAdvanceGame";
        //     "Game Boy Color" => "GameBoyColorGame";
        //     "Gamecube" => "GameCubeGame";
        //     "Super Nintendo" => "SnesGame";
        //     "Virtual Boy" => "NesGame";
        //     "Nintendo Wii" => "NesGame";
        //     "Nintendo Wii U" => "WiiUGame";
        //     "DOS" => "DosGame";
        //     "Windows" => "WindowsGame";
        //     "Sega 32X" => "GenesisGame";
        //     "Sega CD" => "GenesisGame";
        //     "Dreamcast" => "GenesisGame";
        //     "Game Gear" => "GenesisGame";
        //     "Sega Genesis" => "GenesisGame";
        //     "Sega Master System" => "GenesisGame";
        //     "Sega Mega Drive" => "GenesisGame";
        //     "Sega Saturn" => "GenesisGame";
        //     "Sony Playstation" => "PsOneGame";
        //     "PS2" => "Ps2Game";
        //     "PS3" => "Ps3Game";
        //     "PS4" => "Ps4Game";
        //     "PSP" => "PlayStationPortableGame";
        //     "TurboGrafx 16" => "TurboGrafx16Game";
        //     "TurboGrafx CD" => "TurboGrafx16Game";
        //     "ZX Spectrum" => "ZXSpectrumGame";
    }
}
