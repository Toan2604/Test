using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_ProductProductGroupingMappingDTO : DataDTO
    {
        public long ProductId { get; set; }
        public long ProductGroupingId { get; set; }
        public ExportTemplate_ProductGroupingDTO ProductGrouping { get; set; }
    }
}
