using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.posm_report
{
    public class POSMReport_ShowingItemDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long ShowingCategoryId { get; set; }

        public long UnitOfMeasureId { get; set; }

        public decimal SalePrice { get; set; }

        public string ERPCode { get; set; }
        public string Description { get; set; }

        public long StatusId { get; set; }

        public bool Used { get; set; }

        public Guid RowId { get; set; }


        public POSMReport_ShowingItemDTO() { }
        public POSMReport_ShowingItemDTO(ShowingItem ShowingItem)
        {

            this.Id = ShowingItem.Id;

            this.Code = ShowingItem.Code;

            this.Name = ShowingItem.Name;

            this.ShowingCategoryId = ShowingItem.ShowingCategoryId;

            this.UnitOfMeasureId = ShowingItem.UnitOfMeasureId;

            this.SalePrice = ShowingItem.SalePrice;

            this.ERPCode = ShowingItem.ERPCode;
            this.Description = ShowingItem.Description;

            this.StatusId = ShowingItem.StatusId;

            this.Used = ShowingItem.Used;

            this.RowId = ShowingItem.RowId;

            this.Errors = ShowingItem.Errors;
        }
    }

    public class POSMReport_ShowingItemFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ShowingCategoryId { get; set; }

        public IdFilter UnitOfMeasureId { get; set; }

        public DecimalFilter SalePrice { get; set; }

        public StringFilter ERPCode { get; set; }
        public StringFilter Description { get; set; }

        public IdFilter StatusId { get; set; }

        public GuidFilter RowId { get; set; }
        public string Search { get; set; }
        public ShowingItemOrder OrderBy { get; set; }
    }
}