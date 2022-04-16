using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreScoutingDAO
    {
        public StoreScoutingDAO()
        {
            StoreScoutingImageMappings = new HashSet<StoreScoutingImageMappingDAO>();
            Stores = new HashSet<StoreDAO>();
            SurveyResults = new HashSet<SurveyResultDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string OwnerPhone { get; set; }
        public long? ProvinceId { get; set; }
        public long? DistrictId { get; set; }
        public long? WardId { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Description { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreScoutingStatusId { get; set; }
        public long StoreScoutingTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual DistrictDAO District { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual ProvinceDAO Province { get; set; }
        public virtual StoreScoutingStatusDAO StoreScoutingStatus { get; set; }
        public virtual StoreScoutingTypeDAO StoreScoutingType { get; set; }
        public virtual WardDAO Ward { get; set; }
        public virtual ICollection<StoreScoutingImageMappingDAO> StoreScoutingImageMappings { get; set; }
        public virtual ICollection<StoreDAO> Stores { get; set; }
        public virtual ICollection<SurveyResultDAO> SurveyResults { get; set; }
    }
}
