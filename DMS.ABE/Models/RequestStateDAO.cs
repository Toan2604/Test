using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class RequestStateDAO
    {
        public RequestStateDAO()
        {
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
            ERouteChangeRequests = new HashSet<ERouteChangeRequestDAO>();
            ERoutes = new HashSet<ERouteDAO>();
            IndirectSalesOrders = new HashSet<IndirectSalesOrderDAO>();
            PriceLists = new HashSet<PriceListDAO>();
            RequestWorkflowDefinitionMappings = new HashSet<RequestWorkflowDefinitionMappingDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
        public virtual ICollection<ERouteChangeRequestDAO> ERouteChangeRequests { get; set; }
        public virtual ICollection<ERouteDAO> ERoutes { get; set; }
        public virtual ICollection<IndirectSalesOrderDAO> IndirectSalesOrders { get; set; }
        public virtual ICollection<PriceListDAO> PriceLists { get; set; }
        public virtual ICollection<RequestWorkflowDefinitionMappingDAO> RequestWorkflowDefinitionMappings { get; set; }
    }
}
