using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MLuckyDraw
{
    public interface ILuckyDrawService : IServiceScoped
    {
        Task<int> Count(LuckyDrawFilter LuckyDrawFilter);
        Task<int> CountHistory(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> List(LuckyDrawFilter LuckyDrawFilter);
        Task<List<LuckyDraw>> ListHistory(LuckyDrawFilter LuckyDrawFilter);
        Task<LuckyDraw> Get(long Id);
        Task<LuckyDraw> Draw(LuckyDraw LuckyDraw);
        Task<LuckyDrawFilter> ToFilter(LuckyDrawFilter LuckyDrawFilter);
    }

    public class LuckyDrawService : BaseService, ILuckyDrawService
    {
        private IUOW UOW;
        private ILuckyDrawValidator LuckyDrawValidator;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        

        public LuckyDrawService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            ILuckyDrawValidator LuckyDrawValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.LuckyDrawValidator = LuckyDrawValidator;
            this.CurrentContext = CurrentContext;
            this.Logging = Logging;
        }
        public async Task<int> Count(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                Store Store = await GetStore();
                LuckyDrawFilter.StoreId = new IdFilter { Equal = Store.Id };
                LuckyDrawFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
                int result = await UOW.LuckyDrawRepository.Count(LuckyDrawFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return 0;
        }

        public async Task<List<LuckyDraw>> List(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                Store Store = await GetStore();
                LuckyDrawFilter.StoreId = new IdFilter { Equal = Store.Id };
                LuckyDrawFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
                List<LuckyDraw> LuckyDraws = await UOW.LuckyDrawRepository.List(LuckyDrawFilter);
                List<long> LuckyDrawIds = LuckyDraws.Select(x => x.Id).ToList();
                List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(new LuckyDrawWinnerFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawWinnerSelect.ALL,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds },
                    Used = true
                });
                List<LuckyDrawStructure> LuckyDrawStructures = await UOW.LuckyDrawStructureRepository.List(new LuckyDrawStructureFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawStructureSelect.LuckyDraw | LuckyDrawStructureSelect.Quantity,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds }
                });
                List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawRegistrationSelect.ALL,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds }
                });
                long TotalPrize, UsedPrize, TotalStoreTurnCounter, UsedStoreTurnCounter, RemainingStoreTurnCounter;
                foreach (LuckyDraw LuckyDraw in LuckyDraws)
                {
                    TotalPrize = LuckyDrawStructures.Where(x => x.LuckyDrawId == LuckyDraw.Id).Sum(x => x.Quantity);
                    UsedPrize = LuckyDrawWinners.Count(x => x.LuckyDrawId == LuckyDraw.Id);
                    var Registrations = LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id && x.LuckyDrawId == LuckyDraw.Id).ToList();
                    var RegistrationForStore = Registrations.Where(x => x.IsDrawnByStore == true).ToList();
                    var RegistrationIds = RegistrationForStore.Select(x => x.Id).ToList();
                    TotalStoreTurnCounter = RegistrationForStore.Sum(x => x.TurnCounter); //chỉ hiện số lượt quay mà store được phép tự quay
                    var StoreLuckyDrawWinners = LuckyDrawWinners.Where(x => RegistrationIds.Contains(x.LuckyDrawRegistrationId)).ToList(); //giải đã quay được
                    UsedStoreTurnCounter = StoreLuckyDrawWinners.Count(); //tổng số lượt đã quay
                    RemainingStoreTurnCounter = TotalStoreTurnCounter - UsedStoreTurnCounter; //tổng số lượt chưa quay
                    LuckyDraw.UsedTurnCounter = UsedStoreTurnCounter;
                    LuckyDraw.RemainingTurnCounter = RemainingStoreTurnCounter;
                    LuckyDraw.RemainingLuckyDrawStructureCounter = TotalPrize - UsedPrize;
                    LuckyDraw.LuckyDrawWinners = StoreLuckyDrawWinners;
                }

                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }
        public async Task<int> CountHistory(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                Store Store = await GetStore();
                LuckyDrawFilter.StoreId = new IdFilter { Equal = Store.Id };
                int result = await UOW.LuckyDrawRepository.Count(LuckyDrawFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return 0;
        }

        public async Task<List<LuckyDraw>> ListHistory(LuckyDrawFilter LuckyDrawFilter)
        {
            try
            {
                Store Store = await GetStore();
                LuckyDrawFilter.StoreId = new IdFilter { Equal = Store.Id };
                LuckyDrawFilter.OrganizationId = new IdFilter { Equal = Store.OrganizationId };
                List<LuckyDraw> LuckyDraws = await UOW.LuckyDrawRepository.List(LuckyDrawFilter);
                List<long> LuckyDrawIds = LuckyDraws.Select(x => x.Id).ToList();
                List<LuckyDrawWinner> LuckyDrawWinners = await UOW.LuckyDrawWinnerRepository.List(new LuckyDrawWinnerFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawWinnerSelect.ALL,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds }
                });
                List<LuckyDrawStructure> LuckyDrawStructures = await UOW.LuckyDrawStructureRepository.List(new LuckyDrawStructureFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawStructureSelect.LuckyDraw | LuckyDrawStructureSelect.Quantity,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds }
                });
                List<LuckyDrawRegistration> LuckyDrawRegistrations = await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    Selects = LuckyDrawRegistrationSelect.ALL,
                    LuckyDrawId = new IdFilter { In = LuckyDrawIds }
                });
                long TotalPrize, UsedPrize, TotalStoreTurnCounter, UsedStoreTurnCounter, RemainingStoreTurnCounter;
                foreach (LuckyDraw LuckyDraw in LuckyDraws)
                {
                    TotalPrize = LuckyDrawStructures.Where(x => x.LuckyDrawId == LuckyDraw.Id).Sum(x => x.Quantity);
                    UsedPrize = LuckyDrawWinners.Count(x => x.LuckyDrawId == LuckyDraw.Id);
                    var Registrations = LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id && x.LuckyDrawId == LuckyDraw.Id).ToList();
                    var RegistrationIds = Registrations.Select(x => x.Id).ToList();
                    //var RegistrationForStore = Registrations.Where(x => x.IsDrawnByStore == true).ToList();
                    TotalStoreTurnCounter = Registrations.Sum(x => x.TurnCounter); //số lượt của cả nhân viên và store
                    RemainingStoreTurnCounter = Registrations.Sum(x => x.RemainingTurnCounter);
                    UsedStoreTurnCounter = Registrations.Sum(x => x.TurnCounter) - Registrations.Sum(x => x.RemainingTurnCounter);
                    LuckyDraw.UsedTurnCounter = UsedStoreTurnCounter;
                    LuckyDraw.RemainingTurnCounter = RemainingStoreTurnCounter;
                    LuckyDraw.RemainingLuckyDrawStructureCounter = TotalPrize - UsedPrize;
                    LuckyDraw.LuckyDrawWinners = LuckyDrawWinners.Where(x => RegistrationIds.Contains(x.LuckyDrawRegistrationId)).ToList(); //lấy cả lịch sử do NV quay
                }

                return LuckyDraws;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }

        public async Task<LuckyDraw> Get(long Id)
        {
            try
            {
                Store Store = await GetStore();
                //LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(Id);
                //LuckyDraw.RemainingLuckyDrawStructureCounter = LuckyDraw.LuckyDrawStructures.Where(x => x.LuckyDrawId == LuckyDraw.Id).Sum(x => x.Quantity) - LuckyDraw.LuckyDrawWinners.Count;
                //LuckyDraw.LuckyDrawRegistrations = LuckyDraw.LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id).ToList();
                //var LuckyDrawRegistrationIds = LuckyDraw.LuckyDrawRegistrations.Select(x => x.Id).ToList();
                //LuckyDraw.LuckyDrawWinners = LuckyDraw.LuckyDrawWinners.Where(x => LuckyDrawRegistrationIds.Contains(x.LuckyDrawRegistrationId)).ToList();
                //LuckyDraw.RemainingTurnCounter = LuckyDraw.LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id && x.IsDrawnByStore == true).Sum(x => x.RemainingTurnCounter);
                //var TotalTurnCounter = LuckyDraw.LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id && x.IsDrawnByStore == true).Sum(x => x.TurnCounter);
                //LuckyDraw.UsedTurnCounter = TotalTurnCounter - LuckyDraw.RemainingTurnCounter;
                //foreach (var Structure in LuckyDraw.LuckyDrawStructures)
                //{
                //    Structure.PrizeCounter = LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawStructureId == Structure.Id);
                //}

                //Lấy các đăng kí quay cho store
                LuckyDraw LuckyDraw = await UOW.LuckyDrawRepository.Get(Id);
                if (LuckyDraw == null)
                    return null;
                var LuckyDrawRegistrations = LuckyDraw.LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id && x.IsDrawnByStore).ToList();
                var LuckyDrawRegistrationIds = LuckyDrawRegistrations.Select(x => x.Id).ToList();
                var TotalTurnCounter = LuckyDrawRegistrations.Sum(x => x.TurnCounter);
                //lấy ra các giải
                LuckyDraw.LuckyDrawWinners = LuckyDraw.LuckyDrawWinners.Where(x => LuckyDrawRegistrationIds.Contains(x.LuckyDrawRegistrationId)).ToList();
                var RemainingTurnCounter = LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawNumberId == null);
                foreach (var Structure in LuckyDraw.LuckyDrawStructures)
                {
                    Structure.PrizeCounter = LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawStructureId == Structure.Id);
                }
                LuckyDraw.RemainingLuckyDrawStructureCounter = LuckyDraw.LuckyDrawStructures.Sum(x => x.Quantity) - LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawNumberId.HasValue);
                return LuckyDraw;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
        }        
        
        public async Task<LuckyDrawFilter> ToFilter(LuckyDrawFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<LuckyDrawFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                LuckyDrawFilter subFilter = new LuckyDrawFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Code))
                        subFilter.Code = FilterBuilder.Merge(subFilter.Code, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Name))
                        subFilter.Name = FilterBuilder.Merge(subFilter.Name, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.LuckyDrawTypeId))
                        subFilter.LuckyDrawTypeId = FilterBuilder.Merge(subFilter.LuckyDrawTypeId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.RevenuePerTurn))
                        subFilter.RevenuePerTurn = FilterBuilder.Merge(subFilter.RevenuePerTurn, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StartAt))
                        subFilter.StartAt = FilterBuilder.Merge(subFilter.StartAt, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.EndAt))
                        subFilter.EndAt = FilterBuilder.Merge(subFilter.EndAt, FilterPermissionDefinition.DateFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.ImageId))
                        subFilter.ImageId = FilterBuilder.Merge(subFilter.ImageId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
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
        public async Task<LuckyDraw> Draw(LuckyDraw LuckyDraw)
        {
            if (!await LuckyDrawValidator.Draw(LuckyDraw))
                return LuckyDraw;
            try
            {
                Store Store = await GetStore();
                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                LuckyDraw.LuckyDrawRegistrations = LuckyDraw.LuckyDrawRegistrations.Where(x => x.StoreId == Store.Id).ToList();
                List<long> LuckyDrawRegistrationIds = LuckyDraw.LuckyDrawRegistrations.Select(x => x.Id).ToList();
                //setup quay thưởng
                var LuckyDrawNumberId = await GetRandomNumber(LuckyDraw.LuckyDrawStructures);
                var LuckyDrawNumber = await UOW.LuckyDrawNumberRepository.Get(LuckyDrawNumberId);
                var LuckyDrawRegistration = LuckyDraw.LuckyDrawRegistrations.Where(x => x.RemainingTurnCounter > 0 && x.IsDrawnByStore == true).FirstOrDefault();//nhân viên ai đăng kí trước thì set id nhân viên vào winner
                var TotalCounter = LuckyDraw.LuckyDrawRegistrations.Where(x => x.IsDrawnByStore == true).Sum(x => x.TurnCounter);

                LuckyDrawWinner LuckyDrawWinner = new LuckyDrawWinner
                {
                    LuckyDrawId = LuckyDraw.Id,
                    LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId,
                    LuckyDrawNumberId = LuckyDrawNumberId,
                    RowId = Guid.NewGuid(),
                    LuckyDrawRegistrationId = LuckyDrawRegistration.Id,
                    Time = StaticParams.DateTimeNow
                };
                await UOW.LuckyDrawWinnerRepository.Create(LuckyDrawWinner);
                LuckyDrawRegistration.RemainingTurnCounter -= 1;
                await UOW.LuckyDrawRegistrationRepository.Update(LuckyDrawRegistration);
                await UOW.LuckyDrawNumberRepository.Used(new List<long> { LuckyDrawNumberId });
                //thiết lập lại các thông số hiển thị sau khi quay thưởng thành công
                LuckyDraw = await UOW.LuckyDrawRepository.Get(LuckyDraw.Id);
                LuckyDraw.LuckyDrawWinners = LuckyDraw.LuckyDrawWinners.Where(x => LuckyDrawRegistrationIds.Contains(x.LuckyDrawRegistrationId)).ToList();
                LuckyDraw.LuckyDrawNumberId = LuckyDrawNumberId;
                LuckyDraw.LuckyDrawNumber = new LuckyDrawNumber
                {
                    Id = LuckyDrawNumberId,
                    LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId,
                    LuckyDrawStructure = new LuckyDrawStructure
                    {
                        LuckyDrawId = LuckyDraw.Id,
                        Name = LuckyDrawNumber.LuckyDrawStructure.Name,
                        Value = LuckyDrawNumber.LuckyDrawStructure.Value,
                        Quantity = 1
                    }
                };
                var LuckyDrawWinnerCount = await UOW.LuckyDrawWinnerRepository.Count(new LuckyDrawWinnerFilter
                {
                    Take = int.MaxValue,
                    Skip = 0,
                    LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id }
                });
                LuckyDraw.RemainingLuckyDrawStructureCounter = LuckyDraw.LuckyDrawStructures.Sum(x => x.Quantity) - LuckyDrawWinnerCount;
                var Registration = await UOW.LuckyDrawRegistrationRepository.List(new LuckyDrawRegistrationFilter
                {
                    LuckyDrawId = new IdFilter { Equal = LuckyDraw.Id },
                    StoreId = new IdFilter { Equal = Store.Id },
                    Take = int.MaxValue,
                    Skip = 0,
                    IsDrawnByStore = true,
                    Selects = LuckyDrawRegistrationSelect.TurnCounter | LuckyDrawRegistrationSelect.RemainingTurnCounter
                });
                LuckyDraw.RemainingTurnCounter = Registration.Sum(x => x.RemainingTurnCounter);
                LuckyDraw.UsedTurnCounter = TotalCounter - LuckyDraw.RemainingTurnCounter;
                foreach (var Structure in LuckyDraw.LuckyDrawStructures)
                {
                    Structure.PrizeCounter = LuckyDraw.LuckyDrawWinners.Count(x => x.LuckyDrawStructureId == Structure.Id);
                }
                Logging.CreateAuditLog(LuckyDraw, new { }, nameof(LuckyDrawService));
                return LuckyDraw;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawService));
            }
            return null;
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
    }
}
