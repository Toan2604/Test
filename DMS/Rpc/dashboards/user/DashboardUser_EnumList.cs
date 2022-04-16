using TrueSight.Common;

namespace DMS.Rpc.dashboards.user
{
    public class DashboardUser_EnumList
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DashboardUser_EnumList(GenericEnum GenericEnum)
        {
            this.Id = GenericEnum.Id;
            this.Name = GenericEnum.Name;
        }
    }
}
