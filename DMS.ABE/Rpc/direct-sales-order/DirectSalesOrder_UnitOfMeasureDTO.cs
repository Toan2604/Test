using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_UnitOfMeasureDTO : DataDTO, IEquatable<DirectSalesOrder_UnitOfMeasureDTO>
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public long? Factor { get; set; }
        public long StatusId { get; set; }
        public DirectSalesOrder_UnitOfMeasureDTO() { }
        public DirectSalesOrder_UnitOfMeasureDTO(UnitOfMeasure UnitOfMeasure)
        {

            this.Id = UnitOfMeasure.Id;

            this.Code = UnitOfMeasure.Code;

            this.Name = UnitOfMeasure.Name;

            this.Description = UnitOfMeasure.Description;

            this.StatusId = UnitOfMeasure.StatusId;

            this.Errors = UnitOfMeasure.Errors;
        }
        public DirectSalesOrder_UnitOfMeasureDTO(UnitOfMeasureGroupingContent UnitOfMeasureGroupingContent)
        {

            this.Id = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? 0 : UnitOfMeasureGroupingContent.UnitOfMeasure.Id;

            this.Code = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? string.Empty : UnitOfMeasureGroupingContent.UnitOfMeasure.Code;

            this.Name = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? string.Empty : UnitOfMeasureGroupingContent.UnitOfMeasure.Name;

            this.Description = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? string.Empty : UnitOfMeasureGroupingContent.UnitOfMeasure.Description;

            this.StatusId = UnitOfMeasureGroupingContent.UnitOfMeasure == null ? 0 : UnitOfMeasureGroupingContent.UnitOfMeasure.StatusId;

            this.Factor = UnitOfMeasureGroupingContent.Factor;
        }

        public bool Equals(DirectSalesOrder_UnitOfMeasureDTO other)
        {
            if (other == null) return false;
            return other.Id == this.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class DirectSalesOrder_UnitOfMeasureFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }
        public IdFilter ProductId { get; set; }
        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Description { get; set; }

        public IdFilter StatusId { get; set; }
        public IdFilter UnitOfMeasureGroupingId { get; set; }
        public UnitOfMeasureOrder OrderBy { get; set; }
    }
}