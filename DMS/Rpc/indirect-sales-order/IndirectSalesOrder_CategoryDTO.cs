using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.indirect_sales_order
{
    public class IndirectSalesOrder_CategoryDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public IndirectSalesOrder_CategoryDTO Parent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IndirectSalesOrder_CategoryDTO() { }
        public IndirectSalesOrder_CategoryDTO(Category Category)
        {
            this.Id = Category.Id;
            this.Code = Category.Code;
            this.Name = Category.Name;
            this.ParentId = Category.ParentId;
            this.Path = Category.Path;
            this.Level = Category.Level;
            this.StatusId = Category.StatusId;
            this.ImageId = Category.ImageId;
            this.RowId = Category.RowId;
            this.Used = Category.Used;
            this.Parent = Category.Parent == null ? null : new IndirectSalesOrder_CategoryDTO(Category.Parent);
            this.CreatedAt = Category.CreatedAt;
            this.UpdatedAt = Category.UpdatedAt;
            this.Errors = Category.Errors;
        }
    }

    public class IndirectSalesOrder_CategoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
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
        public CategoryOrder OrderBy { get; set; }
    }
}
