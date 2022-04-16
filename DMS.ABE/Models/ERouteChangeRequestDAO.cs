using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// Y&#234;u c&#7847;u thay &#273;&#7893;i tuy&#7871;n
    /// </summary>
    public partial class ERouteChangeRequestDAO
    {
        public ERouteChangeRequestDAO()
        {
            ERouteChangeRequestContents = new HashSet<ERouteChangeRequestContentDAO>();
        }

        public long Id { get; set; }
        public long ERouteId { get; set; }
        public long CreatorId { get; set; }
        public long RequestStateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual AppUserDAO Creator { get; set; }
        public virtual ERouteDAO ERoute { get; set; }
        public virtual RequestStateDAO RequestState { get; set; }
        public virtual ICollection<ERouteChangeRequestContentDAO> ERouteChangeRequestContents { get; set; }
    }
}
