using Chameleon.Entity;
using SevenTiny.Bantina.Bankinate;
using SevenTiny.Bantina.Bankinate.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chameleon.Repository
{
    [DataBase("Chameleon")]
    public class ChameleonDbContext : MySqlDbContext<ChameleonDbContext>
    {
        public ChameleonDbContext() : base("")
        {
            ////开启一级缓存
            //OpenQueryCache = false;
            //OpenTableCache = false;
            ////用redis做缓存
            //CacheMediaType = CacheMediaType.Local;
            //CacheMediaServer = $"{RedisConfig.Get("101", "Server")}:{RedisConfig.Get("101", "Port")}";//redis服务器地址以及端口号
        }

        public DbSet<CloudApplication> CloudApplication { get; set; }
    }
}
