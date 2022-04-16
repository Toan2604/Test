using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_StoreScoutingStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public StoreScouting_StoreScoutingStatusDTO() { }
        public StoreScouting_StoreScoutingStatusDTO(StoreScoutingStatus StoreScoutingStatus)
        {

            this.Id = StoreScoutingStatus.Id;

            this.Code = StoreScoutingStatus.Code;

            this.Name = StoreScoutingStatus.Name;

            this.Errors = StoreScoutingStatus.Errors;
        }
    }

    public class StoreScouting_StoreScoutingStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StoreScoutingStatusOrder OrderBy { get; set; }
    }
}