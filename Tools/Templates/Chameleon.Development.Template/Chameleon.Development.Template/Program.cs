using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using SevenTiny.Bantina;
using Chameleon.Common;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Chameleon.Development.Template
{
    class Program
    {
        static void Main(string[] args)
        {
            var re = new DynamicScriptDataSource().Get(null);

            var triggerContext = new Dictionary<string, string>();
            var result = new List<Dictionary<string, CloudData>>();

            result = new MetaObjectInterfaceTrigger().QueryList_After(triggerContext, result);

            Console.WriteLine("Hello World!");
        }
    }
}
