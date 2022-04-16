using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;

namespace DMS.ABE.Rpc.product
{
    public class Product_CategoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long OrderNumber { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public long ProductCounter { get; set; }
        public Product_ImageDTO Image { get; set; }
        public Product_CategoryDTO Parent { get; set; }
        public Product_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Product_CategoryDTO() { }
        public Product_CategoryDTO(Category Category)
        {
            this.Id = Category.Id;
            this.OrderNumber = Category.OrderNumber;
            this.Code = Category.Code;
            this.Name = Category.Name;
            this.ParentId = Category.ParentId;
            this.Path = Category.Path;
            this.Level = Category.Level;
            this.StatusId = Category.StatusId;
            this.ImageId = Category.ImageId;
            this.RowId = Category.RowId;
            this.Used = Category.Used;
            this.ProductCounter = Category.ProductCounter;
            this.Image = Category.Image == null ? null : new Product_ImageDTO(Category.Image);
            this.Parent = Category.Parent == null ? null : new Product_CategoryDTO(Category.Parent);
            this.Status = Category.Status == null ? null : new Product_StatusDTO(Category.Status);
            this.CreatedAt = Category.CreatedAt;
            this.UpdatedAt = Category.UpdatedAt;
            this.Errors = Category.Errors;
        }
    }

    public class Product_CategoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public LongFilter OrderNumber { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ImageId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public bool? HasChildren { get; set; }
        public CategoryOrder OrderBy { get; set; }
    }
}
