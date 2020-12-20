using Chameleon.Common;
using Chameleon.Domain;
using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Application
{
    public interface IUserAccountApp
    {
        Result AddUserAccount(UserAccount userAccount, string token);
    }

    public class UserAccountApp : IUserAccountApp
    {
        IUserAccountService _userAccountService;
        public UserAccountApp(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public Result AddUserAccount(UserAccount userAccount, string token)
        {
            //添加
            var addResult = _userAccountService.AddUserAccount(userAccount);

            if (!addResult.IsSuccess)
                return addResult;

            //发送注册成功消息
            new InterfaceRequest(new Dictionary<string, string> { { "_AccessToken", token } }).CloudDataGet($"ChameleonSystem.TDS.SignUpUserAlert&userId={userAccount.UserId}");

            return Result.Success();
        }
    }
}
