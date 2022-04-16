using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_ExportDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public List<StoreScouting_StoreScoutingDTO> StoreScoutings { get; set; }
    }
}
