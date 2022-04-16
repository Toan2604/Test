using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_KpiCriteriaItemDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PermissionMobile_KpiCriteriaItemDTO() { }
        public PermissionMobile_KpiCriteriaItemDTO(KpiCriteriaItem KpiCriteriaItem)
        {

            this.Id = KpiCriteriaItem.Id;

            this.Code = KpiCriteriaItem.Code;

            this.Name = KpiCriteriaItem.Name;

            this.Errors = KpiCriteriaItem.Errors;
        }
    }
}
