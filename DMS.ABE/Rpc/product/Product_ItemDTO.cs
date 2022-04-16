using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DMS.ABE.Rpc.product
{
    public class Product_ItemDTO : DataDTO
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
        public bool HasInventory { get; set; }
        public DateTime? LastUpdateInventory { get; set; }
        public Product_ProductDTO Product { get; set; }
        public List<Product_ImageDTO> Images { get; set; }
        public Product_ItemDTO() { }
        public Product_ItemDTO(Item Item)
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
            this.HasInventory = Item.HasInventory;
            this.LastUpdateInventory = Item.LastUpdateInventory;
            this.Product = Item.Product == null ? null : new Product_ProductDTO(Item.Product);
            this.Images = Item.ItemImageMappings?.Where(iim => iim.Image != null).Select(iim => new Product_ImageDTO(iim.Image)).ToList();
            this.Errors = Item.Errors;
        }
    }

    public class Product_ItemFilterDTO : FilterDTO
    {
        public string Search { get; set; }
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
        public bool? IsNew { get; set; }
        public ItemOrder OrderBy { get; set; }
    }
}
