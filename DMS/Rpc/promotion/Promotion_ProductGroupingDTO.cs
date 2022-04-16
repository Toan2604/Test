using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_ProductGroupingDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public long Level { get; set; }


        public Promotion_ProductGroupingDTO() { }
        public Promotion_ProductGroupingDTO(ProductGrouping ProductGrouping)
        {

            this.Id = ProductGrouping.Id;

            this.Code = ProductGrouping.Code;

            this.Name = ProductGrouping.Name;

            this.Description = ProductGrouping.Description;

            this.ParentId = ProductGrouping.ParentId;

            this.Path = ProductGrouping.Path;

            this.Level = ProductGrouping.Level;

            this.Errors = ProductGrouping.Errors;
        }
    }

    public class Promotion_ProductGroupingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Description { get; set; }

        public IdFilter ParentId { get; set; }

        public StringFilter Path { get; set; }

        public LongFilter Level { get; set; }

        public ProductGroupingOrder OrderBy { get; set; }
    }
}