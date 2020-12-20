using Microsoft.AspNetCore.Mvc;

namespace Chameleon.Account.Controllers
{
    public class ChameleonMessageController : WebControllerBase
    {
        public ChameleonMessageController()
        {
        }

        /// <summary>
        /// 已读列表
        /// </summary>
        /// <returns></returns>
        public IActionResult ReadMessageList()
        {
            return View();
        }

        /// <summary>
        /// 未读列表
        /// </summary>
        /// <returns></returns>
        public IActionResult UnReadMessageList()
        {
            return View();
        }
    }
}
