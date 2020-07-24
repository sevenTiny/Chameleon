using Chameleon.Entity;
using Chameleon.Infrastructure;
using Chameleon.Repository;
using Chameleon.ValueObject;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SevenTiny.Bantina;
using SevenTiny.Cloud.ScriptEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Domain
{
    public interface ITriggerScriptService : ICommonServiceBase<TriggerScript>
    {
        /// <summary>
        /// 获取对象接口触发器默认脚本
        /// </summary>
        /// <param name="metaObjectInterfaceServiceTypeEnum"></param>
        /// <returns></returns>
        DefaultScriptBase GetDefaultMetaObjectTriggerScript(InterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum);
        /// <summary>
        /// 获取默认动态接口触发器脚本
        /// </summary>
        /// <returns></returns>
        DefaultScriptBase GetDeefaultDynamicScriptDataSourceTrigger();
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="triggerScript"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Result<TResult> ExecuteTriggerScript<TResult>(TriggerScript triggerScript, object[] parameters);
        /// <summary>
        /// 检查脚本
        /// </summary>
        /// <param name="triggerScript"></param>
        /// <returns></returns>
        Result CheckScript(TriggerScript triggerScript);
        /// <summary>
        /// 添加对象触发器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Result MetaObjectTriggerAdd(TriggerScript entity);
    }

    public class TriggerScriptService : CommonServiceBase<TriggerScript>, ITriggerScriptService
    {
        IDynamicScriptEngine _dynamicScriptEngine;
        ITriggerScriptRepository _triggerScriptRepository;
        public TriggerScriptService(IDynamicScriptEngine dynamicScriptEngine, ITriggerScriptRepository repository) : base(repository)
        {
            _dynamicScriptEngine = dynamicScriptEngine;
            _triggerScriptRepository = repository;
        }

        public Result MetaObjectTriggerAdd(TriggerScript entity)
        {
            //校验是否已经存在相同服务的触发器脚本
            if (_triggerScriptRepository.CheckMetaObjectInterfaceServiceTypeExistIfMetaObjectTrigger(entity.MetaObjectId, entity.GetMetaObjectInterfaceServiceType()))
                return Result.Error($"该对象下已经存在一个[{entity.GetMetaObjectInterfaceServiceType().GetDescription()}]类型的脚本");

            return base.AddCheckCode(entity);
        }

        public DefaultScriptBase GetDefaultMetaObjectTriggerScript(InterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum)
        {
            switch (metaObjectInterfaceServiceTypeEnum)
            {
                case InterfaceServiceTypeEnum.UnKnown:
                    return null;
                case InterfaceServiceTypeEnum.MetaObject_Add_Before:
                    return new MetaObjectInterface_Add_Before();
                case InterfaceServiceTypeEnum.MetaObject_Add_After:
                    return new MetaObjectInterface_Add_After();
                case InterfaceServiceTypeEnum.MetaObject_BatchAdd_Before:
                    return new MetaObjectInterface_BatchAdd_Before();
                case InterfaceServiceTypeEnum.MetaObject_BatchAdd_After:
                    return new MetaObjectInterface_BatchAdd_After();
                case InterfaceServiceTypeEnum.MetaObject_Update_Before:
                    return new MetaObjectInterface_Update_Before();
                case InterfaceServiceTypeEnum.MetaObject_Update_After:
                    return new MetaObjectInterface_Update_After();
                case InterfaceServiceTypeEnum.MetaObject_Delete_Before:
                    return new MetaObjectInterface_Delete_Before();
                case InterfaceServiceTypeEnum.MetaObject_Delete_After:
                    return new MetaObjectInterface_Delete_After();
                case InterfaceServiceTypeEnum.MetaObject_QueryCount_Before:
                    return new MetaObjectInterface_QueryCount_Before();
                case InterfaceServiceTypeEnum.MetaObject_QueryCount_After:
                    return new MetaObjectInterface_QueryCount_After();
                case InterfaceServiceTypeEnum.MetaObject_QuerySingle_Before:
                    return new MetaObjectInterface_QuerySingle_Before();
                case InterfaceServiceTypeEnum.MetaObject_QuerySingle_After:
                    return new MetaObjectInterface_QuerySingle_After();
                case InterfaceServiceTypeEnum.MetaObject_QueryList_Before:
                    return new MetaObjectInterface_QueryList_Before();
                case InterfaceServiceTypeEnum.MetaObject_QueryList_After:
                    return new MetaObjectInterface_QueryList_After();
                default:
                    break;
            }
            return null;
        }

        public DefaultScriptBase GetDeefaultDynamicScriptDataSourceTrigger()
        {
            return new DynamicScriptDataSourceScript();
        }

        public Result<TResult> ExecuteTriggerScript<TResult>(TriggerScript triggerScript, object[] parameters)
        {
            var dynamicScript = new DynamicScript
            {
                ClassFullName = triggerScript.ClassFullName,
                FunctionName = triggerScript.FunctionName,
                Script = triggerScript.Script,
                Language = (DynamicScriptLanguage)triggerScript.Language,
                Parameters = parameters,
                IsTrustedScript = false,
                MillisecondsTimeout = 5000//脚本最多允许执行5s，超过将会终止执行，避免影响其他服务
            };

            var result = _dynamicScriptEngine.Execute<TResult>(dynamicScript);

            return new Result<TResult>
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
                Data = result.Data
            };
        }

        public Result CheckScript(TriggerScript triggerScript)
        {
            var dynamicScript = new DynamicScript
            {
                ClassFullName = triggerScript.ClassFullName,
                FunctionName = triggerScript.FunctionName,
                Script = triggerScript.Script,
                Language = (DynamicScriptLanguage)triggerScript.Language,
            };

            var result = _dynamicScriptEngine.CheckScript(dynamicScript);

            return new Result
            {
                IsSuccess = result.IsSuccess,
                Message = result.Message,
            };
        }
    }
}
