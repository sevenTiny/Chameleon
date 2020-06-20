using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using SevenTiny.Bantina;
using SevenTiny.Bantina.Extensions;
using SevenTiny.Bantina.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chameleon.Domain
{
    public interface IInterfaceVerificationService : IMetaObjectCommonServiceBase<InterfaceVerification>
    {
        /// <summary>
        /// 更新选择字段
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result UpdateSeletedMetaField(InterfaceVerification entity);
        /// <summary>
        /// 保存接口校验配置
        /// </summary>
        /// <param name="metaObjectId"></param>
        /// <param name="interfaceVerificationId"></param>
        /// <param name="metaFieldIds"></param>
        /// <returns></returns>
        Result SaveSetting(Guid metaObjectId, Guid interfaceVerificationId, List<Guid> metaFieldIds);
        /// <summary>
        /// 正则表达式校验输入值是否正确
        /// </summary>
        /// <param name="interfaceVerification"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        bool IsMatch(InterfaceVerification interfaceVerification, string input);
    }

    public class InterfaceVerificationService : MetaObjectCommonServiceBase<InterfaceVerification>, IInterfaceVerificationService
    {
        IInterfaceVerificationRepository _InterfaceVerificationRepository;
        IMetaFieldRepository _metaFieldRepository;
        public InterfaceVerificationService(IInterfaceVerificationRepository repository, IMetaFieldRepository metaFieldRepository) : base(repository)
        {
            _metaFieldRepository = metaFieldRepository;
            _InterfaceVerificationRepository = repository;
        }

        public Result UpdateSeletedMetaField(InterfaceVerification entity)
        {
            return base.UpdateWithId(entity.Id, item =>
            {
                item.VerificationTips = entity.VerificationTips;
                item.RegularExpression = entity.RegularExpression;
            });
        }

        public Result SaveSetting(Guid metaObjectId, Guid interfaceVerificationId, List<Guid> metaFieldIds)
        {
            if (metaFieldIds == null || !metaFieldIds.Any()) return Result.Success();

            //获取现在已经有的
            var currentList = _InterfaceVerificationRepository.GetInterfaceVerificationByParentId(interfaceVerificationId) ?? new List<InterfaceVerification>(0);

            //删掉已经不包含的字段
            _InterfaceVerificationRepository.BatchDelete(currentList.Where(t => !metaFieldIds.Contains(t.MetaFieldId)).Select(t => t.Id));

            //缓存当前已经存在的字段id集合
            var currentMetaFieldIds = currentList.Select(t => t.MetaFieldId).ToArray();

            //找出本次需要添加的字段
            var toAddIds = metaFieldIds.Where(t => !currentMetaFieldIds.Contains(t)).ToArray();

            if (!toAddIds.Any()) return Result.Success();

            //查询字段
            var fieldDic = _metaFieldRepository.GetMetaFieldIdDicByMetaObjectId(metaObjectId);

            var toAdds = toAddIds.Select(t => new InterfaceVerification
            {
                Code = fieldDic.SafeGet(t)?.Code,
                Name = fieldDic.SafeGet(t)?.Name,
                ParentId = interfaceVerificationId,
                MetaFieldId = t,
                MetaFieldShortCode = fieldDic.SafeGet(t)?.ShortCode,
                MetaObjectId = metaObjectId,
                CloudApplicationtId = fieldDic.SafeGet(t)?.CloudApplicationtId ?? Guid.Empty
            });

            base.BatchAdd(toAdds);

            return Result.Success();
        }

        public bool IsMatch(InterfaceVerification interfaceVerification, string input)
        {
            //用正则表达式校验
            if (!string.IsNullOrEmpty(interfaceVerification.RegularExpression))
            {
                return Regex.IsMatch(input, interfaceVerification.RegularExpression);
            }

            return true;
        }
    }
}
