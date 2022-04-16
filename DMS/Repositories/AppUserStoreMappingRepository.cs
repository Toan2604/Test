using DMS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Repositories
{
    public interface IAppUserStoreMappingRepository
    {

        Task<bool> Update(long AppUserId, long StoreId);
        Task<bool> Delete(long? AppUserId, long StoreId);
    }
    public class AppUserStoreMappingRepository : IAppUserStoreMappingRepository
    {
        private DataContext DataContext;
        public AppUserStoreMappingRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Delete(long? AppUserId, long StoreId)
        {
            await DataContext.AppUserStoreMapping
                .Where(x => x.StoreId == StoreId)
                .Where(x => AppUserId.HasValue == false || (AppUserId.HasValue && x.AppUserId == AppUserId.Value))
                .DeleteFromQueryAsync();
            return true;
        }

        public async Task<bool> Update(long AppUserId, long StoreId)
        {
            AppUserStoreMappingDAO AppUserStoreMappingDAO = new AppUserStoreMappingDAO
            {
                StoreId = StoreId,
                AppUserId = AppUserId,
            };
            await DataContext.BulkMergeAsync(new List<AppUserStoreMappingDAO> { AppUserStoreMappingDAO });
            return true;
        }
    }
}
