using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using QForum.Web.Models.AppSettings;

namespace QForum.Web.Storage
{
    public abstract class TableStorageProvider
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTableClient _tableClient;

        protected TableStorageProvider(IOptions<ConnectionStrings> connectionStrings)
        {
            if (string.IsNullOrEmpty(connectionStrings.Value?.AzureStorage))
                return;

            _storageAccount = CloudStorageAccount.Parse(connectionStrings.Value.AzureStorage);
            _tableClient = _storageAccount.CreateCloudTableClient();
        }

        public async Task<CloudTable> GetOrCreateTableAsync(string tableName)
        {
            var table = _tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public async Task<TableResult> InsertAsync<TEntity>(string tableName, TEntity entity) where TEntity : TableEntity
        {
            var table = await GetOrCreateTableAsync(tableName);
            var insertOperation = TableOperation.Insert(entity);
            return await table.ExecuteAsync(insertOperation);
        }
    }
}