using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03.CachForlders
{
    public class CachedUsersService
    {
        private NotePlannerDbContext db;
        private IMemoryCache cache;
        private int _rowsNumber;
        private const int option = 2;
        private readonly int resultCacheTime;

        public CachedUsersService(NotePlannerDbContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
            resultCacheTime = option * 2 + 240;
        }

        public IEnumerable<User> GetUsers()
        {
            return db.Users.Take(_rowsNumber).ToList();
        }

        public void AddUsers(string cacheKey)
        {
            IEnumerable<User> users = db.Users.Take(_rowsNumber);

            cache.Set(cacheKey, users, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(resultCacheTime)
            });
        }

        public IEnumerable<User> GetUsers(string cacheKey)
        {
            IEnumerable<User> users = null;
            if (!cache.TryGetValue(cacheKey, out users))
            {
                users = db.Users.Take(_rowsNumber).ToList();
                if (users != null)
                {
                    cache.Set(cacheKey, users, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(resultCacheTime)));
                }
            }
            return users;
        }

    }


}
