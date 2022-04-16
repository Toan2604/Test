using TrueSight.Common;
using DMS.Common;
using DMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.Repositories;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Handlers.Configuration;

namespace DMS.Services.MLuckyDrawWinner
{
    public interface ILuckyDrawWinnerService :  IServiceScoped
    {
        Task<int> Count(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<List<LuckyDrawWinner>> List(LuckyDrawWinnerFilter LuckyDrawWinnerFilter);
        Task<LuckyDrawWinner> Get(long Id);
        Task<LuckyDrawWinner> Draw(LuckyDrawWinner LuckyDrawWinner);
        Task<LuckyDrawWinner> Create(LuckyDrawWinner LuckyDrawWinner);
        Task<LuckyDrawWinner> Update(LuckyDrawWinner LuckyDrawWinner);
        Task<LuckyDrawWinner> Delete(LuckyDrawWinner LuckyDrawWinner);
        Task<List<LuckyDrawWinner>> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners);
        Task<List<LuckyDrawWinner>> Import(List<LuckyDrawWinner> LuckyDrawWinners);
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

        public async Task<LuckyDrawWinner> Create(LuckyDrawWinner LuckyDrawWinner)
        {
            if (!await LuckyDrawWinnerValidator.Create(LuckyDrawWinner))
                return LuckyDrawWinner;

            try
            {
                await UOW.LuckyDrawWinnerRepository.Create(LuckyDrawWinner);
                LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);
                Logging.CreateAuditLog(LuckyDrawWinner, new { }, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinner;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }

        public async Task<LuckyDrawWinner> Update(LuckyDrawWinner LuckyDrawWinner)
        {
            if (!await LuckyDrawWinnerValidator.Update(LuckyDrawWinner))
                return LuckyDrawWinner;
            try
            {
                var oldData = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);

                await UOW.LuckyDrawWinnerRepository.Update(LuckyDrawWinner);

                LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);
                Logging.CreateAuditLog(LuckyDrawWinner, oldData, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinner;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }

        public async Task<LuckyDrawWinner> Delete(LuckyDrawWinner LuckyDrawWinner)
        {
            if (!await LuckyDrawWinnerValidator.Delete(LuckyDrawWinner))
                return LuckyDrawWinner;

            try
            {
                await UOW.LuckyDrawWinnerRepository.Delete(LuckyDrawWinner);
                Logging.CreateAuditLog(new { }, LuckyDrawWinner, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinner;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }

        public async Task<List<LuckyDrawWinner>> BulkDelete(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            if (!await LuckyDrawWinnerValidator.BulkDelete(LuckyDrawWinners))
                return LuckyDrawWinners;

            try
            {
                await UOW.LuckyDrawWinnerRepository.BulkDelete(LuckyDrawWinners);
                Logging.CreateAuditLog(new { }, LuckyDrawWinners, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinners;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;

        }

        public async Task<List<LuckyDrawWinner>> Import(List<LuckyDrawWinner> LuckyDrawWinners)
        {
            if (!await LuckyDrawWinnerValidator.Import(LuckyDrawWinners))
                return LuckyDrawWinners;
            try
            {
                await UOW.LuckyDrawWinnerRepository.BulkMerge(LuckyDrawWinners);

                Logging.CreateAuditLog(LuckyDrawWinners, new { }, nameof(LuckyDrawWinnerService));
                return LuckyDrawWinners;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(LuckyDrawWinnerService));
            }
            return null;
        }
        public async Task<LuckyDrawWinner> Draw(LuckyDrawWinner LuckyDrawWinner)
        {
            try
            {
                LuckyDrawWinner = await UOW.LuckyDrawWinnerRepository.Get(LuckyDrawWinner.Id);
                //nếu đã quay thì return giải
                if (LuckyDrawWinner.LuckyDrawNumberId.HasValue) return LuckyDrawWinner;
                var CurrentUser = await UOW.AppUserRepository.Get(CurrentContext.UserId);
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
                if (LuckyDrawWinner.LuckyDrawNumberId != null)
                    await UOW.LuckyDrawNumberRepository.Used(new List<long> { LuckyDrawWinner.LuckyDrawNumberId.Value });
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

        private async Task CreateLuckyDrawNumber(LuckyDraw LuckyDraw)
        {
            var LuckyDrawStructureIds = LuckyDraw.LuckyDrawStructures.Select(x => x.Id).ToList();
            List<LuckyDrawNumber> LuckyDrawNumbers = await UOW.LuckyDrawNumberRepository.List(new LuckyDrawNumberFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = LuckyDrawNumberSelect.ALL,
                OrderBy = LuckyDrawNumberOrder.Id,
                OrderType = OrderType.ASC,
                LuckyDrawStructureId = new IdFilter { In = LuckyDrawStructureIds }
            });
            foreach (LuckyDrawStructure LuckyDrawStructure in LuckyDraw.LuckyDrawStructures)
            {
                var LuckyDrawNumberCheckList = new List<LuckyDrawNumber>();
                var LuckyDrawNumbersInStructure = LuckyDrawNumbers.Where(x => x.LuckyDrawStructureId == LuckyDrawStructure.Id).ToList();
                if (LuckyDrawNumbersInStructure.Count > LuckyDrawStructure.Quantity) //nếu số lượng giải thưởng giảm
                {
                    var NotUsedLuckyDrawNumbers = LuckyDrawNumbersInStructure.Where(x => x.Used == false).ToList();
                    int count = LuckyDrawNumbersInStructure.Count - (int)LuckyDrawStructure.Quantity;
                    int start = NotUsedLuckyDrawNumbers.Count - count;
                    LuckyDrawNumberCheckList = NotUsedLuckyDrawNumbers.GetRange(start, count);
                    await UOW.LuckyDrawNumberRepository.BulkDelete(LuckyDrawNumberCheckList);
                }
                else if (LuckyDrawNumbersInStructure.Count < LuckyDrawStructure.Quantity) //nếu số lượng giải thưởng tăng (gồm cả tạo mới)
                {
                    List<long> LuckyDrawNumberIds = new List<long>();
                    LuckyDrawNumberIds.AddRange(GenerateLuckyDrawNumberId(1, LuckyDrawStructure.Quantity, LuckyDrawStructure.Id));
                    if (LuckyDrawNumbersInStructure.Count > 0) //nếu update số lượng giải
                    {
                        var LuckyDrawNumberIdsInStructure = LuckyDrawNumbersInStructure.Select(x => x.Id).ToList();
                        int count = (int)LuckyDrawStructure.Quantity - LuckyDrawNumbersInStructure.Count;
                        LuckyDrawNumberIds = LuckyDrawNumberIds.Except(LuckyDrawNumberIdsInStructure).ToList().GetRange(0, count);
                    }
                    LuckyDrawNumberCheckList = LuckyDrawNumberIds.Select(x => new LuckyDrawNumber
                    {
                        Id = x,
                        LuckyDrawStructureId = LuckyDrawStructure.Id,
                        Used = false
                    }).ToList();
                    await UOW.LuckyDrawNumberRepository.BulkMerge(LuckyDrawNumberCheckList);
                }
            }
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
    }
}
