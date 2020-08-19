using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions.AspNetCore;
using System;
using System.Text.RegularExpressions;

namespace Chameleon.DataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ApiControllerCommonBase
    {
        IOrganizationService _organizationService;
        IUserAccountRepository _userAccountRepository;
        IUserAccountService _userAccountService;
        IUserAccountApp _userAccountApp;
        public UserAccountController(IUserAccountApp userAccountApp, IUserAccountService userAccountService, IUserAccountRepository userAccountRepository, IOrganizationService organizationService)
        {
            _userAccountApp = userAccountApp;
            _userAccountService = userAccountService;
            _userAccountRepository = userAccountRepository;
            _organizationService = organizationService;
        }

        private string GetTokenAndSaveCookie(UserAccount userAccount)
        {
            //get token
            var token = _userAccountService.GetToken(userAccount).Data;
            //set token to cookie
            Response.Cookies.Append(AccountConst.KEY_AccessToken, token);

            return token;
        }

        /// <summary>
        /// 第三方登陆，返回授权token
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult SignInThirdParty([FromBody]LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                return Result.Error("参数错误").ToJsonResult();

            //如果校验密码成功，则会返回账号信息
            var checkResult = _userAccountService.VerifyPassword(null, loginRequest.Email, loginRequest.Password);

            if (!checkResult.IsSuccess)
                return Result.Error("账号或密码不正确").ToJsonResult();

            LoginResponse loginResult = new LoginResponse
            {
                AccessToken = GetTokenAndSaveCookie(checkResult.Data),
                IsNeedToResetPassword = checkResult.Data.IsNeedToResetPassword,
                TokenExpiredTimeStamp = TimeHelper.GetTimeStamp(DateTime.Now.AddDays(1)),
            };

            return Result<LoginResponse>.Success("登陆成功", loginResult).ToJsonResult();
        }

        /// <summary>
        /// 第三方修改密码
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ResetPasswordThirdParty([FromBody]LoginRequest loginRequest)
        {
            var result = Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest, nameof(loginRequest))
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest.Email, nameof(loginRequest.Email))
                .ContinueEnsureArgumentNotNullOrEmpty(loginRequest.NewPassword, nameof(loginRequest.NewPassword))
                .ContinueAssert(_ => Regex.IsMatch(loginRequest.NewPassword, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,20}$"), "必须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-20之间")
                .Continue(_ =>
                {
                    return _userAccountService.ResetPassword(loginRequest.Email, loginRequest.NewPassword);
                });

            if (!result.IsSuccess)
                return result.ToJsonResult();

            return Result.Success("操作成功").ToJsonResult();
        }
    }
}