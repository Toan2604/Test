using TrueSight.Common;

namespace DMS.Rpc.dashboards.monitor
{
    public class DashboardMonitor_EnumList
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DashboardMonitor_EnumList(GenericEnum GenericEnum)
        {
            this.Id = GenericEnum.Id;
            this.Name = GenericEnum.Name;
        }
    }
}
