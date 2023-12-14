using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service
{
    public class ItemLockManager
    {
        private readonly Dictionary<string, object> lockObjects = new();

        public object GetLockObject(string itemContent)
        {
            lock (lockObjects)
            {
                if (!lockObjects.ContainsKey(itemContent))
                    lockObjects[itemContent] = new object();

                return lockObjects[itemContent];
            }
        }
    }
}
