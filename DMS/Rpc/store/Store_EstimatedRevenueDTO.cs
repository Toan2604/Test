using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_EstimatedRevenueDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Store_EstimatedRevenueDTO() { }
        public Store_EstimatedRevenueDTO(EstimatedRevenue EstimatedRevenue)
        {
            this.Id = EstimatedRevenue.Id;
            this.Code = EstimatedRevenue.Code;
            this.Name = EstimatedRevenue.Name;
            this.Errors = EstimatedRevenue.Errors;
        }
    }

    public class Store_EstimatedRevenueFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public EstimatedRevenueOrder OrderBy { get; set; }
    }
}
