using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;

namespace Chameleon.Domain
{
    public interface IInterfaceSortService : IMetaObjectCommonServiceBase<InterfaceSort>
    {
        /// <summary>
        /// 添加排序项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddSortItem(InterfaceSort entity);
        /// <summary>
        /// 更新排序项
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result UpdateSortItem(InterfaceSort entity);
    }

    public class InterfaceSortService : MetaObjectCommonServiceBase<InterfaceSort>, IInterfaceSortService
    {
        IInterfaceSortRepository _repository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceSortService(IMetaFieldRepository metaFieldRepository, IInterfaceSortRepository repository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _repository = repository;
        }

        public Result AddSortItem(InterfaceSort entity)
        {
            //查询字段
            var fieldDic = _metaFieldRepository.GetMetaFieldIdDicByMetaObjectId(entity.MetaObjectId);

            if (!fieldDic.ContainsKey(entity.MetaFieldId))
                return Result.Error("当前对象中不存在该字段");

            var metaField = fieldDic[entity.MetaFieldId];

            entity.Name = metaField.Name;
            //短编码重新赋值
            entity.MetaFieldShortCode = metaField.ShortCode;

            //校验是否已经存在了这个字段
            if (_repository.CheckMetaFieldShortCodeHasExistInCurrentSort(entity.ParentId, entity.MetaFieldShortCode))
                return Result.Error("已经存在一个该字段的规则了");

            return base.Add(entity);
        }

        public Result UpdateSortItem(InterfaceSort entity)
        {
            return base.UpdateWithId(entity.Id, item =>
            {
                item.SortType = entity.SortType;
            });
        }
    }
}
