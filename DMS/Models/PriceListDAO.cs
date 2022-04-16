using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class PriceListDAO
    {
        public PriceListDAO()
        {
            PriceListItemHistories = new HashSet<PriceListItemHistoryDAO>();
            PriceListItemMappings = new HashSet<PriceListItemMappingDAO>();
            PriceListStoreGroupingMappings = new HashSet<PriceListStoreGroupingMappingDAO>();
            PriceListStoreMappings = new HashSet<PriceListStoreMappingDAO>();
            PriceListStoreTypeMappings = new HashSet<PriceListStoreTypeMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public long CreatorId { get; set; }
        public long OrganizationId { get; set; }
        public long PriceListTypeId { get; set; }
        public long SalesOrderTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        /// <summary>
        /// Id global cho phê duyệt
        /// </summary>
        public Guid RowId { get; set; }
        public long RequestStateId { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual PriceListTypeDAO PriceListType { get; set; }
        public virtual RequestStateDAO RequestState { get; set; }
        public virtual SalesOrderTypeDAO SalesOrderType { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<PriceListItemHistoryDAO> PriceListItemHistories { get; set; }
        public virtual ICollection<PriceListItemMappingDAO> PriceListItemMappings { get; set; }
        public virtual ICollection<PriceListStoreGroupingMappingDAO> PriceListStoreGroupingMappings { get; set; }
        public virtual ICollection<PriceListStoreMappingDAO> PriceListStoreMappings { get; set; }
        public virtual ICollection<PriceListStoreTypeMappingDAO> PriceListStoreTypeMappings { get; set; }
    }
}
