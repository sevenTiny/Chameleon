using Chameleon.Domain;
using Chameleon.Entity;
using SevenTiny.Bantina;
using System;
using System.Collections.Generic;
using System.Text;


namespace Chameleon.Application
{
    public interface IDataAccessApp
    {

    }

    public class DataAccessApp : IDataAccessApp
    {
        IInterfaceConditionService _interfaceConditionService;
        IInterfaceFieldsService _interfaceFieldsService;
        IMetaFieldService _metaFieldService;

        public DataAccessApp(IInterfaceConditionService interfaceConditionService, IInterfaceFieldsService interfaceFieldsService, IMetaFieldService metaFieldService)
        {
            _metaFieldService = metaFieldService;
            _interfaceFieldsService = interfaceFieldsService;
            _interfaceConditionService = interfaceConditionService;
        }


    }
}
