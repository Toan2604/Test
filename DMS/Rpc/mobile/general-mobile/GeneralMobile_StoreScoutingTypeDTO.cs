using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreScoutingTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public GeneralMobile_StoreScoutingTypeDTO() { }
        public GeneralMobile_StoreScoutingTypeDTO(StoreScoutingType StoreScoutingType)
        {
            this.Id = StoreScoutingType.Id;
            this.Code = StoreScoutingType.Code;
            this.Name = StoreScoutingType.Name;
        }
    }
}
