using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreEstimatedRevenueDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public GeneralMobile_StoreEstimatedRevenueDTO() { }
        public GeneralMobile_StoreEstimatedRevenueDTO(EstimatedRevenue EstimatedRevenue)
        {
            this.Id = EstimatedRevenue.Id;
            this.Code = EstimatedRevenue.Code;
            this.Name = EstimatedRevenue.Name;
            this.Errors = EstimatedRevenue.Errors;
        }
    }

    public class GeneralMobile_StoreEstimatedRevenueFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public RequestStateOrder OrderBy { get; set; }
    }
}
