using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MLuckyNumber
{
    public interface ILuckyNumberService : IServiceScoped
    {
        Task<int> Count(LuckyNumberFilter LuckyNumberFilter);
        Task<List<LuckyNumber>> List(LuckyNumberFilter LuckyNumberFilter);
        Task<LuckyNumber> LuckyDraw(long RewardHistoryId);
        Task<LuckyNumber> Get(long Id);
        Task<LuckyNumber> Create(LuckyNumber LuckyNumber);
        Task<LuckyNumber> Update(LuckyNumber LuckyNumber);
        Task<LuckyNumber> Delete(LuckyNumber LuckyNumber);
        Task<List<LuckyNumber>> BulkDelete(List<LuckyNumber> LuckyNumbers);
        Task<List<LuckyNumber>> Import(List<LuckyNumber> LuckyNumbers);
        Task<LuckyNumberFilter> ToFilter(LuckyNumberFilter LuckyNumberFilter);
    }

    public class LuckyNumberService : BaseService, ILuckyNumberService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private ILuckyNumberValidator LuckyNumberValidator;

        public LuckyNumberService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            ILuckyNumberValidator LuckyNumberValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.LuckyNumberValidator = LuckyNumberValidator;
        }
        public async Task<int> Count(LuckyNumberFilter LuckyNumberFilter)
        {
            try
            {
                int result = await UOW.LuckyNumberRepository.Count(LuckyNumberFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return 0;
        }

        public async Task<List<LuckyNumber>> List(LuckyNumberFilter LuckyNumberFilter)
        {
            try
            {
                List<LuckyNumber> LuckyNumbers = await UOW.LuckyNumberRepository.List(LuckyNumberFilter);
                return LuckyNumbers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }
        public async Task<LuckyNumber> Get(long Id)
        {
            LuckyNumber LuckyNumber = await UOW.LuckyNumberRepository.Get(Id);
            if (LuckyNumber == null)
                return null;
            return LuckyNumber;
        }

        public async Task<LuckyNumber> LuckyDraw(long RewardHistoryId)
        {
            RewardHistory RewardHistory = await UOW.RewardHistoryRepository.Get(RewardHistoryId);
            if (RewardHistory != null && RewardHistory.TurnCounter > 0)
            {
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                var Organizations = await UOW.OrganizationRepository.List(new OrganizationFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = OrganizationSelect.Id | OrganizationSelect.Path
                });
                var OrganizationIds = Organizations.Where(x => CurrentUser.Organization.Path.StartsWith(x.Path)).Select(x => x.Id).ToList();

                List<LuckyNumberGrouping> LuckyNumberGroupings = await UOW.LuckyNumberGroupingRepository.List(new LuckyNumberGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = LuckyNumberGroupingSelect.Id,
                    OrganizationId = new IdFilter { In = OrganizationIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
                });
                var LuckyNumberGroupingIds = LuckyNumberGroupings.Select(x => x.Id).ToList();
                List<LuckyNumber> LuckyNumbers = await UOW.LuckyNumberRepository.List(new LuckyNumberFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = LuckyNumberSelect.ALL,
                    LuckyNumberGroupingId = new IdFilter { In = LuckyNumberGroupingIds },
                    RewardStatusId = new IdFilter { Equal = RewardStatusEnum.ACTIVE.Id }
                });

                if (LuckyNumbers.Count() >= 1)
                {
                    Random rnd = new Random();
                    var RandomNumber = LuckyNumbers.OrderBy(x => rnd.Next()).FirstOrDefault();

                    RandomNumber.RewardStatusId = RewardStatusEnum.INACTIVE.Id;
                    RandomNumber.Used = true;
                    RandomNumber.UsedAt = StaticParams.DateTimeNow;
                    await UOW.LuckyNumberRepository.Update(RandomNumber);

                    if (RewardHistory.RewardHistoryContents == null)
                    {
                        RewardHistory.RewardHistoryContents = new List<RewardHistoryContent>();
                    }
                    RewardHistory.RewardHistoryContents.Add(new RewardHistoryContent
                    {
                        LuckyNumberId = RandomNumber.Id,
                        RewardHistoryId = RewardHistory.Id
                    });
                    await UOW.RewardHistoryRepository.Update(RewardHistory);

                    return RandomNumber;
                }
            }
            return null;
        }

        public async Task<LuckyNumber> Create(LuckyNumber LuckyNumber)
        {
            if (!await LuckyNumberValidator.Create(LuckyNumber))
                return LuckyNumber;

            try
            {

                await UOW.LuckyNumberRepository.Create(LuckyNumber);

                LuckyNumber = await UOW.LuckyNumberRepository.Get(LuckyNumber.Id);
                Logging.CreateAuditLog(LuckyNumber, new { }, nameof(LuckyNumberService));
                return LuckyNumber;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }

        public async Task<LuckyNumber> Update(LuckyNumber LuckyNumber)
        {
            if (!await LuckyNumberValidator.Update(LuckyNumber))
                return LuckyNumber;
            try
            {
                var oldData = await UOW.LuckyNumberRepository.Get(LuckyNumber.Id);


                await UOW.LuckyNumberRepository.Update(LuckyNumber);


                LuckyNumber = await UOW.LuckyNumberRepository.Get(LuckyNumber.Id);
                Logging.CreateAuditLog(LuckyNumber, oldData, nameof(LuckyNumberService));
                return LuckyNumber;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }

        public async Task<LuckyNumber> Delete(LuckyNumber LuckyNumber)
        {
            if (!await LuckyNumberValidator.Delete(LuckyNumber))
                return LuckyNumber;

            try
            {

                await UOW.LuckyNumberRepository.Delete(LuckyNumber);

                Logging.CreateAuditLog(new { }, LuckyNumber, nameof(LuckyNumberService));
                return LuckyNumber;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }

        public async Task<List<LuckyNumber>> BulkDelete(List<LuckyNumber> LuckyNumbers)
        {
            if (!await LuckyNumberValidator.BulkDelete(LuckyNumbers))
                return LuckyNumbers;

            try
            {

                await UOW.LuckyNumberRepository.BulkDelete(LuckyNumbers);

                Logging.CreateAuditLog(new { }, LuckyNumbers, nameof(LuckyNumberService));
                return LuckyNumbers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }

        public async Task<List<LuckyNumber>> Import(List<LuckyNumber> LuckyNumbers)
        {
            if (!await LuckyNumberValidator.Import(LuckyNumbers))
                return LuckyNumbers;
            try
            {

                await UOW.LuckyNumberRepository.BulkMerge(LuckyNumbers);


                Logging.CreateAuditLog(LuckyNumbers, new { }, nameof(LuckyNumberService));
                return LuckyNumbers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyNumberService));
            }
            return null;
        }

        public async Task<LuckyNumberFilter> ToFilter(LuckyNumberFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyNumberFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyNumberFilter subFilter = new LuckyNumberFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RewardStatusId))
                        subFilter.RewardStatusId = FilterBuilder.Merge(subFilter.RewardStatusId, FilterPermissionDefinition.IdFilter);
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
    }
}
