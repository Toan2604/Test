using DMS.Entities;
using DMS.Models;
using System;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_albums
{
    public class MonitorStoreAlbum_StoreCheckingImageMappingDTO : DataDTO
    {
        public long ImageId { get; set; }
        public long StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long? Distance { get; set; }
        public DateTime ShootingAt { get; set; }
        public MonitorStoreAlbum_AlbumDTO Album { get; set; }
        public MonitorStoreAlbum_AppUserDTO SaleEmployee { get; set; }
        public MonitorStoreAlbum_ImageDTO Image { get; set; }
        public MonitorStoreAlbum_StoreDTO Store { get; set; }
        public MonitorStoreAlbum_StoreCheckingImageMappingDTO() { }
        public MonitorStoreAlbum_StoreCheckingImageMappingDTO(StoreCheckingImageMapping StoreCheckingImageMapping)
        {
            this.ImageId = StoreCheckingImageMapping.ImageId;
            this.StoreCheckingId = StoreCheckingImageMapping.StoreCheckingId;
            this.AlbumId = StoreCheckingImageMapping.AlbumId;
            this.ShootingAt = StoreCheckingImageMapping.ShootingAt;
            this.Album = StoreCheckingImageMapping.Album == null ? null : new MonitorStoreAlbum_AlbumDTO(StoreCheckingImageMapping.Album);
            this.Image = StoreCheckingImageMapping.Image == null ? null : new MonitorStoreAlbum_ImageDTO(StoreCheckingImageMapping.Image);
        }

        public MonitorStoreAlbum_StoreCheckingImageMappingDTO(StoreImageDAO StoreImageDAO)
        {
            this.ImageId = StoreImageDAO.ImageId;
            this.AlbumId = StoreImageDAO.AlbumId;
            this.ShootingAt = StoreImageDAO.ShootingAt;
            this.Distance = StoreImageDAO.Distance;
            this.Album = new MonitorStoreAlbum_AlbumDTO { Name = StoreImageDAO.AlbumName };
            this.SaleEmployee = new MonitorStoreAlbum_AppUserDTO { DisplayName = StoreImageDAO.DisplayName };
            this.Image = new MonitorStoreAlbum_ImageDTO { Url = StoreImageDAO.Url, ThumbnailUrl = StoreImageDAO.ThumbnailUrl };
            this.Store = new MonitorStoreAlbum_StoreDTO { Name = StoreImageDAO.StoreName, Address = StoreImageDAO.StoreAddress };
        }
    }
}
