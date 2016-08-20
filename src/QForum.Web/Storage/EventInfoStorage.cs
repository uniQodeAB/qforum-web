using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using QForum.Web.Entities;
using QForum.Web.Models.AppSettings;
using System.Linq;

namespace QForum.Web.Storage
{
    public class EventInfoStorage : TableStorageProvider, IEventInfoStorage
    {
        private readonly IMemoryCache _memoryCache;
        private const string TableName = "EventInfo";

        public EventInfoStorage(IMemoryCache memoryCache, IOptions<ConnectionStrings> connectionStrings)
            : base(connectionStrings)
        {
            _memoryCache = memoryCache;
        }

        public async Task<int> InsertAsync(EventInfoEntity entity)
        {
            var result = await base.InsertAsync(TableName, entity);
            return result.HttpStatusCode;
        }

        public async Task<List<EventInfoEntity>> GetAllAsync()
        {
            const string cacheKey = "EventInfoStorage.GetAllAsync";
            List<EventInfoEntity> list;

            if (_memoryCache.TryGetValue(cacheKey, out list))
                return list;

            var table = await GetOrCreateTableAsync(TableName);

            // Initialize a default TableQuery to retrieve all the entities in the table.
            var tableQuery = new TableQuery<EventInfoEntity>();

            // Initialize the continuation token to null to start from the beginning of the table.
            TableContinuationToken continuationToken = null;


            list = new List<EventInfoEntity>();

            do
            {
                // Retrieve a segment (up to 1,000 entities).
                var tableQueryResult =
                    await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

                // Assign the new continuation token to tell the service where to
                // continue on the next iteration (or null if it has reached the end).
                continuationToken = tableQueryResult.ContinuationToken;


                list.AddRange(tableQueryResult.Results);

                // Loop until a null continuation token is received, indicating the end of the table.
            } while (continuationToken != null);
            
            _memoryCache.Set(cacheKey, list,
                new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));

            return list;
        }

        public async Task<EventInfoEntity> GetLatestAsync()
        {
            var all = await GetAllAsync();
            all = all.OrderByDescending(e => e.EventDate).ToList();
            return all.FirstOrDefault();
        }
    }
}