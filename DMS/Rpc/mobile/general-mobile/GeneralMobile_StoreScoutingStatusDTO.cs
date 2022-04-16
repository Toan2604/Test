using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreScoutingStatusDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public GeneralMobile_StoreScoutingStatusDTO() { }
        public GeneralMobile_StoreScoutingStatusDTO(StoreScoutingStatus StoreScoutingStatus)
        {

            this.Id = StoreScoutingStatus.Id;

            this.Code = StoreScoutingStatus.Code;

            this.Name = StoreScoutingStatus.Name;

            this.Errors = StoreScoutingStatus.Errors;
        }
    }

    public class GeneralMobile_StoreScoutingStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StoreScoutingStatusOrder OrderBy { get; set; }
    }
}
