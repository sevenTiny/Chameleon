using Chameleon.Domain;
using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Application
{
    public interface IInterfaceSettingApp
    {
        /// <summary>
        /// 添加接口条件节点
        /// </summary>
        /// <param name="brotherNodeId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result InterfaceConditionAddNode(Guid brotherNodeId, InterfaceCondition entity);
    }

    public class InterfaceSettingApp : IInterfaceSettingApp
    {
        IInterfaceConditionService _interfaceConditionService;
        IInterfaceFieldsService _interfaceFieldsService;
        IMetaFieldService _metaFieldService;
        public InterfaceSettingApp(IInterfaceConditionService interfaceConditionService, IInterfaceFieldsService interfaceFieldsService, IMetaFieldService metaFieldService)
        {
            _metaFieldService = metaFieldService;
            _interfaceFieldsService = interfaceFieldsService;
            _interfaceConditionService = interfaceConditionService;
        }

        public Result InterfaceConditionAddNode(Guid brotherNodeId, InterfaceCondition entity)
        {
            return Result.Success("添加成功")
                .Continue(_ =>
                {
                    //如果是固定值，则校验值是否和类型一致
                    if (entity.ConditionValueType == (int)ConditionValueTypeEnum.FixedValue)
                        return _.ContinueAssert(re => _metaFieldService.CheckAndGetFieldValueByFieldType(entity.MetaFieldId, entity.ConditionValue).IsSuccess, "设置的值和字段值格式不匹配");

                    return _;
                })
                .Continue(_ => _interfaceConditionService.AddNode(brotherNodeId, entity));
        }
    }
}
