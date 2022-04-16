using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Rpc.lucky_draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MLuckyDrawRegistration
{
    public interface ILuckyDrawRegistrationService : IServiceScoped
    {
        Task<int> Count(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<List<LuckyDrawRegistration>> List(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<List<LuckyDrawRegistration>> ListHistory(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<int> CountHistory(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
        Task<LuckyDrawRegistration> Get(long Id);
        Task<LuckyDrawRegistration> Create(LuckyDrawRegistration LuckyDrawRegistration);
        Task<LuckyDrawRegistration> Update(LuckyDrawRegistration LuckyDrawRegistration);
        Task<LuckyDrawRegistration> Delete(LuckyDrawRegistration LuckyDrawRegistration);
        Task<List<LuckyDrawRegistration>> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations);
        Task<List<LuckyDrawRegistration>> Import(List<LuckyDrawRegistration> LuckyDrawRegistrations);
        Task<LuckyDrawRegistrationFilter> ToFilter(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter);
    }

    public class LuckyDrawRegistrationService : BaseService, ILuckyDrawRegistrationService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILuckyDrawRegistrationTemplate LuckyDrawRegistrationTemplate;
        private ILogging Logging;
        private ICurrentContext CurrentContext;

        private ILuckyDrawRegistrationValidator LuckyDrawRegistrationValidator;

        public LuckyDrawRegistrationService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ILuckyDrawRegistrationTemplate LuckyDrawRegistrationTemplate,
            IRabbitManager RabbitManager,
            ILuckyDrawRegistrationValidator LuckyDrawRegistrationValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.LuckyDrawRegistrationTemplate = LuckyDrawRegistrationTemplate;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;

            this.LuckyDrawRegistrationValidator = LuckyDrawRegistrationValidator;
        }
        public async Task<int> Count(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter)
        {
            try
            {
                int result = await UOW.LuckyDrawRegistrationRepository.Count(LuckyDrawRegistrationFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return 0;
        }

        public async Task<List<LuckyDrawRegistration>> List(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter)
        {
            try
            {
                List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(LuckyDrawRegistrationFilter);
                return LuckyDrawRegistrations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }

        public async Task<List<LuckyDrawRegistration>> ListHistory(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter)
        {
            try
            {
                var take = LuckyDrawRegistrationFilter.Take;
                var skip = LuckyDrawRegistrationFilter.Skip;
                LuckyDrawRegistrationFilter.Take = int.MaxValue;
                LuckyDrawRegistrationFilter.Skip = 0;
                List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(LuckyDrawRegistrationFilter);
                var LuckyDrawRegistrationIds = LuckyDrawRegistrations.Select(x => x.Id).ToList();
                LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
                LuckyDrawWinnerFilter.LuckyDrawId = LuckyDrawRegistrationFilter.LuckyDrawId;
                LuckyDrawWinnerFilter.LuckyDrawRegistrationId = new IdFilter { In = LuckyDrawRegistrationIds };
                LuckyDrawWinnerFilter.Take = int.MaxValue;
                LuckyDrawWinnerFilter.Skip = 0;
                LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
                LuckyDrawWinnerFilter.Used = true;
                List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(LuckyDrawWinnerFilter);
                foreach (LuckyDrawRegistration LuckyDrawRegistration in LuckyDrawRegistrations)
                {
                    LuckyDrawRegistration.LuckyDrawWinners = LuckyDrawWinners.Where(x => x.LuckyDrawRegistrationId == LuckyDrawRegistration.Id).ToList();
                    var count_used = LuckyDrawRegistration.LuckyDrawWinners.Count();
                    LuckyDrawRegistration.UsedTurnCounter = count_used;
                    LuckyDrawRegistration.RemainingTurnCounter = LuckyDrawRegistration.TurnCounter - count_used;
                }
                LuckyDrawRegistrations = await MergeLuckyDrawRegistrations(LuckyDrawRegistrations);
                LuckyDrawRegistrations = LuckyDrawRegistrations.Skip(skip).Take(take).ToList();
                return LuckyDrawRegistrations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }


        public async Task<int> CountHistory(LuckyDrawRegistrationFilter LuckyDrawRegistrationFilter)
        {
            try
            {
                List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(LuckyDrawRegistrationFilter);
                var LuckyDrawRegistrationIds = LuckyDrawRegistrations.Select(x => x.Id).ToList();
                LuckyDrawWinnerFilter LuckyDrawWinnerFilter = new LuckyDrawWinnerFilter();
                LuckyDrawWinnerFilter.LuckyDrawId = LuckyDrawRegistrationFilter.LuckyDrawId;
                LuckyDrawWinnerFilter.LuckyDrawRegistrationId = new IdFilter { In = LuckyDrawRegistrationIds };
                LuckyDrawWinnerFilter.Take = int.MaxValue;
                LuckyDrawWinnerFilter.Skip = 0;
                LuckyDrawWinnerFilter.Selects = LuckyDrawWinnerSelect.ALL;
                List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(LuckyDrawWinnerFilter);
                foreach (LuckyDrawRegistration LuckyDrawRegistration in LuckyDrawRegistrations)
                {
                    LuckyDrawRegistration.LuckyDrawWinners = LuckyDrawWinners.Where(x => x.LuckyDrawRegistrationId == LuckyDrawRegistration.Id).ToList();
                    LuckyDrawRegistration.UsedTurnCounter = LuckyDrawRegistration.TurnCounter - LuckyDrawRegistration.RemainingTurnCounter;
                }
                LuckyDrawRegistrations = await MergeLuckyDrawRegistrations(LuckyDrawRegistrations);
                return LuckyDrawRegistrations.Count();
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return 0;
        }
        public async Task<LuckyDrawRegistration> Get(long Id)
        {
            LuckyDrawRegistration LuckyDrawRegistration = await UOW.LuckyDrawRegistrationRepository.Get(Id);
            await LuckyDrawRegistrationValidator.Get(LuckyDrawRegistration);
            if (LuckyDrawRegistration == null)
                return null;
            return LuckyDrawRegistration;
        }

        public async Task<LuckyDrawRegistration> Create(LuckyDrawRegistration LuckyDrawRegistration)
        {
            if (!await LuckyDrawRegistrationValidator.Create(LuckyDrawRegistration))
                return LuckyDrawRegistration;

            try
            {
                LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDrawRegistration.LuckyDrawId);
                var TurnCounter = (long)Math.Floor(LuckyDrawRegistration.Revenue / LuckyDraw.RevenuePerTurn);

                LuckyDrawRegistration.TurnCounter = TurnCounter;
                LuckyDrawRegistration.RemainingTurnCounter = 0;
                await UOW.LuckyDrawRegistrationRepository.Create(LuckyDrawRegistration);
                List<LuckyDrawWinner> LuckyDrawWinners = Enumerable.Range(1, (int)TurnCounter).Select(x => new LuckyDrawWinner
                {
                    LuckyDrawId = LuckyDrawRegistration.LuckyDrawId,
                    LuckyDrawRegistrationId = LuckyDrawRegistration.Id,
                    Time = StaticParams.DateTimeNow,
                }).ToList();
                await UOW.LuckyDrawWinnerRepository.BulkMerge(LuckyDrawWinners);

                LuckyDrawRegistration = await UOW.LuckyDrawRegistrationRepository.Get(LuckyDrawRegistration.Id);
                if (LuckyDrawRegistration.IsDrawnByStore)
                {
                    List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                    var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
                    DateTime Now = StaticParams.DateTimeNow;
                    var RecipientId = LuckyDrawRegistration.StoreId;
                    var StoreUser = (await UOW.StoreUserRepository.List(new StoreUserFilter
                    {
                        Take = 1,
                        Skip = 0,
                        StoreId = new IdFilter { Equal = RecipientId },
                        Selects = StoreUserSelect.Row,
                    })).FirstOrDefault();
                    GlobalUserNotification UserNotification = LuckyDrawRegistrationTemplate.CreateStoreUserNotification(CurrentUser.RowId, StoreUser.RowId, LuckyDraw, CurrentUser, NotificationType.CREATE);
                    GlobalUserNotifications.Add(UserNotification);
                    RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);
                }
                Sync(new List<LuckyDrawRegistration> { LuckyDrawRegistration });
                Logging.CreateAuditLog(LuckyDrawRegistration, new { }, nameof(LuckyDrawRegistrationService));
                return LuckyDrawRegistration;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }

        public async Task<LuckyDrawRegistration> Update(LuckyDrawRegistration LuckyDrawRegistration)
        {
            if (!await LuckyDrawRegistrationValidator.Update(LuckyDrawRegistration))
                return LuckyDrawRegistration;
            try
            {
                var oldData = await UOW.LuckyDrawRegistrationRepository.Get(LuckyDrawRegistration.Id);
                await UOW.LuckyDrawRegistrationRepository.Update(LuckyDrawRegistration);
                LuckyDrawRegistration = await UOW.LuckyDrawRegistrationRepository.Get(LuckyDrawRegistration.Id);
                Sync(new List<LuckyDrawRegistration> { LuckyDrawRegistration });
                Logging.CreateAuditLog(LuckyDrawRegistration, oldData, nameof(LuckyDrawRegistrationService));
                return LuckyDrawRegistration;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }

        public async Task<LuckyDrawRegistration> Delete(LuckyDrawRegistration LuckyDrawRegistration)
        {
            if (!await LuckyDrawRegistrationValidator.Delete(LuckyDrawRegistration))
                return LuckyDrawRegistration;

            try
            {
                await UOW.LuckyDrawRegistrationRepository.Delete(LuckyDrawRegistration);
                Logging.CreateAuditLog(new { }, LuckyDrawRegistration, nameof(LuckyDrawRegistrationService));
                return LuckyDrawRegistration;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }

        public async Task<List<LuckyDrawRegistration>> BulkDelete(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            if (!await LuckyDrawRegistrationValidator.BulkDelete(LuckyDrawRegistrations))
                return LuckyDrawRegistrations;

            try
            {
                await UOW.LuckyDrawRegistrationRepository.BulkDelete(LuckyDrawRegistrations);
                Logging.CreateAuditLog(new { }, LuckyDrawRegistrations, nameof(LuckyDrawRegistrationService));
                return LuckyDrawRegistrations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;

        }

        public async Task<List<LuckyDrawRegistration>> Import(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            if (!await LuckyDrawRegistrationValidator.Import(LuckyDrawRegistrations))
                return LuckyDrawRegistrations;
            try
            {
                await UOW.LuckyDrawRegistrationRepository.BulkMerge(LuckyDrawRegistrations);

                Logging.CreateAuditLog(LuckyDrawRegistrations, new { }, nameof(LuckyDrawRegistrationService));
                return LuckyDrawRegistrations;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawRegistrationService));
            }
            return null;
        }

        public async Task<LuckyDrawRegistrationFilter> ToFilter(LuckyDrawRegistrationFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawRegistrationFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawRegistrationFilter subFilter = new LuckyDrawRegistrationFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawId))
                        subFilter.LuckyDrawId = FilterBuilder.Merge(subFilter.LuckyDrawId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.AppUserId))
                        subFilter.AppUserId = FilterBuilder.Merge(subFilter.AppUserId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterBuilder.Merge(subFilter.StoreId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Revenue))
                        subFilter.Revenue = FilterBuilder.Merge(subFilter.Revenue, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.TurnCounter))
                        subFilter.TurnCounter = FilterBuilder.Merge(subFilter.TurnCounter, FilterPermissionDefinition.LongFilter);
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

        private async Task<List<LuckyDrawRegistration>> MergeLuckyDrawRegistrations(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            List<LuckyDrawRegistration> UniqueLuckyDrawRegistrations = new List<LuckyDrawRegistration>();
            foreach (var LuckyDrawRegistration in LuckyDrawRegistrations)
            {
                var LuckyDrawRegistrationCheck = UniqueLuckyDrawRegistrations.Where(x => x.LuckyDrawId == LuckyDrawRegistration.LuckyDrawId &&
                                                                                x.StoreId == LuckyDrawRegistration.StoreId).FirstOrDefault();
                if (LuckyDrawRegistrationCheck == null)
                {
                    UniqueLuckyDrawRegistrations.Add(LuckyDrawRegistration);
                }
                else
                {
                    LuckyDrawRegistrationCheck.TurnCounter += LuckyDrawRegistration.TurnCounter;
                    LuckyDrawRegistrationCheck.RemainingTurnCounter += LuckyDrawRegistration.RemainingTurnCounter;
                    LuckyDrawRegistrationCheck.Revenue += LuckyDrawRegistration.Revenue;
                    LuckyDrawRegistrationCheck.UsedTurnCounter += LuckyDrawRegistration.UsedTurnCounter;
                    LuckyDrawRegistrationCheck.LuckyDrawWinners = LuckyDrawRegistrationCheck.LuckyDrawWinners.Union(LuckyDrawRegistration.LuckyDrawWinners).Distinct().ToList();
                }
            }

            return UniqueLuckyDrawRegistrations;
        }
        private void Sync(List<LuckyDrawRegistration> LuckyDrawRegistrations)
        {
            List<LuckyDraw> LuckyDraws = LuckyDrawRegistrations.Select(x => new LuckyDraw { Id = x.LuckyDrawId }).Distinct().ToList();
            RabbitManager.PublishList(LuckyDraws, RoutingKeyEnum.LuckyDrawUsed.Code);
        }
    }
}
