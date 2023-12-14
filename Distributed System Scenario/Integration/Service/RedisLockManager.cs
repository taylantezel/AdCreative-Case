using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service
{
    public class RedisLockManager
    {
        private readonly IDatabase _database;

        public RedisLockManager(IDatabase database)
        {
            _database = database;
        }

        public bool AcquireLock(string lockKey, string lockIdentifier, TimeSpan expiration)
        {
            var acquired = _database.StringSet(lockKey, lockIdentifier, expiration, When.NotExists);

            return acquired;
        }

        public void ReleaseLock(string lockKey, string lockIdentifier)
        {
            var currentIdentifier = _database.StringGet(lockKey);

            if (currentIdentifier == lockIdentifier)
            {
                _database.KeyDelete(lockKey);
            }
        }
    }
}
