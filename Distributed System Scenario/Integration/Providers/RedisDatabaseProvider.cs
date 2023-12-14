using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Providers
{
    public class RedisDatabaseProvider
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisDatabaseProvider()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }

        public void Dispose()
        {
            _connectionMultiplexer?.Dispose();
        }
    }
}
