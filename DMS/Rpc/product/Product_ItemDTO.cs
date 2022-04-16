using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.product
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
        public bool HasInventory { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Product_ProductDTO Product { get; set; }
        public Product_StatusDTO Status { get; set; }
        public List<Product_ItemImageMappingDTO> ItemImageMappings { get; set; }
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
            this.HasInventory = Item.HasInventory;
            this.StatusId = Item.StatusId;
            this.Used = Item.Used;
            this.Product = Item.Product == null ? null : new Product_ProductDTO(Item.Product);
            this.Status = Item.Status == null ? null : new Product_StatusDTO(Item.Status);
            this.ItemImageMappings = Item.ItemImageMappings?.Select(x => new Product_ItemImageMappingDTO(x)).ToList();
            this.Errors = Item.Errors;
        }
    }

    public class Product_ItemFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter ProductId { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter ERPCode { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter ScanCode { get; set; }

        public DecimalFilter SalePrice { get; set; }

        public DecimalFilter RetailPrice { get; set; }

        public ItemOrder OrderBy { get; set; }
    }
}