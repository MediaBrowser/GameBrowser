using System;
using System.Collections.Generic;
using System.Linq;
using GameBrowser.Configuration;
using MediaBrowser.Model.IO;

namespace GameBrowser.Resolvers
{
    class ResolverHelper
    {
        public static ConsoleFolderConfiguration GetGamePlatformFromPath(IFileSystem fileSystem, string path)
        {
            return Plugin.Instance.Configuration.GameSystems.FirstOrDefault(s => fileSystem.ContainsSubPath(s.Path.AsSpan(), path.AsSpan()) || string.Equals(s.Path, path, StringComparison.OrdinalIgnoreCase));
        }
    }
}
