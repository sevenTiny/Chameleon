using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IUserAccountRepository : ICommonRepositoryBase<UserAccount>
    {
        List<UserAccount> GetUserAccountList();
        /// <summary>
        /// 获取无开发人员的所有人员列表
        /// </summary>
        /// <returns></returns>
        List<UserAccount> GetUserAccountListNoDeveolper();
        /// <summary>
        /// 通过邮箱或者手机号获取账号
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        UserAccount GetUserAccountByEmailOrPhone(string email, string phone);
        /// <summary>
        /// 校验邮箱或者手机是否已经被注册
        /// </summary>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        bool CheckEmailOrPhoneExist(string email, string phone);
    }

    public class UserAccountRepository : CommonRepositoryBase<UserAccount>, IUserAccountRepository
    {
        public UserAccountRepository(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }

        public List<UserAccount> GetUserAccountList()
        {
            return _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0).ToList();
        }

        public List<UserAccount> GetUserAccountListNoDeveolper()
        {
            var deveolperRole = (int)RoleEnum.Developer;
            return _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0 && t.Role != deveolperRole).ToList();
        }

        public UserAccount GetUserAccountByEmailOrPhone(string email, string phone)
        {
            if (!string.IsNullOrEmpty(email))
                return _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0 && t.Email.Equals(email)).FirstOrDefault();
            else if (!string.IsNullOrEmpty(phone))
                return _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0 && t.Phone.Equals(phone)).FirstOrDefault();

            return null;
        }

        public bool CheckEmailOrPhoneExist(string email, string phone)
        {
            var result = false;

            if (!string.IsNullOrEmpty(email))
                result = _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0 && t.Email.Equals(email)).Any();
            if (result == false && !string.IsNullOrEmpty(phone))
                return _dbContext.Queryable<UserAccount>().Where(t => t.IsDeleted == 0 && t.Phone.Equals(phone)).Any();

            return result;
        }
    }
}
