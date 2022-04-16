using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_StoreScoutingTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public StoreScouting_StoreScoutingTypeDTO() { }
        public StoreScouting_StoreScoutingTypeDTO(StoreScoutingType StoreScoutingType)
        {

            this.Id = StoreScoutingType.Id;

            this.Code = StoreScoutingType.Code;

            this.Name = StoreScoutingType.Name;

            this.Errors = StoreScoutingType.Errors;
        }
    }

    public class StoreScouting_StoreScoutingTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StoreScoutingTypeOrder OrderBy { get; set; }
    }
}