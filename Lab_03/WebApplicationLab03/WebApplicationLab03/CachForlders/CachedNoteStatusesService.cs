using Microsoft.Extensions.Caching.Memory;
using WebApplicationLab03.Tables;

namespace WebApplicationLab03.CachForlders
{
    public class CachedNoteStatusesService
    {
        private NotePlannerDbContext db;
        private IMemoryCache cache;
        private int _rowsNumber;
        private const int option = 2;
        private readonly int resultCacheTime;

        public CachedNoteStatusesService(NotePlannerDbContext context, IMemoryCache memoryCache)
        {
            db = context;
            cache = memoryCache;
            _rowsNumber = 20;
            resultCacheTime = option * 2 + 240;
        }

        public IEnumerable<NoteStatus> GetNoteStatuses()
        {
            return db.NoteStatuses.Take(_rowsNumber).ToList();
        }

        public void AddNoteStatuses(string cacheKey)
        {
            IEnumerable<NoteStatus> noteStatuses = db.NoteStatuses.Take(_rowsNumber);

            cache.Set(cacheKey, noteStatuses, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(resultCacheTime)
            });
        }

        public IEnumerable<NoteStatus> GetNoteStatuses(string cacheKey)
        {
            IEnumerable<NoteStatus> noteStatuses = null;
            if (!cache.TryGetValue(cacheKey, out noteStatuses))
            {
                noteStatuses = db.NoteStatuses.Take(_rowsNumber).ToList();
                if (noteStatuses != null)
                {
                    cache.Set(cacheKey, noteStatuses, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(resultCacheTime)));
                }
            }
            return noteStatuses;
        }

    }
}
