using System;
using System.Collections.Generic;
using System.Linq;
using GameBrowser.Api.Querying;
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

    [Route("/GameBrowser/Games/Dos", "GET")]
    public class GetDosGames
    {
    }

    [Route("/GameBrowser/Games/Windows", "GET")]
    public class GetWindowsGames
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(GetDosGames request)
        {
            _logger.Debug("GetDosGames request received");

            var dosGames = _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { typeof(Game).Name },
                OrderBy = new[] { new ValueTuple<string, SortOrder>(ItemSortBy.SortName, SortOrder.Ascending) }
            })
                .Where(i => i is Game && !string.IsNullOrEmpty(((Game)i).GameSystem) && ((Game)i).GameSystem.Equals("DOS"))
                .ToList();

            var gameNameList = new List<String>();

            if (dosGames.Count > 0)
                gameNameList.AddRange(dosGames.Select(bi => bi.Name));

            return new GameQueryResult
            {
                TotalCount = gameNameList.Count,
                GameTitles = gameNameList.ToArray()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(GetWindowsGames request)
        {
            _logger.Debug("GetWindowsGames request received");

            var windowsGames = _libraryManager.GetItemList(new InternalItemsQuery
            {
                IncludeItemTypes = new[] { typeof(Game).Name },
                OrderBy = new[] { new ValueTuple<string, SortOrder>(ItemSortBy.SortName, SortOrder.Ascending) }
            })
                .Where(i => i is Game && !string.IsNullOrEmpty(((Game)i).GameSystem) && ((Game)i).GameSystem.Equals("Windows"))
                .ToList();

            var gameNameList = new List<String>();

            if (windowsGames.Count > 0)
                gameNameList.AddRange(windowsGames.Select(bi => bi.Name));

            return new GameQueryResult
            {
                TotalCount = gameNameList.Count,
                GameTitles = gameNameList.ToArray()
            };
        }
    }
}
