using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chameleon.Development.Models;
using Chameleon.Domain;
using Chameleon.Application;
using SevenTiny.Bantina.Extensions.AspNetCore;

namespace Chameleon.Development.Controllers
{
    public class StatisticsController : WebControllerBase
    {
        IStatisticsApp _statisticsApp;
        public StatisticsController(IStatisticsApp statisticsApp)
        {
            _statisticsApp = statisticsApp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CurrentApplication(Guid applicationId)
        {
            ViewData["ApplicationId"] = applicationId;
            return View();
        }

        public IActionResult CurrentApplicationJson(Guid applicationId)
        {
            return ResponseModel.Success(data: _statisticsApp.GetDevelopmentStatistics(applicationId)).ToJsonResult();
        }
    }
}
