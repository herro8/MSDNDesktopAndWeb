using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Ahead.Data
{
    public static class ServerCacheManager
    {
        public static MemoryCache Cache = MemoryCache.Default;
    }
}