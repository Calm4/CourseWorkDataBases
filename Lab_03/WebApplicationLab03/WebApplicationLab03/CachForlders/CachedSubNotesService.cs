using Microsoft.Extensions.Caching.Memory;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03.CachForlders
{
    public class CachedSubNotesService
    {
        private NotePlannerDbContext db;
        private IMemoryCache cache;
        private int _rowsNumber;
        private const int option = 2;
        private readonly int resultCacheTime;

        public CachedSubNotesService(NotePlannerDbContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
            resultCacheTime = option * 2 + 240;
        }

        public IEnumerable<SubNote> GetSubNotes()
        {
            return db.SubNotes.Take(_rowsNumber).ToList();
        }

        public void AddSubNotes(string cacheKey)
        {
            IEnumerable<SubNote> subNotes = db.SubNotes.Take(_rowsNumber);

            cache.Set(cacheKey, subNotes, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(resultCacheTime)
            });
        }

        public IEnumerable<SubNote> GetSubNotes(string cacheKey)
        {
            IEnumerable<SubNote> subNotes = null;
            if (!cache.TryGetValue(cacheKey, out subNotes))
            {
                subNotes = db.SubNotes.Take(_rowsNumber).ToList();
                if (subNotes != null)
                {
                    cache.Set(cacheKey, subNotes, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(resultCacheTime)));
                }
            }
            return subNotes;
        }

    }
}
