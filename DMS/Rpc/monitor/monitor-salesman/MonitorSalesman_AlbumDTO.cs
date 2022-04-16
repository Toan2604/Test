using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_salesman
{
    public class MonitorSalesman_AlbumDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public MonitorSalesman_AlbumDTO() { }
        public MonitorSalesman_AlbumDTO(Album Album)
        {
            this.Id = Album.Id;
            this.Name = Album.Name;
            this.StatusId = Album.StatusId;
        }
    }

    public class MonitorSalesman_AlbumFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public AlbumOrder OrderBy { get; set; }
    }
}
