using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_ItemDTO : DataDTO
    {

        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ScanCode { get; set; }
        public string ERPCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal SaleStock { get; set; }
        public long StatusId { get; set; }
        public MobileIndirectSalesOrder_ProductDTO Product { get; set; }
        public List<MobileIndirectSalesOrder_ItemImageMappingDTO> ItemImageMappings { get; set; }
        public MobileIndirectSalesOrder_ItemDTO() { }
        public MobileIndirectSalesOrder_ItemDTO(Item Item)
        {
            this.Id = Item.Id;
            this.ProductId = Item.ProductId;
            this.Code = Item.Code;
            this.Name = Item.Name;
            this.ScanCode = Item.ScanCode;
            this.ERPCode = Item.ERPCode;
            this.SalePrice = Item.SalePrice;
            this.RetailPrice = Item.RetailPrice;
            this.SaleStock = Item.SaleStock;
            this.StatusId = Item.StatusId;
            this.Product = Item.Product == null ? null : new MobileIndirectSalesOrder_ProductDTO(Item.Product);
            this.ItemImageMappings = Item.ItemImageMappings?.Select(x => new MobileIndirectSalesOrder_ItemImageMappingDTO(x)).ToList();
            this.Errors = Item.Errors;
        }
    }

    public class MobileIndirectSalesOrder_ItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProductId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter ScanCode { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter SupplierId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter SalesEmployeeId { get; set; }
        public string Search { get; set; }
        public bool? IsNew { get; set; }
        public ItemOrder OrderBy { get; set; }
        public IdFilter CategoryId { get; set; }
        public IdFilter BrandId { get; set; }
    }
}