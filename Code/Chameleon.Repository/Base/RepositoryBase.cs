using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Result Add(TEntity entity);
        Result<IEnumerable<TEntity>> BatchAdd(IEnumerable<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);

        void TransactionBegin();
        void TransactionCommit();
        void TransactionRollback();
    }

    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        public RepositoryBase(ChameleonMetaDataDbContext chameleonDbContext)
        {
            _dbContext = chameleonDbContext;
        }

        protected ChameleonMetaDataDbContext _dbContext;

        public Result Add(TEntity entity)
        {
            _dbContext.Add(entity);
            return Result.Success();
        }

        public Result<IEnumerable<TEntity>> BatchAdd(IEnumerable<TEntity> entities)
        {
            _dbContext.Add<TEntity>(entities);
            return Result<IEnumerable<TEntity>>.Success(data: entities);
        }

        public Result Update(TEntity entity)
        {
            _dbContext.Update(entity);
            return Result.Success();
        }

        public Result Delete(TEntity entity)
        {
            _dbContext.Delete<TEntity>(entity);
            return Result.Success();
        }

        public void TransactionBegin()
        {
            _dbContext.TransactionBegin();
        }

        public void TransactionCommit()
        {
            _dbContext.TransactionCommit();
        }

        public void TransactionRollback()
        {
            _dbContext.TransactionRollback();
        }
    }
}
