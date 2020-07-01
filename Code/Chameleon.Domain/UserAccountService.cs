using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Security;
using SevenTiny.Cloud.ScriptEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Domain
{
    public interface IUserAccountService
    {
        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Result AddUserAccount(UserAccount userAccount);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Result ChangePassword(Guid id, string password);
        /// <summary>
        /// 验证密码并获取账号信息
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Result<UserAccount> VerifyPassword(string phone, string email, string password);
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result PresetPassword(Guid id);
        /// <summary>
        /// 获取能正常显示的名称
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        string GetViewName(UserAccount userAccount);
    }

    public class UserAccountService : IUserAccountService
    {
        IUserAccountRepository _userAccountRepository;
        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }

        private string GetSaltPassword(string password)
        {
            return MD5Helper.GetMd5Hash(string.Concat("Chameleon_", password, "_Chameleon"));
        }

        public Result AddUserAccount(UserAccount userAccount)
        {
            //校验邮箱和手机是否被注册
            if (_userAccountRepository.CheckEmailOrPhoneExist(userAccount.Email, userAccount.Phone))
                return Result.Error("该联系方式已经被注册");

            //密码加盐
            userAccount.Password = GetSaltPassword(userAccount.Password);
            userAccount.Role = (int)RoleEnum.User;

            return _userAccountRepository.Add(userAccount);
        }

        public Result ChangePassword(Guid id, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result.Error("密码不能为空");

            var userAccount = _userAccountRepository.GetById(id);

            if (userAccount == null)
                return Result.Error("该账号不存在");

            userAccount.Password = GetSaltPassword(password);

            userAccount.ModifyTime = DateTime.Now;

            return _userAccountRepository.Update(userAccount);
        }

        public Result<UserAccount> VerifyPassword(string phone, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result<UserAccount>.Error("密码不能为空");

            var userAccount = _userAccountRepository.GetUserAccountByEmailOrPhone(email, phone);

            if (userAccount == null)
                return Result<UserAccount>.Error("账号不存在");

            if (userAccount.Password.Equals(GetSaltPassword(password)))
                return Result<UserAccount>.Error("密码不正确");

            return Result<UserAccount>.Success(data: userAccount);
        }

        public Result PresetPassword(Guid id)
        {
            var userAccount = _userAccountRepository.GetById(id);

            if (userAccount == null)
                return Result.Error("该账号不存在");

            userAccount.IsNeedToResetPassword = 1;

            return _userAccountRepository.Update(userAccount);
        }

        public string GetViewName(UserAccount userAccount)
        {
            if (!string.IsNullOrEmpty(userAccount.Name))
                return userAccount.Name;

            if (!string.IsNullOrEmpty(userAccount.Phone))
                return userAccount.Phone;

            if (!string.IsNullOrEmpty(userAccount.Email))
                return userAccount.Email;

            return string.Empty;
        }
    }
}
