using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }

    }
}
