using Chameleon.Entity;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chameleon.Domain
{
    public interface IInterfaceFieldsService : IMetaObjectCommonServiceBase<InterfaceFields>
    {
        /// <summary>
        /// 添加顶级接口字段配置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result AddTopInterfaceFields(InterfaceFields entity);
        /// <summary>
        /// 更新选择字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result UpdateSeletedMetaField(InterfaceFields entity);
        /// <summary>
        /// 保存接口字段配置
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="interfaceFieldId"></param>
        /// <param name="metaFieldIds"></param>
        /// <returns></returns>
        Result SaveSetting(Guid metaObjectId, Guid interfaceFieldId, List<Guid> metaFieldIds);
    }

    public class InterfaceFieldsService : MetaObjectCommonServiceBase<InterfaceFields>, IInterfaceFieldsService
    {
        IInterfaceFieldsRepository _interfaceFieldsRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceFieldsService(IInterfaceFieldsRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _interfaceFieldsRepository = repository;
        }

        public Result AddTopInterfaceFields(InterfaceFields entity)
        {
            entity.ParentId = Guid.Empty;
            entity.MetaFieldShortCode = "-";
            entity.MetaFieldId = Guid.Empty;

            return base.Add(entity);
        }

        public Result UpdateSeletedMetaField(InterfaceFields entity)
        {
            return base.UpdateWithId(entity.Id, _ =>
            {
                _.MetaFieldCustomViewName = entity.MetaFieldCustomViewName;
            });
        }

        public Result SaveSetting(Guid metaObjectId, Guid interfaceFieldId, List<Guid> metaFieldIds)
        {
            if (metaFieldIds == null || !metaFieldIds.Any()) return Result.Success();

            //获取现在已经有的
            var currentList = _interfaceFieldsRepository.GetInterfaceFieldsByParentId(interfaceFieldId) ?? new List<InterfaceFields>(0);

            //删掉已经不包含的字段
            _interfaceFieldsRepository.BatchDelete(currentList.Where(t => !metaFieldIds.Contains(t.MetaFieldId)).Select(t => t.Id));

            //缓存当前已经存在的字段id集合
            var currentMetaFieldIds = currentList.Select(t => t.MetaFieldId).ToArray();

            //找出本次需要添加的字段
            var toAddIds = metaFieldIds.Where(t => !currentMetaFieldIds.Contains(t)).ToArray();

            if (!toAddIds.Any()) return Result.Success();

            //查询字段
            var fieldDic = _metaFieldRepository.GetMetaFieldIdDicByMetaObjectId(metaObjectId);

            var toAdds = toAddIds.Select(t => new InterfaceFields
            {
                Code = fieldDic.SafeGet(t)?.Code,
                Name = fieldDic.SafeGet(t)?.Name,
                ParentId = interfaceFieldId,
                MetaFieldId = t,
                MetaFieldShortCode = fieldDic.SafeGet(t)?.ShortCode,
                MetaFieldCustomViewName = fieldDic.SafeGet(t)?.Name,
                MetaObjectId = metaObjectId,
                CloudApplicationtId = fieldDic.SafeGet(t)?.CloudApplicationtId ?? Guid.Empty
            });

            base.BatchAdd(toAdds);

            return Result.Success();
        }
    }
}
