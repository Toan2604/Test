using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class ERouteTypeDAO
    {
        public ERouteTypeDAO()
        {
            ERoutes = new HashSet<ERouteDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ERouteDAO> ERoutes { get; set; }
    }
}
