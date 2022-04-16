using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiYearDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PermissionMobile_KpiYearDTO() { }
        public PermissionMobile_KpiYearDTO(KpiYear KpiYear)
        {

            this.Id = KpiYear.Id;

            this.Code = KpiYear.Code;

            this.Name = KpiYear.Name;

            this.Errors = KpiYear.Errors;
        }
    }

    public class PermissionMobile_KpiYearFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public KpiYearOrder OrderBy { get; set; }
    }
}