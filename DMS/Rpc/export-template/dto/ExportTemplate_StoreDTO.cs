using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_StoreDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }
        public string CodeDraft { get; set; }

        public string Name { get; set; }

        public string Telephone { get; set; }
        public string Address { get; set; }

        public string DeliveryAddress { get; set; }
        public ExportTemplate_StoreDTO ParentStore { get; set; }
        public ExportTemplate_StoreGroupingDTO StoreGrouping { get; set; }
        public ExportTemplate_StoreTypeDTO StoreType { get; set; }
        public ExportTemplate_StoreStatusDTO StoreStatus { get; set; }
    }
}