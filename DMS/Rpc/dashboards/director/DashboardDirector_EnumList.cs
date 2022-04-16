using TrueSight.Common;

namespace DMS.Rpc.dashboards.director
{
    public class DashboardDirector_EnumList
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public DashboardDirector_EnumList(GenericEnum GenericEnum)
        {
            this.Id = GenericEnum.Id;
            this.Name = GenericEnum.Name;
        }
    }
}
