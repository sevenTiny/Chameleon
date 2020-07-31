using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.ValueObject
{
    public class ChameleonStatistics
    {
        public long MetaObjectCount { get; set; }
        public long MetaFieldCount { get; set; }
        public long InterfaceFieldsCount { get; set; }
        public long InterfaceConditionCount { get; set; }
        public long InterfaceVerificationCount { get; set; }
        public long InterfaceSortCount { get; set; }
        public long InterfaceCount { get; set; }
        public long TriggerScriptCount { get; set; }

        public long ProfileCount { get; set; }
        public long UserAccountAllCount { get; set; }
        public long UserAccountAdminisratorCount { get; set; }
        public long MenuCount { get; set; }
        public long FunctionCount { get; set; }
    }
}
