using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ItemDTO : DataDTO
    {

        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string ScanCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal SaleStock { get; set; }
        public long StatusId { get; set; }
        public DirectSalesOrder_ProductDTO Product { get; set; }
        public List<DirectSalesOrder_ItemImageMappingDTO> ItemImageMappings { get; set; }
        public DirectSalesOrder_ItemDTO() { }
        public DirectSalesOrder_ItemDTO(Item Item)
        {
            this.Id = Item.Id;
            this.ProductId = Item.ProductId;
            this.Code = Item.Code;
            this.ERPCode = Item.ERPCode;
            this.Name = Item.Name;
            this.ScanCode = Item.ScanCode;
            this.SalePrice = Item.SalePrice;
            this.RetailPrice = Item.RetailPrice;
            this.SaleStock = Item.SaleStock;
            this.StatusId = Item.StatusId;
            this.Product = Item.Product == null ? null : new DirectSalesOrder_ProductDTO(Item.Product);
            this.ItemImageMappings = Item.ItemImageMappings?.Select(x => new DirectSalesOrder_ItemImageMappingDTO(x)).ToList();
            this.Errors = Item.Errors;
        }
    }

    public class DirectSalesOrder_ItemFilterDTO : FilterDTO
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
        public ItemOrder OrderBy { get; set; }
    }
}