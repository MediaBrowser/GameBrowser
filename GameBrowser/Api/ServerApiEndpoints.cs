using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Services;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Querying;

namespace GameBrowser.Api
{
    [Route("/GameBrowser/GamePlatforms", "GET")]
    public class GetConfiguredPlatforms
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class GameBrowserUriService : IService
    {
        private readonly ILogger _logger;
        private readonly ILibraryManager _libraryManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="libraryManager"></param>
        public GameBrowserUriService(ILogger logger, ILibraryManager libraryManager)
        {
            _logger = logger;
            _libraryManager = libraryManager;
        }

        /// <summary>
        /// Get all the game platforms that the user has added
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(GetConfiguredPlatforms request)
        {
            _logger.Debug("GetConfiguredPlatforms request received");

            return Plugin.Instance.Configuration.GameSystems;
        }
    }
}
