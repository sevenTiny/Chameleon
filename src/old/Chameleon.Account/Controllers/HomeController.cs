using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chameleon.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Chameleon.Application;
using SevenTiny.Bantina.Extensions.AspNetCore;

namespace Chameleon.Account.Controllers
{
    public class HomeController : WebControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        IStatisticsApp _statisticsApp;
        public HomeController(IStatisticsApp statisticsApp, ILogger<HomeController> logger)
        {
            _statisticsApp = statisticsApp;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //获取用户信息到页面
            SetUserInfoToViewData();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult Http403()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Http404()
        {
            return View();
        }

        public IActionResult Abount()
        {
            return View();
        }

        public IActionResult AbountStatisticsJson()
        {
            return ResponseModel.Success(data: _statisticsApp.GetUserAccountStatistics()).ToJsonResult();
        }
    }
}
