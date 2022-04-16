using TrueSight.Common;
using DMS.ABE.Common;
using DMS.ABE.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.ABE.Repositories;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Handlers.Configuration;

namespace DMS.ABE.Services.MLuckyDrawWinner
{
    public interface ILuckyDrawWinnerService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<List<LuckyDrawWinner>> List(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<LuckyDrawWinner> Get(long Id);
        Task<LuckyDrawWinner> Draw(LuckyDrawWinner LuckyDrawWinner);
        Task<LuckyDrawWinnerFilter> ToFilter(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
    }

    public class LuckyDrawWinnerService : BaseService, ILuckyDrawWinnerService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        private ILuckyDrawWinnerValidator LuckyDrawWinnerValidator;

        public LuckyDrawWinnerService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IRabbitManager RabbitManager,
            ILuckyDrawWinnerValidator LuckyDrawWinnerValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;

            this.LuckyDrawWinnerValidator = LuckyDrawWinnerValidator;
        }
        public async Task<int> Count(LuckyDrawWinnerFilter LuckyDrawWinnerFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawWinnerRepository.Count(LuckyDrawWinnerFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return 0;
        }

        public async Task<List<LuckyDrawWinner>> List(LuckyDrawWinnerFilter LuckyDrawWinnerFilter)
        {
            try
            {
                List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(LuckyDrawWinnerFilter);
                return LuckyDrawWinners;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }

        public async Task<LuckyDrawWinner> Get(long Id)
        {
            LuckyDrawWinner LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(Id);
            await LuckyDrawWinnerValidator.Get(LuckyDrawWinner);
            if (LuckyDrawWinner == null)
                return null;
            return LuckyDrawWinner;
        }
        public async Task<LuckyDrawWinner> Draw(LuckyDrawWinner LuckyDrawWinner)
        {
            try
            {
                LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);
                //nếu đã quay thì return giải
                if (LuckyDrawWinner.LuckyDrawNumberId.HasValue) return LuckyDrawWinner;
                Store Store = await GetStore();

                var LuckyDrawStructures = await UOW.LuckyDrawStructureRepository.List(new LuckyDrawStructureFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawStructureSelect.Id,
                    LuckyDrawId = new IdFilter { Equal = LuckyDrawWinner.LuckyDrawId }
                });
                var LuckyDrawNumberId = await GetRandomNumber(LuckyDrawStructures);
                LuckyDrawNumber LuckyDrawNumber = await UOW.LuckyDrawNumberRepository.Get(LuckyDrawNumberId);
                LuckyDrawWinner.LuckyDrawNumberId = LuckyDrawNumberId;
                LuckyDrawWinner.LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId;
                LuckyDrawWinner.Time = StaticParams.DateTimeNow;
                await UOW.LuckyDrawWinnerRepository.Update(LuckyDrawWinner);
                LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);
                //TODO khi làm với app đại lý thì sẽ ko cần tính số lượt còn lại nữa
                //var LuckyDrawRegistration = await UOW.LuckyDrawRegistrationRepository.Get(LuckyDrawWinner.LuckyDrawRegistrationId); 
                //await UOW.LuckyDrawRegistrationRepository.UpdateTurnCounter(LuckyDrawWinner.LuckyDrawRegistrationId, LuckyDrawRegistration.RemainingTurnCounter - 1);
                await UOW.LuckyDrawNumberRepository.Used(new List<long> { LuckyDrawNumberId });
                Logging.CreateAuditLog(LuckyDrawWinner, new { }, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinner;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }

        public async Task<LuckyDrawWinnerFilter> ToFilter(LuckyDrawWinnerFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawWinnerFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawWinnerFilter subFilter = new LuckyDrawWinnerFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawId))
                        subFilter.LuckyDrawId = FilterBuilder.Merge(subFilter.LuckyDrawId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawStructureId))
                        subFilter.LuckyDrawStructureId = FilterBuilder.Merge(subFilter.LuckyDrawStructureId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawRegistrationId))
                        subFilter.LuckyDrawRegistrationId = FilterBuilder.Merge(subFilter.LuckyDrawRegistrationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Time))
                        subFilter.Time = FilterBuilder.Merge(subFilter.Time, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }
        private List<long> GenerateLuckyDrawNumberId(long start, long end, long prefix)
        {
            List<long> list = new List<long>();
            var limit = prefix * 1_000_000;
            for (long i = start; i <= end; i++)
            {
                list.Add(limit + i);
            }
            return list;
        }
        private async Task<long> GetRandomNumber(List<LuckyDrawStructure> LuckyDrawStructures)
        {
            List<long> LuckyDrawStructureIds = LuckyDrawStructures.Select(x => x.Id).ToList();
            List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(new LuckyDrawNumberFilter
            {
                Take = int.MaxValue,
                Selects = LuckyDrawNumberSelect.Id,
                Skip = 0,
                LuckyDrawStructureId = new IdFilter { In = LuckyDrawStructureIds },
                Used = false,
            });
            List<long> LuckyDrawNumberIds = LuckyDrawNumbers.Select(x => x.Id).ToList();
            var random = new Random();
            var result = random.Next(LuckyDrawNumberIds.Count);
            return LuckyDrawNumberIds.ElementAt(result);
        }
        private async Task<Store> GetStore()
        {
            var StoreUserId = CurrentContext.StoreUserId;
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(StoreUserId);
            if (StoreUser == null)
            {
                return null;
            } // check storeUser co ton tai khong
            Store Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
            if (Store == null)
            {
                return null;
            } // check store tuong ung vs storeUser co ton tai khong
            return Store;
        }
    }
}
