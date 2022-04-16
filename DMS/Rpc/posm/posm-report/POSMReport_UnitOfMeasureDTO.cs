using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public class POSMReport_UnitOfMeasureDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long StatusId { get; set; }

        public bool Used { get; set; }

        public Guid RowId { get; set; }


        public POSMReport_UnitOfMeasureDTO() { }
        public POSMReport_UnitOfMeasureDTO(UnitOfMeasure UnitOfMeasure)
        {

            this.Id = UnitOfMeasure.Id;

            this.Code = UnitOfMeasure.Code;

            this.Name = UnitOfMeasure.Name;

            this.Description = UnitOfMeasure.Description;

            this.StatusId = UnitOfMeasure.StatusId;

            this.Used = UnitOfMeasure.Used;

            this.RowId = UnitOfMeasure.RowId;

            this.Errors = UnitOfMeasure.Errors;
        }
    }

    public class POSMReport_UnitOfMeasureFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Description { get; set; }

        public IdFilter StatusId { get; set; }

        public GuidFilter RowId { get; set; }

        public UnitOfMeasureOrder OrderBy { get; set; }
    }
}