using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_albums
{
    public class MonitorStoreAlbum_AlbumDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public MonitorStoreAlbum_AlbumDTO() { }
        public MonitorStoreAlbum_AlbumDTO(Album Album)
        {
            this.Id = Album.Id;
            this.Name = Album.Name;
            this.StatusId = Album.StatusId;
        }
    }

    public class MonitorStoreAlbum_AlbumFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public AlbumOrder OrderBy { get; set; }
    }
}
