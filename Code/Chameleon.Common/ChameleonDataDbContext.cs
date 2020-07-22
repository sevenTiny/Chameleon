using Chameleon.Common.Configs;
using SevenTiny.Bantina.Bankinate;

namespace Chameleon.Common
{
    public class ChameleonDataDbContext : MongoDbContext<ChameleonDataDbContext>
    {
        public ChameleonDataDbContext() : base(ConnectionStringsConfig.Instance.mongodb39911)
        {
        }
    }
}
