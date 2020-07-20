using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        /// <summary>
        /// 新增并校验编码
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddCheckCode(TEntity entity);
        Result AddNoCareCode(TEntity entity);
        Result<IEnumerable<TEntity>> BatchAdd(IEnumerable<TEntity> entities);
        Result Update(TEntity entity);
        Result Delete(TEntity entity);

        void TransactionBegin();
        void TransactionCommit();
        void TransactionRollback();

        Result CheckCodeExist(string code);
        Result CheckCodeExistWithoutSameId(Guid id, string code);
        /// <summary>
        /// 根据id更新（更新字段用action手动控制，内部仅更新了更新时间）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateFieldAction"></param>
        /// <returns></returns>
        Result UpdateWithId(Guid id, Action<TEntity> updateFieldAction = null);
        /// <summary>
        /// 通用编辑
        /// 注：编码不能修改，其他通用字段内部修改，个性字段赋值通过action手动修改
        /// </summary>
        /// <param name="source"></param>
        /// <param name="updateFieldAction"></param>
        /// <returns></returns>
        Result UpdateWithOutCode(TEntity source, Action<TEntity> updateFieldAction = null);

        /// <summary>
        /// 业务通常调用LogicDelete接口进行逻辑删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Result Delete(Guid id);
        Result LogicDelete(Guid id);
        Result Recover(Guid id);
        TEntity GetById(Guid id);
        TEntity GetByCode(string code);
        List<TEntity> GetListAll();
        List<TEntity> GetListDeleted();
        List<TEntity> GetListUnDeleted();

        TEntity GetByCodeWithoutSameId(Guid id, string code);
    }

    public abstract class CommonServiceBase<TEntity> : ICommonServiceBase<TEntity> where TEntity : CommonBase
    {
        public CommonServiceBase(ICommonRepositoryBase<TEntity> commonRepositoryBase)
        {
            _commonRepositoryBase = commonRepositoryBase;
        }

        protected ICommonRepositoryBase<TEntity> _commonRepositoryBase;

        /// <summary>
        /// 检查是否编码已经存在
        /// 常用于新增操作
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result CheckCodeExist(string code)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(code, nameof(code))
                .ContinueAssert(_ => !_commonRepositoryBase.CheckCodeExist(code), $"编码[{code}]已存在");
        }

        /// <summary>
        /// 检查是否存在其他相同编码的记录
        /// 常用于编辑操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result CheckCodeExistWithoutSameId(Guid id, string code)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(code, nameof(code))
                .ContinueAssert(_ => !_commonRepositoryBase.CheckCodeExistWithoutSameId(id, code), $"编码[{code}]已存在");
        }

        public Result AddCheckCode(TEntity entity)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                .ContinueEnsureArgumentNotNullOrEmpty(entity.Code, nameof(entity.Code))
                //校验编码是否已经存在
                .Continue(_ => CheckCodeExist(entity.Code))
                //校验Id并赋值
                .Continue(_ =>
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();

                    return _;
                })
                .Continue(_ => _commonRepositoryBase.Add(entity));
        }

        public Result AddNoCareCode(TEntity entity)
        {
            return Result.Success()
                .ContinueEnsureArgumentNotNullOrEmpty(entity, nameof(entity))
                //校验Id并赋值
                .Continue(_ =>
                {
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();

                    entity.Code = "-";

                    return _;
                })
                .Continue(_ => _commonRepositoryBase.Add(entity));
        }

        public Result UpdateWithId(Guid id, Action<TEntity> updateFieldAction = null)
        {
            TEntity target = _commonRepositoryBase.GetById(id);

            if (target == null)
                return Result.Error($"没有查到Id[{id}]对应的数据.");

            //个性化字段赋值
            if (updateFieldAction == null)
                return Result.Success("操作成功");

            updateFieldAction.Invoke(target);

            target.ModifyTime = DateTime.Now;

            _commonRepositoryBase.Update(target);

            return Result.Success("操作成功");
        }

        public Result UpdateWithOutCode(TEntity source, Action<TEntity> updateFieldAction = null)
        {
            TEntity target = _commonRepositoryBase.GetById(source.Id);

            if (target == null)
                return Result.Error($"没有查到Id[{source.Id}]对应的数据.");

            //个性化字段赋值
            updateFieldAction?.Invoke(target);

            //编码不允许修改!
            target.Name = source.Name;
            target.Group = source.Group;
            target.SortNumber = source.SortNumber;
            target.Description = source.Description;
            target.ModifyBy = source.ModifyBy;
            target.ModifyTime = DateTime.Now;

            return _commonRepositoryBase.Update(target);
        }

        public Result Delete(Guid id)
        {
            return _commonRepositoryBase.Delete(id);
        }

        public Result LogicDelete(Guid id)
        {
            return _commonRepositoryBase.LogicDelete(id);
        }

        public Result Recover(Guid id)
        {
            return _commonRepositoryBase.Recover(id);
        }

        public TEntity GetById(Guid id)
        {
            return _commonRepositoryBase.GetById(id);
        }

        public TEntity GetByCode(string code)
        {
            return _commonRepositoryBase.GetByCode(code);
        }

        public List<TEntity> GetListAll()
        {
            return _commonRepositoryBase.GetListAll();
        }

        public List<TEntity> GetListDeleted()
        {
            return _commonRepositoryBase.GetListDeleted();
        }

        public List<TEntity> GetListUnDeleted()
        {
            return _commonRepositoryBase.GetListUnDeleted();
        }

        public TEntity GetByCodeWithoutSameId(Guid id, string code)
        {
            return _commonRepositoryBase.GetByCodeWithoutSameId(id, code);
        }

        public Result<IEnumerable<TEntity>> BatchAdd(IEnumerable<TEntity> entities)
        {
            var toAdd = entities.ToList();

            foreach (var item in toAdd)
            {
                if (item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();
            }

            return _commonRepositoryBase.BatchAdd(toAdd);
        }

        public Result Update(TEntity entity)
        {
            return _commonRepositoryBase.Update(entity);
        }

        public Result Delete(TEntity entity)
        {
            return _commonRepositoryBase.Delete(entity);
        }

        public void TransactionBegin()
        {
            _commonRepositoryBase.TransactionBegin();
        }

        public void TransactionCommit()
        {
            _commonRepositoryBase.TransactionCommit();
        }

        public void TransactionRollback()
        {
            _commonRepositoryBase.TransactionRollback();
        }
    }
}
