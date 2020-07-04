using Chameleon.Bootstrapper;
using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Infrastructure.Consts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Linq;

namespace Chameleon.Account.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [Authorize]
    public class WebControllerBase : WebControllerCommonBase
    {

    }
}