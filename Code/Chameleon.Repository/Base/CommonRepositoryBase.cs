using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Repository
{
    public interface ICommonRepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : CommonBase
    {
        /// <summary>
        /// 只有工具才可以调用真实删除接口，业务通常调用LogicDelete接口进行逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result Delete(Guid id);
        Result BatchDelete(IEnumerable<Guid> ids);
        Result LogicDelete(Guid id);
        Result Recover(Guid id);
        TEntity GetById(Guid id);
        TEntity GetByCode(string code);
        List<TEntity> GetListAll();
        List<TEntity> GetListDeleted();
        List<TEntity> GetListUnDeleted();

        TEntity GetByCodeWithoutSameId(Guid id, string code);
        bool CheckCodeExist(string code);
        bool CheckCodeExistWithoutSameId(Guid id, string code);
    }

    public abstract class CommonRepositoryBase<TEntity> : RepositoryBase<TEntity>, ICommonRepositoryBase<TEntity> where TEntity : CommonBase
    {
        public CommonRepositoryBase(ChameleonMetaDataDbContext dbContext) : base(dbContext)
        {
        }

        public new Result Update(TEntity entity)
        {
            entity.ModifyTime = DateTime.Now;

            base.Update(entity);
            return Result.Success();
        }

        public Result Delete(Guid id)
        {
            _dbContext.Delete<TEntity>(t => t.Id.Equals(id));
            return Result.Success();
        }

        public Result LogicDelete(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.Deleted;
                _dbContext.Update(entity);
            }
            return Result.Success();
        }

        public Result Recover(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                entity.IsDeleted = (int)IsDeleted.UnDeleted;
                _dbContext.Update(entity);
            }
            return Result.Success();
        }

        public TEntity GetById(Guid id)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id.Equals(id)).FirstOrDefault();

        public TEntity GetByCode(string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code)).FirstOrDefault();

        public List<TEntity> GetListAll()
            => _dbContext.Queryable<TEntity>().ToList();

        public List<TEntity> GetListDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.Deleted).ToList();

        public List<TEntity> GetListUnDeleted()
            => _dbContext.Queryable<TEntity>().Where(t => t.IsDeleted == (int)IsDeleted.UnDeleted).ToList();

        /// <summary>
        /// 通过编码查询但是id不同的数据，通常用在修改编码操作，校验编码是否已经被其他数据使用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public TEntity GetByCodeWithoutSameId(Guid id, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Code.Equals(code)).FirstOrDefault();

        public bool CheckCodeExistWithoutSameId(Guid id, string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Id != id && t.Code.Equals(code)).Any();

        public bool CheckCodeExist(string code)
            => _dbContext.Queryable<TEntity>().Where(t => t.Code.Equals(code)).Any();

        public Result BatchDelete(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any())
                return Result.Success();

            foreach (var item in ids)
            {
                this.Delete(item);
            }

            return Result.Success();
        }
    }
}
