using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.export_template.dto
{
    public class ExportTemplate_PrintDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public string StoreAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string sOrderDate { get; set; }
        public string sDeliveryDate { get; set; }
        public string SubTotalString { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string TotalString { get; set; }
        public string TotalText { get; set; }
        public string Note { get; set; }
        public ExportTemplate_StoreDTO BuyerStore { get; set; }
        public ExportTemplate_AppUserDTO SaleEmployee { get; set; }
        public List<ExportTemplate_PrintContentDTO> Contents { get; set; }
        public List<ExportTemplate_PrintPromotionDTO> Promotions { get; set; }
    }
}
