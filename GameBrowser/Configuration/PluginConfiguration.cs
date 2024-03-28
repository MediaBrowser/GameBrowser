using System;
using MediaBrowser.Model.Plugins;

namespace GameBrowser.Configuration
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// Gets or sets the game systems.
        /// </summary>
        /// <value>The game systems.</value>
        public ConsoleFolderConfiguration[] GameSystems { get; set; } = Array.Empty<ConsoleFolderConfiguration>();
    }

    /// <summary>
    /// Class ConsoleFolderConfiguration
    /// </summary>
    public class ConsoleFolderConfiguration
    {
        public string Path { get; set; }

        public string ConsoleType { get; set; }
    }
}
