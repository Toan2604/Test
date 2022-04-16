using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.kpi_item
{
    public class KpiItem_AppUserDTO : DataDTO
    {

        public long Id { get; set; }

        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }


        public KpiItem_AppUserDTO() { }
        public KpiItem_AppUserDTO(AppUser AppUser)
        {

            this.Id = AppUser.Id;

            this.Username = AppUser.Username;

            this.DisplayName = AppUser.DisplayName;

            this.Address = AppUser.Address;

            this.Email = AppUser.Email;

            this.Phone = AppUser.Phone;

            this.Errors = AppUser.Errors;
        }
    }

    public class KpiItem_AppUserFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Username { get; set; }

        public StringFilter DisplayName { get; set; }

        public StringFilter Address { get; set; }

        public StringFilter Email { get; set; }

        public StringFilter Phone { get; set; }

        public IdFilter PositionId { get; set; }

        public StringFilter Department { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StatusId { get; set; }

        public IdFilter ProvinceId { get; set; }

        public IdFilter SexId { get; set; }

        public DateFilter Birthday { get; set; }
        public IdFilter KpiYearId { get; set; }
        public IdFilter KpiPeriodId { get; set; }
        public IdFilter KpiItemTypeId { get; set; }

        public AppUserOrder OrderBy { get; set; }
    }
}