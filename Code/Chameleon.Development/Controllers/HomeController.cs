using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Chameleon.Development.Models;
using Chameleon.Domain;

namespace Chameleon.Development.Controllers
{
    public class HomeController : WebControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        ICloudApplicationService _cloudApplicationService;
        public HomeController(ICloudApplicationService cloudApplicationService, ILogger<HomeController> logger)
        {
            _cloudApplicationService = cloudApplicationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //未删除的应用
            ViewData["CloudApplicationList"] = _cloudApplicationService.GetListUnDeleted();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
