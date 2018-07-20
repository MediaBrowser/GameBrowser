using System;
using System.Linq;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{

    class ResolverHelper
    {
        private static ILookup<string, GameSystemDefinition> extendedInfoFromConsoleType;
        private static ILookup<string, GameSystemDefinition> extendedInfoFromGameSystem;

        static ResolverHelper()
        {
            extendedInfoFromConsoleType = GameSystemDefinition.All.ToLookup(system => system.ConsoleType, StringComparer.OrdinalIgnoreCase);
            extendedInfoFromGameSystem = GameSystemDefinition.All.ToLookup(system => system.Name, StringComparer.OrdinalIgnoreCase);
        }

        public static GameSystemDefinition GetExtendedInfoFromPlatform(string platform)
        {
            // platform == consoleType
            return extendedInfoFromConsoleType[platform].FirstOrDefault();
        }
        public static GameSystemDefinition GetExtendedInfoFromConsoleType(string consoleType)
        {
            return extendedInfoFromConsoleType[consoleType].FirstOrDefault();
        }
        public static GameSystemDefinition GetExtendedInfoFromGameSystem(string gameSystem)
        {
            return extendedInfoFromGameSystem[gameSystem].FirstOrDefault();
        }

        public static string AttemptGetGamePlatformTypeFromPath(IFileSystem fileSystem, string path)
        {
            var system = Plugin.Instance.Configuration.GameSystems.FirstOrDefault(s => fileSystem.ContainsSubPath(s.Path, path) || string.Equals(s.Path, path, StringComparison.OrdinalIgnoreCase));

            return system?.ConsoleType;
        }

        public static string GetGameSystemFromPath(IFileSystem fileSystem, string path)
        {
            var platform = AttemptGetGamePlatformTypeFromPath(fileSystem, path);

            if (string.IsNullOrEmpty(platform))
            {
                return null;
            }

            return ResolverHelper.GetExtendedInfoFromPlatform(platform)?.Name;
        }
    }
}
