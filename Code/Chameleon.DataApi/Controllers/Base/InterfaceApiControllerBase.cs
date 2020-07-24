using Chameleon.Application;
using Chameleon.Bootstrapper;
using Chameleon.DataApi.Models;
using Chameleon.Domain;
using Chameleon.Entity;
using Chameleon.Repository;
using Microsoft.AspNetCore.Authorization;
using SevenTiny.Bantina.Extensions;
using System;
using System.Collections.Generic;

namespace Chameleon.DataApi.Controllers
{
    [Authorize]
    public class InterfaceApiControllerBase : ApiControllerCommonBase
    {
        protected IInterfaceSettingRepository _interfaceSettingRepository;
        protected ITriggerScriptRepository _triggerScriptRepository;
        public InterfaceApiControllerBase(ITriggerScriptRepository triggerScriptRepository, IInterfaceSettingRepository interfaceSettingRepository)
        {
            _triggerScriptRepository = triggerScriptRepository;
            _interfaceSettingRepository = interfaceSettingRepository;
        }

        protected QueryContext _queryContext;

        /// <summary>
        /// 初始化查询上下文
        /// </summary>
        /// <param name="queryArgs"></param>
        protected void InitQueryContext(QueryArgs queryArgs)
        {
            if (queryArgs == null)
                throw new ArgumentNullException(nameof(queryArgs), "query args cannot be null");

            _queryContext = new QueryContext();

            //查询接口
            _queryContext.InterfaceSetting = _interfaceSettingRepository.GetInterfaceSettingByCodeWithVerify(queryArgs._interface);

            //argumentsDic generate
            foreach (var item in Request.Query)
                _queryContext.ConditionArgumentsUpperKeyDic.AddOrUpdate(item.Key.ToUpperInvariant(), item.Value);

            //初始化触发器
            switch (_queryContext.InterfaceSetting.GetInterfaceType())
            {
                case InterfaceTypeEnum.Add:
                case InterfaceTypeEnum.BatchAdd:
                case InterfaceTypeEnum.Update:
                case InterfaceTypeEnum.Delete:
                case InterfaceTypeEnum.QueryCount:
                case InterfaceTypeEnum.QuerySingle:
                case InterfaceTypeEnum.QueryList:
                    //查询对象接口
                    _queryContext.TriggerScripts = _triggerScriptRepository.GetMetaObjectTriggerListByMetaObjectId(_queryContext.InterfaceSetting.MetaObjectId);
                    break;
                case InterfaceTypeEnum.FileUpload:
                case InterfaceTypeEnum.FileDownload:
                case InterfaceTypeEnum.DynamicScriptDataSource:
                case InterfaceTypeEnum.JsonDataSource:
                case InterfaceTypeEnum.UnKnown:
                default:
                    break;
            }

            //上下文赋值一些查询可能用到的参数
            _queryContext.TriggerContext.Add("Interface", queryArgs._interface);
            _queryContext.TriggerContext.Add("PageIndex", queryArgs._pageIndex.ToString());
            _queryContext.TriggerContext.Add("PageSize", _queryContext.InterfaceSetting.PageSize.ToString());
            _queryContext.TriggerContext.Add("MetaObjectCode", _queryContext.InterfaceSetting.MetaObjectCode);
            _queryContext.TriggerContext.Add("MetaObjectId", _queryContext.InterfaceSetting.MetaObjectId.ToString());
            _queryContext.TriggerContext.Add("CloudApplicationCode", _queryContext.InterfaceSetting.CloudApplicationCode);
            _queryContext.TriggerContext.Add("CloudApplicationtId", _queryContext.InterfaceSetting.CloudApplicationId.ToString());
            _queryContext.TriggerContext.Add("CurrentOrganization", CurrentOrganization.ToString());
            _queryContext.TriggerContext.Add("CurrentUserId", CurrentUserId.ToString());
            _queryContext.TriggerContext.Add("CurrentUserEmail", CurrentUserEmail);
            _queryContext.TriggerContext.Add("CurrentUserName", CurrentUserName);
            _queryContext.TriggerContext.Add("CurrentUserRole", CurrentUserRole.ToString());
        }
    }
}
