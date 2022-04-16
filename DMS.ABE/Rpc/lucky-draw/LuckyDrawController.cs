using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Services.MLuckyDraw;
using DMS.ABE.Services.MLuckyDrawWinner;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Rpc.lucky_draw
{
    public class LuckyDrawController : SimpleController
    {
        private ILuckyDrawService LuckyDrawService;
        private ILuckyDrawWinnerService LuckyDrawWinnerService;
        public LuckyDrawController(
            ILuckyDrawService LuckyDrawService,
            ILuckyDrawWinnerService LuckyDrawWinnerService
            )
        {
            this.LuckyDrawService = LuckyDrawService;
            this.LuckyDrawWinnerService = LuckyDrawWinnerService;
        }

        [Route(LuckyDrawRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.InNotAction = false;
            int count = await LuckyDrawService.Count(LuckyDrawFilter);
            return count;
        }

        [Route(LuckyDrawRoute.List), HttpPost]
        public async Task<ActionResult<List<LuckyDraw_LuckyDrawDTO>>> List([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.InNotAction = false;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.List(LuckyDrawFilter);
            List<LuckyDraw_LuckyDrawDTO> LuckyDraw_LuckyDrawDTOs = LuckyDraws
                .Select(c => new LuckyDraw_LuckyDrawDTO(c)).ToList();
            return LuckyDraw_LuckyDrawDTOs;
        }
        [Route(LuckyDrawRoute.Get), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Get([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(LuckyDraw_LuckyDrawDTO.Id);
            return new LuckyDraw_LuckyDrawDTO(LuckyDraw);
        }
        //[Route(LuckyDrawRoute.Draw), HttpPost]
        //public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> Draw([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    LuckyDraw LuckyDraw = ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO);
        //    LuckyDraw = await LuckyDrawService.Draw(LuckyDraw);
        //    return new LuckyDraw_LuckyDrawDTO(LuckyDraw);
        //}

        [Route(LuckyDrawRoute.Draw), HttpPost]
        public async Task<LuckyDraw_LuckyDrawWinnerDTO> Draw([FromBody] LuckyDraw_LuckyDrawWinnerDTO LuckyDraw_LuckyDrawWinnerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawWinner LuckyDrawWinner = new LuckyDrawWinner();
            LuckyDrawWinner.Id = LuckyDraw_LuckyDrawWinnerDTO.Id;
            LuckyDrawWinner = await LuckyDrawWinnerService.Draw(LuckyDrawWinner);
            return new LuckyDraw_LuckyDrawWinnerDTO(LuckyDrawWinner);
        }


        [Route(LuckyDrawRoute.GetDrawHistory), HttpPost]
        public async Task<ActionResult<LuckyDraw_LuckyDrawDTO>> GetDrawHistory([FromBody] LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(LuckyDraw_LuckyDrawDTO.Id);
            return new LuckyDraw_LuckyDrawDTO(LuckyDraw);
        }
        [Route(LuckyDrawRoute.ListLuckyDrawHistory), HttpPost]
        public async Task<ActionResult<List<LuckyDraw_LuckyDrawDTO>>> ListLuckyDrawHistory([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter.HasPrize = true;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.ListHistory(LuckyDrawFilter);
            List<LuckyDraw_LuckyDrawDTO> LuckyDraw_LuckyDrawDTOs = LuckyDraws.Select(x => new LuckyDraw_LuckyDrawDTO(x)).ToList();
            return LuckyDraw_LuckyDrawDTOs;
        }
        [Route(LuckyDrawRoute.CountLuckyDrawHistory), HttpPost]
        public async Task<ActionResult<int>> CountLuckyDrawHistory([FromBody] LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter.HasPrize = true;
            int count = await LuckyDrawService.CountHistory(LuckyDrawFilter);
            return count;
        }
        private LuckyDraw ConvertDTOToEntity(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            LuckyDraw_LuckyDrawDTO.TrimString();
            LuckyDraw LuckyDraw = new LuckyDraw();
            LuckyDraw.Id = LuckyDraw_LuckyDrawDTO.Id;
            LuckyDraw.Code = LuckyDraw_LuckyDrawDTO.Code;
            LuckyDraw.Name = LuckyDraw_LuckyDrawDTO.Name;
            LuckyDraw.LuckyDrawTypeId = LuckyDraw_LuckyDrawDTO.LuckyDrawTypeId;
            LuckyDraw.OrganizationId = LuckyDraw_LuckyDrawDTO.OrganizationId;
            LuckyDraw.RevenuePerTurn = LuckyDraw_LuckyDrawDTO.RevenuePerTurn;
            LuckyDraw.StartAt = LuckyDraw_LuckyDrawDTO.StartAt;
            LuckyDraw.EndAt = LuckyDraw_LuckyDrawDTO.EndAt;
            LuckyDraw.ImageId = LuckyDraw_LuckyDrawDTO.ImageId;
            LuckyDraw.AvatarImageId = LuckyDraw_LuckyDrawDTO.AvatarImageId;
            LuckyDraw.Description = LuckyDraw_LuckyDrawDTO.Description;
            LuckyDraw.StatusId = LuckyDraw_LuckyDrawDTO.StatusId;
            LuckyDraw.Used = LuckyDraw_LuckyDrawDTO.Used;
            LuckyDraw.Image = LuckyDraw_LuckyDrawDTO.Image == null ? null : new Image
            {
                Id = LuckyDraw_LuckyDrawDTO.Image.Id,
                Name = LuckyDraw_LuckyDrawDTO.Image.Name,
                Url = LuckyDraw_LuckyDrawDTO.Image.Url,
                ThumbnailUrl = LuckyDraw_LuckyDrawDTO.Image.ThumbnailUrl,
            };
            LuckyDraw.AvatarImage = LuckyDraw_LuckyDrawDTO.AvatarImage == null ? null : new Image
            {
                Id = LuckyDraw_LuckyDrawDTO.AvatarImage.Id,
                Name = LuckyDraw_LuckyDrawDTO.AvatarImage.Name,
                Url = LuckyDraw_LuckyDrawDTO.AvatarImage.Url,
                ThumbnailUrl = LuckyDraw_LuckyDrawDTO.AvatarImage.ThumbnailUrl,
            };
            LuckyDraw.LuckyDrawType = LuckyDraw_LuckyDrawDTO.LuckyDrawType == null ? null : new LuckyDrawType
            {
                Id = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Id,
                Code = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Code,
                Name = LuckyDraw_LuckyDrawDTO.LuckyDrawType.Name,
            };
            LuckyDraw.Status = LuckyDraw_LuckyDrawDTO.Status == null ? null : new Status
            {
                Id = LuckyDraw_LuckyDrawDTO.Status.Id,
                Code = LuckyDraw_LuckyDrawDTO.Status.Code,
                Name = LuckyDraw_LuckyDrawDTO.Status.Name,
            };
            LuckyDraw.LuckyDrawStoreGroupingMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreGroupingMappings?
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                }).ToList();
            LuckyDraw.LuckyDrawStoreMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreMappings?
                .Select(x => new LuckyDrawStoreMapping
                {
                    StoreId = x.StoreId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        UnsignName = x.Store.UnsignName,
                        ParentStoreId = x.Store.ParentStoreId,
                        OrganizationId = x.Store.OrganizationId,
                        StoreTypeId = x.Store.StoreTypeId,
                        Telephone = x.Store.Telephone,
                        ProvinceId = x.Store.ProvinceId,
                        DistrictId = x.Store.DistrictId,
                        WardId = x.Store.WardId,
                        Address = x.Store.Address,
                        UnsignAddress = x.Store.UnsignAddress,
                        DeliveryAddress = x.Store.DeliveryAddress,
                        Latitude = x.Store.Latitude,
                        Longitude = x.Store.Longitude,
                        DeliveryLatitude = x.Store.DeliveryLatitude,
                        DeliveryLongitude = x.Store.DeliveryLongitude,
                        OwnerName = x.Store.OwnerName,
                        OwnerPhone = x.Store.OwnerPhone,
                        OwnerEmail = x.Store.OwnerEmail,
                        TaxCode = x.Store.TaxCode,
                        LegalEntity = x.Store.LegalEntity,
                        CreatorId = x.Store.CreatorId,
                        AppUserId = x.Store.AppUserId,
                        StatusId = x.Store.StatusId,
                        Used = x.Store.Used,
                        StoreScoutingId = x.Store.StoreScoutingId,
                        StoreStatusId = x.Store.StoreStatusId,
                    },
                }).ToList();
            LuckyDraw.LuckyDrawStoreTypeMappings = LuckyDraw_LuckyDrawDTO.LuckyDrawStoreTypeMappings?
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    StoreTypeId = x.StoreTypeId
                }).ToList();
            LuckyDraw.LuckyDrawStructures = LuckyDraw_LuckyDrawDTO.LuckyDrawStructures?
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToList();
            LuckyDraw.LuckyDrawWinners = LuckyDraw_LuckyDrawDTO.LuckyDrawWinners?
                .Select(x => new LuckyDrawWinner
                {
                    Id = x.Id,
                    LuckyDrawStructureId = x.LuckyDrawStructureId,
                    LuckyDrawRegistrationId = x.LuckyDrawRegistrationId,
                    Time = x.Time,
                    LuckyDrawRegistration = x.LuckyDrawRegistration == null ? null : new LuckyDrawRegistration
                    {
                        Id = x.LuckyDrawRegistration.Id,
                        LuckyDrawId = x.LuckyDrawRegistration.LuckyDrawId,
                        AppUserId = x.LuckyDrawRegistration.AppUserId,
                        StoreId = x.LuckyDrawRegistration.StoreId,
                        Revenue = x.LuckyDrawRegistration.Revenue,
                        TurnCounter = x.LuckyDrawRegistration.TurnCounter,
                        RemainingTurnCounter = x.LuckyDrawRegistration.RemainingTurnCounter,
                        IsDrawnByStore = x.LuckyDrawRegistration.IsDrawnByStore,
                        Time = x.LuckyDrawRegistration.Time,
                    },
                    LuckyDrawStructure = x.LuckyDrawStructure == null ? null : new LuckyDrawStructure
                    {
                        Id = x.LuckyDrawStructure.Id,
                        LuckyDrawId = x.LuckyDrawStructure.LuckyDrawId,
                        Name = x.LuckyDrawStructure.Name,
                        Value = x.LuckyDrawStructure.Value,
                        Quantity = x.LuckyDrawStructure.Quantity,
                    },
                }).ToList();
            return LuckyDraw;
        }

        private LuckyDrawFilter ConvertFilterDTOToFilterEntity(LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter.Selects = LuckyDrawSelect.ALL;
            LuckyDrawFilter.Skip = LuckyDraw_LuckyDrawFilterDTO.Skip;
            LuckyDrawFilter.Take = LuckyDraw_LuckyDrawFilterDTO.Take;
            LuckyDrawFilter.OrderBy = LuckyDraw_LuckyDrawFilterDTO.OrderBy;
            LuckyDrawFilter.OrderType = LuckyDraw_LuckyDrawFilterDTO.OrderType;

            LuckyDrawFilter.Id = LuckyDraw_LuckyDrawFilterDTO.Id;
            LuckyDrawFilter.Code = LuckyDraw_LuckyDrawFilterDTO.Code;
            LuckyDrawFilter.Name = LuckyDraw_LuckyDrawFilterDTO.Name;
            LuckyDrawFilter.LuckyDrawTypeId = LuckyDraw_LuckyDrawFilterDTO.LuckyDrawTypeId;
            LuckyDrawFilter.OrganizationId = LuckyDraw_LuckyDrawFilterDTO.OrganizationId;
            LuckyDrawFilter.RevenuePerTurn = LuckyDraw_LuckyDrawFilterDTO.RevenuePerTurn;
            LuckyDrawFilter.StartAt = LuckyDraw_LuckyDrawFilterDTO.StartAt;
            LuckyDrawFilter.EndAt = LuckyDraw_LuckyDrawFilterDTO.EndAt;
            LuckyDrawFilter.ImageId = LuckyDraw_LuckyDrawFilterDTO.ImageId;
            LuckyDrawFilter.AvatarImageId = LuckyDraw_LuckyDrawFilterDTO.AvatarImageId;
            LuckyDrawFilter.StatusId = LuckyDraw_LuckyDrawFilterDTO.StatusId;
            LuckyDrawFilter.CreatedAt = LuckyDraw_LuckyDrawFilterDTO.CreatedAt;
            LuckyDrawFilter.UpdatedAt = LuckyDraw_LuckyDrawFilterDTO.UpdatedAt;
            return LuckyDrawFilter;
        }
    }
}
