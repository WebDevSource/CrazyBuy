using System.Collections.Generic;

namespace CrazyBuy.Common
{
    public class CacheResult
    {
        private static readonly Dictionary<string, object> cache = new Dictionary<string, object>();

        public static bool isKeyExist(string key)
        {
            return cache.ContainsKey(key);
        }

        public static void setData(string key, object data)
        {
            if (cache.ContainsKey(key))
            {
                remove(key);
            }
            MDebugLog.debug("[CacheResult-setData] key:" + key);
            cache.Add(key, data);
        }

        public static object getData(string key)
        {
            MDebugLog.debug("[CacheResult-getData] key:" + key);
            return cache.GetValueOrDefault(key);
        }

        public static void clearCache()
        {
            cache.Clear();
        }

        public static void remove(string key)
        {
            cache.Remove(key);
        }
    }
}
