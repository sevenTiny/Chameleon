using Chameleon.Entity;
using Chameleon.Repository;
using Chameleon.ValueObject;
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
        DefaultScriptBase GetDefaultMetaObjectInterfaceTriggerScript(MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum);
        /// <summary>
        /// 获取默认动态接口触发器脚本
        /// </summary>
        /// <returns></returns>
        DefaultScriptBase GetDeefaultDynamicScriptInterfaceTrigger();
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
    }

    public class TriggerScriptService : CommonServiceBase<TriggerScript>, ITriggerScriptService
    {
        IDynamicScriptEngine _dynamicScriptEngine;
        public TriggerScriptService(IDynamicScriptEngine dynamicScriptEngine, ITriggerScriptRepository repository) : base(repository)
        {
            _dynamicScriptEngine = dynamicScriptEngine;
        }

        public DefaultScriptBase GetDefaultMetaObjectInterfaceTriggerScript(MetaObjectInterfaceServiceTypeEnum metaObjectInterfaceServiceTypeEnum)
        {
            switch (metaObjectInterfaceServiceTypeEnum)
            {
                case MetaObjectInterfaceServiceTypeEnum.UnKnown:
                    return null;
                case MetaObjectInterfaceServiceTypeEnum.Add_Before:
                    return new MetaObjectInterface_Add_Before();
                case MetaObjectInterfaceServiceTypeEnum.Add_After:
                    return new MetaObjectInterface_Add_After();
                case MetaObjectInterfaceServiceTypeEnum.BatchAdd_Before:
                    return new MetaObjectInterface_BatchAdd_Before();
                case MetaObjectInterfaceServiceTypeEnum.BatchAdd_After:
                    return new MetaObjectInterface_BatchAdd_After();
                case MetaObjectInterfaceServiceTypeEnum.Update_Before:
                    return new MetaObjectInterface_Update_Before();
                case MetaObjectInterfaceServiceTypeEnum.Update_After:
                    return new MetaObjectInterface_Update_After();
                case MetaObjectInterfaceServiceTypeEnum.Delete_Before:
                    return new MetaObjectInterface_Delete_Before();
                case MetaObjectInterfaceServiceTypeEnum.Delete_After:
                    return new MetaObjectInterface_Delete_After();
                case MetaObjectInterfaceServiceTypeEnum.QueryCount_Before:
                    return new MetaObjectInterface_QueryCount_Before();
                case MetaObjectInterfaceServiceTypeEnum.QueryCount_After:
                    return new MetaObjectInterface_QueryCount_After();
                case MetaObjectInterfaceServiceTypeEnum.QuerySingle_Before:
                    return new MetaObjectInterface_QuerySingle_Before();
                case MetaObjectInterfaceServiceTypeEnum.QuerySingle_After:
                    return new MetaObjectInterface_QuerySingle_After();
                case MetaObjectInterfaceServiceTypeEnum.QueryList_Before:
                    return new MetaObjectInterface_QueryList_Before();
                case MetaObjectInterfaceServiceTypeEnum.QueryList_After:
                    return new MetaObjectInterface_QueryList_After();
                default:
                    break;
            }
            return null;
        }

        public DefaultScriptBase GetDeefaultDynamicScriptInterfaceTrigger()
        {
            return new DynamicScriptInterfaceScript();
        }

        public Result<TResult> ExecuteTriggerScript<TResult>(TriggerScript triggerScript, object[] parameters)
        {
            var dynamicScript = new DynamicScript
            {
                ClassFullName = triggerScript.ClassFullName,
                FunctionName = triggerScript.FunctionName,
                Script = triggerScript.Script,
                Language = (DynamicScriptLanguage)triggerScript.Language,
                Parameters = parameters
            };

            var result = _dynamicScriptEngine.Execute<TResult>(dynamicScript);

            return new Result<TResult>
            {
                //1. 这里要调整bantina为可赋值
                //2. 要调整
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
                //1. 这里要调整bantina为可赋值
                //2. 要调整
            };
        }
    }
}
