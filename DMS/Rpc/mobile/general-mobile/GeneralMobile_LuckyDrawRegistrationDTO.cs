using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawRegistrationDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public decimal Revenue { get; set; }
        public long TurnCounter { get; set; }
        public long UsedTurnCounter { get; set; }
        public long RemainingTurnCounter { get; set; }
        public DateTime Time { get; set; }
        public GeneralMobile_AppUserDTO AppUser { get; set; }
        public GeneralMobile_LuckyDrawDTO LuckyDraw { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }
        public List<GeneralMobile_LuckyDrawWinnerDTO> LuckyDrawWinners { get; set; }
        public GeneralMobile_LuckyDrawRegistrationDTO() {}
        public GeneralMobile_LuckyDrawRegistrationDTO(LuckyDrawRegistration LuckyDrawRegistration)
        {
            this.Id = LuckyDrawRegistration.Id;
            this.LuckyDrawId = LuckyDrawRegistration.LuckyDrawId;
            this.AppUserId = LuckyDrawRegistration.AppUserId;
            this.StoreId = LuckyDrawRegistration.StoreId;
            this.Revenue = LuckyDrawRegistration.Revenue;
            this.TurnCounter = LuckyDrawRegistration.TurnCounter;
            this.UsedTurnCounter = LuckyDrawRegistration.UsedTurnCounter;
            this.RemainingTurnCounter = LuckyDrawRegistration.RemainingTurnCounter;
            this.Time = LuckyDrawRegistration.Time;
            this.AppUser = LuckyDrawRegistration.AppUser == null ? null : new GeneralMobile_AppUserDTO(LuckyDrawRegistration.AppUser);
            this.LuckyDraw = LuckyDrawRegistration.LuckyDraw == null ? null : new GeneralMobile_LuckyDrawDTO(LuckyDrawRegistration.LuckyDraw);
            this.Store = LuckyDrawRegistration.Store == null ? null : new GeneralMobile_StoreDTO(LuckyDrawRegistration.Store);
            this.LuckyDrawWinners = LuckyDrawRegistration.LuckyDrawWinners == null ? null : LuckyDrawRegistration.LuckyDrawWinners.Select(x => new GeneralMobile_LuckyDrawWinnerDTO(x)).ToList();
            this.Informations = LuckyDrawRegistration.Informations;
            this.Warnings = LuckyDrawRegistration.Warnings;
            this.Errors = LuckyDrawRegistration.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawRegistrationFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public DecimalFilter Revenue { get; set; }
        public LongFilter TurnCounter { get; set; }
        public DateFilter Time { get; set; }
        public string Search { get; set; }
        public LuckyDrawRegistrationOrder OrderBy { get; set; }
    }
}
