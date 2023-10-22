using Microsoft.Extensions.Caching.Memory;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03.CachForlders
{
    public class CachedNotesService
    {
        private NotePlannerDbContext db;
        private IMemoryCache cache;
        private int _rowsNumber;
        private const int option = 2;
        private readonly int resultCacheTime;
        public CachedNotesService(NotePlannerDbContext context, IMemoryCache memoryCache) 
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
            resultCacheTime = option * 2 + 240;
        }

        public IEnumerable<Note> GetNotes()
        {
            return db.Notes.Take(_rowsNumber).ToList();
        }

        public void AddNotes(string cacheKey)
        {
            IEnumerable<Note> notes = db.Notes.Take(_rowsNumber);

            cache.Set(cacheKey, notes, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(resultCacheTime)
            });
        }

        public IEnumerable<Note> GetNotes(string cacheKey)
        {
            IEnumerable<Note> notes = null;
            if (!cache.TryGetValue(cacheKey, out notes))
            {
                notes = db.Notes.Take(_rowsNumber).ToList();
                if (notes != null)
                {
                    cache.Set(cacheKey, notes, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(resultCacheTime)));
                }
            }
            return notes;
        }

    }
}
