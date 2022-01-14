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
            var system = Plugin.Instance.Configuration.GameSystems.FirstOrDefault(s => fileSystem.ContainsSubPath(s.Path.AsSpan(), path.AsSpan()) || string.Equals(s.Path, path, StringComparison.OrdinalIgnoreCase));

            return system != null ? system.ConsoleType : null;
        }
    }
}
