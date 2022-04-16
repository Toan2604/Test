using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MStoreType
{
    public interface IStoreTypeService : IServiceScoped
    {
        Task<int> Count(StoreTypeFilter StoreTypeFilter);
        Task<List<StoreType>> List(StoreTypeFilter StoreTypeFilter);
        Task<StoreType> Get(long Id);
        Task<StoreType> Create(StoreType StoreType);
        Task<StoreType> Update(StoreType StoreType);
        Task<StoreType> Delete(StoreType StoreType);
        Task<List<StoreType>> BulkMerge(List<StoreType> StoreTypes);
        Task<List<StoreType>> BulkDelete(List<StoreType> StoreTypes);
    }

    public class StoreTypeService : BaseService, IStoreTypeService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreTypeValidator StoreTypeValidator;
        private IRabbitManager RabbitManager;

        public StoreTypeService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreTypeValidator StoreTypeValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreTypeValidator = StoreTypeValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreTypeFilter StoreTypeFilter)
        {
            try
            {
                int result = await UOW.StoreTypeRepository.Count(StoreTypeFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return 0;
        }
        public async Task<List<StoreType>> List(StoreTypeFilter StoreTypeFilter)
        {
            try
            {
                List<StoreType> StoreTypes = await UOW.StoreTypeRepository.List(StoreTypeFilter);
                return StoreTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        public async Task<StoreType> Get(long Id)
        {
            StoreType StoreType = await UOW.StoreTypeRepository.Get(Id);
            if (StoreType == null)
                return null;
            return StoreType;
        }
        public async Task<StoreType> Create(StoreType StoreType)
        {
            if (!await StoreTypeValidator.Create(StoreType))
                return StoreType;

            try
            {

                await UOW.StoreTypeRepository.Create(StoreType);


                var newData = await UOW.StoreTypeRepository.Get(StoreType.Id);
                Sync(new List<StoreType> { newData });

                Logging.CreateAuditLog(StoreType, new { }, nameof(StoreTypeService));
                return await UOW.StoreTypeRepository.Get(StoreType.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        public async Task<StoreType> Update(StoreType StoreType)
        {
            if (!await StoreTypeValidator.Update(StoreType))
                return StoreType;
            try
            {
                var oldData = await UOW.StoreTypeRepository.Get(StoreType.Id);


                await UOW.StoreTypeRepository.Update(StoreType);


                var newData = await UOW.StoreTypeRepository.Get(StoreType.Id);
                Sync(new List<StoreType> { newData });

                Logging.CreateAuditLog(newData, oldData, nameof(StoreTypeService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        public async Task<StoreType> Delete(StoreType StoreType)
        {
            if (!await StoreTypeValidator.Delete(StoreType))
                return StoreType;

            try
            {

                await UOW.StoreTypeRepository.Delete(StoreType);


                var newData = await UOW.StoreTypeRepository.Get(StoreType.Id);
                Sync(new List<StoreType> { newData });

                Logging.CreateAuditLog(new { }, StoreType, nameof(StoreTypeService));
                return StoreType;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        public async Task<List<StoreType>> BulkDelete(List<StoreType> StoreTypes)
        {
            if (!await StoreTypeValidator.BulkDelete(StoreTypes))
                return StoreTypes;

            try
            {

                await UOW.StoreTypeRepository.BulkDelete(StoreTypes);


                var Ids = StoreTypes.Select(x => x.Id).ToList();
                StoreTypes = await UOW.StoreTypeRepository.List(Ids);
                Sync(StoreTypes);

                Logging.CreateAuditLog(new { }, StoreTypes, nameof(StoreTypeService));
                return StoreTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        public async Task<List<StoreType>> BulkMerge(List<StoreType> StoreTypes)
        {
            if (!await StoreTypeValidator.Import(StoreTypes))
                return StoreTypes;
            try
            {
                await UOW.StoreTypeRepository.BulkMerge(StoreTypes);   
                List<long> Ids = StoreTypes.Select(x => x.Id).ToList();
                StoreTypes = await UOW.StoreTypeRepository.List(Ids);
                Sync(StoreTypes);

                Logging.CreateAuditLog(StoreTypes, new { }, nameof(StoreTypeService));
                return StoreTypes;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreTypeService));
            }
            return null;
        }
        private void Sync(List<StoreType> StoreTypes)
        {
            RabbitManager.PublishList(StoreTypes, RoutingKeyEnum.StoreTypeSync.Code);
        }
    }
}
