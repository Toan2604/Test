using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting
{
    public class StoreScouting_OrganizationDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public long Level { get; set; }

        public long StatusId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        public bool IsDisplay { get; set; }


        public StoreScouting_OrganizationDTO() { }
        public StoreScouting_OrganizationDTO(Organization Organization)
        {

            this.Id = Organization.Id;

            this.Code = Organization.Code;

            this.Name = Organization.Name;

            this.ParentId = Organization.ParentId;

            this.Path = Organization.Path;

            this.Level = Organization.Level;

            this.StatusId = Organization.StatusId;

            this.Phone = Organization.Phone;

            this.Email = Organization.Email;

            this.Address = Organization.Address;
            this.IsDisplay = Organization.IsDisplay;

            this.Errors = Organization.Errors;
        }
    }
}