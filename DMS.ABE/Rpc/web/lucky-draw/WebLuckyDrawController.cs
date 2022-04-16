using DMS.ABE.Common;
using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Services.MLuckyDraw;
using DMS.ABE.Services.MCategory;
using DMS.ABE.Services.MProduct;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Helpers;
using DMS.ABE.Services.MLuckyDrawWinner;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDrawController : SimpleController
    {
        private ILuckyDrawService LuckyDrawService;
        private ILuckyDrawWinnerService LuckyDrawWinnerService;
        public WebLuckyDrawController(
            ILuckyDrawService LuckyDrawService,
            ILuckyDrawWinnerService LuckyDrawWinnerService
            )
        {
            this.LuckyDrawService = LuckyDrawService;
            this.LuckyDrawWinnerService = LuckyDrawWinnerService;
        }

        [Route(WebLuckyDrawRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WebLuckyDraw_LuckyDrawFilterDTO WebLuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(WebLuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.InNotAction = false;
            int count = await LuckyDrawService.Count(LuckyDrawFilter);
            return count;
        }
        [Route(WebLuckyDrawRoute.List), HttpPost]
        public async Task<ActionResult<List<WebLuckyDraw_LuckyDrawDTO>>> List([FromBody] WebLuckyDraw_LuckyDrawFilterDTO WebWebLuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(WebWebLuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter = await LuckyDrawService.ToFilter(LuckyDrawFilter);
            LuckyDrawFilter.StartAt = new DateFilter { LessEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.EndAt = new DateFilter { GreaterEqual = StaticParams.DateTimeNow };
            LuckyDrawFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            LuckyDrawFilter.InNotAction = false;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.List(LuckyDrawFilter);
            List<WebLuckyDraw_LuckyDrawDTO> WebLuckyDraw_LuckyDrawDTOs = LuckyDraws
                .Select(c => new WebLuckyDraw_LuckyDrawDTO(c)).ToList();
            return WebLuckyDraw_LuckyDrawDTOs;
        }
        [Route(WebLuckyDrawRoute.Get), HttpPost]
        public async Task<ActionResult<WebLuckyDraw_LuckyDrawDTO>> Get([FromBody] WebLuckyDraw_LuckyDrawDTO WebLuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(WebLuckyDraw_LuckyDrawDTO.Id);
            return new WebLuckyDraw_LuckyDrawDTO(LuckyDraw);
        }
        [Route(WebLuckyDrawRoute.GetDrawHistory), HttpPost]
        public async Task<ActionResult<WebLuckyDraw_LuckyDrawDTO>> GetDrawHistory([FromBody] WebLuckyDraw_LuckyDrawDTO WebLuckyDraw_LuckyDrawDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDraw LuckyDraw = await LuckyDrawService.Get(WebLuckyDraw_LuckyDrawDTO.Id);
            return new WebLuckyDraw_LuckyDrawDTO(LuckyDraw);
        }
        [Route(WebLuckyDrawRoute.ListLuckyDrawHistory), HttpPost]
        public async Task<ActionResult<List<WebLuckyDraw_LuckyDrawDTO>>> ListLuckyDrawHistory([FromBody] WebLuckyDraw_LuckyDrawFilterDTO WebLuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(WebLuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter.InNotAction = true;
            LuckyDrawFilter.HasPrize = true;
            List<LuckyDraw> LuckyDraws = await LuckyDrawService.ListHistory(LuckyDrawFilter);
            List<WebLuckyDraw_LuckyDrawDTO> WebLuckyDraw_LuckyDrawDTOs = LuckyDraws.Select(x => new WebLuckyDraw_LuckyDrawDTO(x)).ToList();
            return WebLuckyDraw_LuckyDrawDTOs;
        }
        [Route(WebLuckyDrawRoute.CountLuckyDrawHistory), HttpPost]
        public async Task<ActionResult<int>> CountLuckyDrawHistory([FromBody] WebLuckyDraw_LuckyDrawFilterDTO WebLuckyDraw_LuckyDrawFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            LuckyDrawFilter LuckyDrawFilter = ConvertFilterDTOToFilterEntity(WebLuckyDraw_LuckyDrawFilterDTO);
            LuckyDrawFilter.HasPrize = true;
            LuckyDrawFilter.InNotAction = true;
            int count = await LuckyDrawService.CountHistory(LuckyDrawFilter);
            return count;
        }
        //[Route(WebLuckyDrawRoute.Draw), HttpPost]
        //public async Task<ActionResult<WebLuckyDraw_LuckyDrawDTO>> Draw([FromBody] WebLuckyDraw_LuckyDrawDTO WebLuckyDraw_LuckyDrawDTO)
        //{
        //    if (!ModelState.IsValid)
        //        throw new BindException(ModelState);

        //    LuckyDraw LuckyDraw = ConvertDTOToEntity(WebLuckyDraw_LuckyDrawDTO);
        //    LuckyDraw = await LuckyDrawService.Draw(LuckyDraw);
        //    return new WebLuckyDraw_LuckyDrawDTO(LuckyDraw);
        //}


        [Route(WebLuckyDrawRoute.Draw), HttpPost]
        public async Task<WebLuckyDraw_LuckyDrawWinnerDTO> Draw([FromBody] WebLuckyDraw_LuckyDrawWinnerDTO LuckyDraw_LuckyDrawWinnerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            LuckyDrawWinner LuckyDrawWinner = new LuckyDrawWinner();
            LuckyDrawWinner.Id = LuckyDraw_LuckyDrawWinnerDTO.Id;
            LuckyDrawWinner = await LuckyDrawWinnerService.Draw(LuckyDrawWinner);
            return new WebLuckyDraw_LuckyDrawWinnerDTO(LuckyDrawWinner);
        }
        private LuckyDraw ConvertDTOToEntity(WebLuckyDraw_LuckyDrawDTO WebLuckyDraw_LuckyDrawDTO)
        {
            WebLuckyDraw_LuckyDrawDTO.TrimString();
            LuckyDraw LuckyDraw = new LuckyDraw();
            LuckyDraw.Id = WebLuckyDraw_LuckyDrawDTO.Id;
            LuckyDraw.Code = WebLuckyDraw_LuckyDrawDTO.Code;
            LuckyDraw.Name = WebLuckyDraw_LuckyDrawDTO.Name;
            LuckyDraw.LuckyDrawTypeId = WebLuckyDraw_LuckyDrawDTO.LuckyDrawTypeId;
            LuckyDraw.OrganizationId = WebLuckyDraw_LuckyDrawDTO.OrganizationId;
            LuckyDraw.RevenuePerTurn = WebLuckyDraw_LuckyDrawDTO.RevenuePerTurn;
            LuckyDraw.StartAt = WebLuckyDraw_LuckyDrawDTO.StartAt;
            LuckyDraw.EndAt = WebLuckyDraw_LuckyDrawDTO.EndAt;
            LuckyDraw.ImageId = WebLuckyDraw_LuckyDrawDTO.ImageId;
            LuckyDraw.AvatarImageId = WebLuckyDraw_LuckyDrawDTO.AvatarImageId;
            LuckyDraw.Description = WebLuckyDraw_LuckyDrawDTO.Description;
            LuckyDraw.StatusId = WebLuckyDraw_LuckyDrawDTO.StatusId;
            LuckyDraw.Used = WebLuckyDraw_LuckyDrawDTO.Used;
            LuckyDraw.Image = WebLuckyDraw_LuckyDrawDTO.Image == null ? null : new Image
            {
                Id = WebLuckyDraw_LuckyDrawDTO.Image.Id,
                Name = WebLuckyDraw_LuckyDrawDTO.Image.Name,
                Url = WebLuckyDraw_LuckyDrawDTO.Image.Url,
                ThumbnailUrl = WebLuckyDraw_LuckyDrawDTO.Image.ThumbnailUrl,
            };
            LuckyDraw.AvatarImage = WebLuckyDraw_LuckyDrawDTO.AvatarImage == null ? null : new Image
            {
                Id = WebLuckyDraw_LuckyDrawDTO.AvatarImage.Id,
                Name = WebLuckyDraw_LuckyDrawDTO.AvatarImage.Name,
                Url = WebLuckyDraw_LuckyDrawDTO.AvatarImage.Url,
                ThumbnailUrl = WebLuckyDraw_LuckyDrawDTO.AvatarImage.ThumbnailUrl,
            };
            LuckyDraw.LuckyDrawType = WebLuckyDraw_LuckyDrawDTO.LuckyDrawType == null ? null : new LuckyDrawType
            {
                Id = WebLuckyDraw_LuckyDrawDTO.LuckyDrawType.Id,
                Code = WebLuckyDraw_LuckyDrawDTO.LuckyDrawType.Code,
                Name = WebLuckyDraw_LuckyDrawDTO.LuckyDrawType.Name,
            };
            LuckyDraw.Status = WebLuckyDraw_LuckyDrawDTO.Status == null ? null : new Status
            {
                Id = WebLuckyDraw_LuckyDrawDTO.Status.Id,
                Code = WebLuckyDraw_LuckyDrawDTO.Status.Code,
                Name = WebLuckyDraw_LuckyDrawDTO.Status.Name,
            };
            LuckyDraw.LuckyDrawStoreGroupingMappings = WebLuckyDraw_LuckyDrawDTO.LuckyDrawStoreGroupingMappings?
                .Select(x => new LuckyDrawStoreGroupingMapping
                {
                    StoreGroupingId = x.StoreGroupingId,
                }).ToList();
            LuckyDraw.LuckyDrawStoreMappings = WebLuckyDraw_LuckyDrawDTO.LuckyDrawStoreMappings?
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
            LuckyDraw.LuckyDrawStoreTypeMappings = WebLuckyDraw_LuckyDrawDTO.LuckyDrawStoreTypeMappings?
                .Select(x => new LuckyDrawStoreTypeMapping
                {
                    StoreTypeId = x.StoreTypeId
                }).ToList();
            LuckyDraw.LuckyDrawStructures = WebLuckyDraw_LuckyDrawDTO.LuckyDrawStructures?
                .Select(x => new LuckyDrawStructure
                {
                    Id = x.Id,
                    Name = x.Name,
                    Value = x.Value,
                    Quantity = x.Quantity,
                }).ToList();
            LuckyDraw.LuckyDrawWinners = WebLuckyDraw_LuckyDrawDTO.LuckyDrawWinners?
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

        private LuckyDrawFilter ConvertFilterDTOToFilterEntity(WebLuckyDraw_LuckyDrawFilterDTO WebLuckyDraw_LuckyDrawFilterDTO)
        {
            LuckyDrawFilter LuckyDrawFilter = new LuckyDrawFilter();
            LuckyDrawFilter.Selects = LuckyDrawSelect.ALL;
            LuckyDrawFilter.Skip = WebLuckyDraw_LuckyDrawFilterDTO.Skip;
            LuckyDrawFilter.Take = WebLuckyDraw_LuckyDrawFilterDTO.Take;
            LuckyDrawFilter.OrderBy = WebLuckyDraw_LuckyDrawFilterDTO.OrderBy;
            LuckyDrawFilter.OrderType = WebLuckyDraw_LuckyDrawFilterDTO.OrderType;

            LuckyDrawFilter.Id = WebLuckyDraw_LuckyDrawFilterDTO.Id;
            LuckyDrawFilter.Code = WebLuckyDraw_LuckyDrawFilterDTO.Code;
            LuckyDrawFilter.Name = WebLuckyDraw_LuckyDrawFilterDTO.Name;
            LuckyDrawFilter.LuckyDrawTypeId = WebLuckyDraw_LuckyDrawFilterDTO.LuckyDrawTypeId;
            LuckyDrawFilter.OrganizationId = WebLuckyDraw_LuckyDrawFilterDTO.OrganizationId;
            LuckyDrawFilter.RevenuePerTurn = WebLuckyDraw_LuckyDrawFilterDTO.RevenuePerTurn;
            LuckyDrawFilter.StartAt = WebLuckyDraw_LuckyDrawFilterDTO.StartAt;
            LuckyDrawFilter.EndAt = WebLuckyDraw_LuckyDrawFilterDTO.EndAt;
            LuckyDrawFilter.ImageId = WebLuckyDraw_LuckyDrawFilterDTO.ImageId;
            LuckyDrawFilter.AvatarImageId = WebLuckyDraw_LuckyDrawFilterDTO.AvatarImageId;
            LuckyDrawFilter.StatusId = WebLuckyDraw_LuckyDrawFilterDTO.StatusId;
            LuckyDrawFilter.CreatedAt = WebLuckyDraw_LuckyDrawFilterDTO.CreatedAt;
            LuckyDrawFilter.UpdatedAt = WebLuckyDraw_LuckyDrawFilterDTO.UpdatedAt;
            return LuckyDrawFilter;
        }
    }
}
