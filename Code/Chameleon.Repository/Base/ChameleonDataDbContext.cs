using Chameleon.Infrastructure.Configs;
using SevenTiny.Bantina.Bankinate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    public class ChameleonDataDbContext : MongoDbContext<ChameleonDataDbContext>
    {
        public ChameleonDataDbContext() : base(ConnectionStringsConfig.Instance.mongodb39911)
        {
        }
    }
}
