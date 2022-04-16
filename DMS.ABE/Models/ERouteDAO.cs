using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Thi&#7871;t l&#7853;p tuy&#7871;n
    /// </summary>
    public partial class ERouteDAO
    {
        public ERouteDAO()
        {
            ERouteChangeRequests = new HashSet<ERouteChangeRequestDAO>();
            ERouteContents = new HashSet<ERouteContentDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long SaleEmployeeId { get; set; }
        public long OrganizationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime RealStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long ERouteTypeId { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long CreatorId { get; set; }
        /// <summary>
        /// Id global cho phê duyệt
        /// </summary>
        public Guid RowId { get; set; }
        public long RequestStateId { get; set; }
        public bool Used { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual ERouteTypeDAO ERouteType { get; set; }
        public virtual OrganizationDAO Organization { get; set; }
        public virtual RequestStateDAO RequestState { get; set; }
        public virtual AppUserDAO SaleEmployee { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<ERouteChangeRequestDAO> ERouteChangeRequests { get; set; }
        public virtual ICollection<ERouteContentDAO> ERouteContents { get; set; }
    }
}
