using Microsoft.Extensions.Caching.Memory;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03.CachForlders
{
    public class CachedNoteTagsService
    {
        private NotePlannerDbContext db;
        private IMemoryCache cache;
        private int _rowsNumber;
        private const int option = 2;
        private readonly int resultCacheTime;

        public CachedNoteTagsService(NotePlannerDbContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
            resultCacheTime = option * 2 + 240;
        }

        public IEnumerable<NoteTag> GetNoteTags()
        {
            return db.NoteTags.Take(_rowsNumber).ToList();
        }

        public void AddNoteTags(string cacheKey)
        {
            IEnumerable<NoteTag> noteTags = db.NoteTags.Take(_rowsNumber);

            cache.Set(cacheKey, noteTags, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(resultCacheTime)
            });
        }

        public IEnumerable<NoteTag> GetNoteTags(string cacheKey)
        {
            IEnumerable<NoteTag> noteTags = null;
            if (!cache.TryGetValue(cacheKey, out noteTags))
            {
                noteTags = db.NoteTags.Take(_rowsNumber).ToList();
                if (noteTags != null)
                {
                    cache.Set(cacheKey, noteTags, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(resultCacheTime)));
                }
            }
            return noteTags;
        }

    }
}
