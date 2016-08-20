using System.Collections.Generic;
using System.Threading.Tasks;
using QForum.Web.Entities;

namespace QForum.Web.Storage
{
    public interface IEventInfoStorage
    {
        Task<int> InsertAsync(EventInfoEntity entity);

        Task<List<EventInfoEntity>> GetAllAsync();

        Task<EventInfoEntity> GetLatestAsync();
    }
}