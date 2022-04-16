using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_LuckyNumberGroupingDTO : DataDTO
    {

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long OrganizationId { get; set; }
        public long StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LuckyNumber_OrganizationDTO Organization { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public LuckyNumber_LuckyNumberGroupingDTO() { }
        public LuckyNumber_LuckyNumberGroupingDTO(LuckyNumberGrouping LuckyNumberGrouping)
        {
            this.Organization = LuckyNumberGrouping.Organization == null ? null : new LuckyNumber_OrganizationDTO(LuckyNumberGrouping.Organization);
            this.Errors = LuckyNumberGrouping.Errors;
        }
    }
}