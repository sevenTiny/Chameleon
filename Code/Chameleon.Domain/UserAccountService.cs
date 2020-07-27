using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Infrastructure.Configs;
using Chameleon.Infrastructure.Consts;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Security;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.ScriptEngine;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chameleon.Domain
{
    public interface IUserAccountService : ICommonServiceBase<UserAccount>
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
        /// <param name="password"></param>
        /// <returns></returns>
        Result ResetPassword(Guid id, string password);
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
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        List<UserAccount> GetUserAccountList();
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        Result<string> GetToken(UserAccount userAccount);
        /// <summary>
        /// 设置下次登陆重置密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result SetNextTimeResetPassword(Guid id);
    }

    public class UserAccountService : CommonServiceBase<UserAccount>, IUserAccountService
    {
        IUserAccountRepository _userAccountRepository;
        IOrganizationRepository _organizationRepository;
        public UserAccountService(IOrganizationRepository organizationRepository, IUserAccountRepository userAccountRepository) : base(userAccountRepository)
        {
            _organizationRepository = organizationRepository;
            _userAccountRepository = userAccountRepository;
        }

        private string GetSaltPassword(string password)
        {
            return MD5Helper.GetMd5Hash(string.Concat(AccountConst.SALT_BEFORE, password, AccountConst.SALT_AFTER));
        }

        public Result AddUserAccount(UserAccount userAccount)
        {
            Ensure.ArgumentNotNullOrEmpty(userAccount, nameof(userAccount));
            Ensure.ArgumentNotNullOrEmpty(userAccount.Email, nameof(userAccount.Email));
            Ensure.ArgumentNotNullOrEmpty(userAccount.Password, nameof(userAccount.Password));

            //校验邮箱和手机是否被注册
            if (_userAccountRepository.CheckEmailOrPhoneExist(userAccount.Email, userAccount.Phone))
                return Result.Error("该联系方式已经被注册");

            userAccount.Id = Guid.NewGuid();
            //密码加盐
            userAccount.Password = GetSaltPassword(userAccount.Password);
            userAccount.Role = (int)RoleEnum.User;

            userAccount.UserId = TimeHelper.GetTimeStamp();

            return base.AddNoCareCode(userAccount);
        }

        public Result SetNextTimeResetPassword(Guid id)
        {
            return base.UpdateWithId(id, t =>
            {
                t.IsNeedToResetPassword = 1;
            });
        }

        public Result ResetPassword(Guid id, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result.Error("密码不能为空");

            var user = base.GetById(id);
            //判断该账号是否被设置成了修改密码,如果不是，则

            if (user.IsNeedToResetPassword != 1)
                return Result.Error("该账号无需重置密码，请联系管理员处理");

            return base.UpdateWithId(id, t =>
            {
                t.IsNeedToResetPassword = 0;
                t.Password = GetSaltPassword(password);
            });
        }

        public Result ChangePassword(Guid id, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result.Error("密码不能为空");

            return base.UpdateWithId(id, t =>
            {
                t.Password = GetSaltPassword(password);
            });
        }

        public Result<UserAccount> VerifyPassword(string phone, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Result<UserAccount>.Error("密码不能为空");

            var userAccount = _userAccountRepository.GetUserAccountByEmailOrPhone(email, phone);

            if (userAccount == null)
                return Result<UserAccount>.Error("账号不存在");

            if (!userAccount.Password.Equals(GetSaltPassword(password)))
                return Result<UserAccount>.Error("密码不正确");

            return Result<UserAccount>.Success(data: userAccount);
        }

        public Result PresetPassword(Guid id)
        {
            return base.UpdateWithId(id, t =>
            {
                t.IsNeedToResetPassword = 1;
            });
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

        public List<UserAccount> GetUserAccountList()
        {
            var userList = _userAccountRepository.GetListUnDeleted();

            if (userList != null)
            {
                var orgs = _organizationRepository.GetListUnDeleted()?.SafeToDictionary(k => k.Id, v => v.Name);

                foreach (var item in userList)
                {
                    item.OrganizationView = orgs.SafeGet(item.Organization);
                }
            }

            return userList;
        }

        public Result<string> GetToken(UserAccount userAccount)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new[]
            {
                //用户Id
                new Claim(AccountConst.KEY_UserId,userAccount.UserId.ToString()),
                //用户邮箱
                new Claim(AccountConst.KEY_UserEmail, userAccount.Email),
                //用户名
                new Claim(AccountConst.KEY_UserName, userAccount.Name),
                //用户系统身份
                new Claim(AccountConst.KEY_ChameleonRole, userAccount.Role.ToString()),
                //用户组织
                new Claim(AccountConst.KEY_Organization, userAccount.Organization.ToString())
            };
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccountConfig.Instance.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            var token = new JwtSecurityToken(
                issuer: AccountConfig.Instance.TokenIssuer,
                audience: AccountConfig.Instance.TokenAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(AccountConfig.Instance.TokenExpiredMinutesTimeSpan),
                signingCredentials: creds);

            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return Result<string>.Success("get token success", tokenStr);
        }
    }
}
