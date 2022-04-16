using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreCheckingImageMappingDTO : DataDTO
    {
        public long ImageId { get; set; }
        public long StoreCheckingId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime ShootingAt { get; set; }
        public long? Distance { get; set; }
        public GeneralMobile_AlbumDTO Album { get; set; }
        public GeneralMobile_AppUserDTO SaleEmployee { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }

        public GeneralMobile_StoreCheckingImageMappingDTO() { }
        public GeneralMobile_StoreCheckingImageMappingDTO(StoreCheckingImageMapping StoreCheckingImageMapping)
        {
            this.ImageId = StoreCheckingImageMapping.ImageId;
            this.StoreCheckingId = StoreCheckingImageMapping.StoreCheckingId;
            this.AlbumId = StoreCheckingImageMapping.AlbumId;
            this.StoreId = StoreCheckingImageMapping.StoreId;
            this.SaleEmployeeId = StoreCheckingImageMapping.SaleEmployeeId;
            this.ShootingAt = StoreCheckingImageMapping.ShootingAt;
            this.Distance = StoreCheckingImageMapping.Distance;
            this.Album = StoreCheckingImageMapping.Album == null ? null : new GeneralMobile_AlbumDTO(StoreCheckingImageMapping.Album);
            this.SaleEmployee = StoreCheckingImageMapping.SaleEmployee == null ? null : new GeneralMobile_AppUserDTO(StoreCheckingImageMapping.SaleEmployee);
            this.Image = StoreCheckingImageMapping.Image == null ? null : new GeneralMobile_ImageDTO(StoreCheckingImageMapping.Image);
            this.Store = StoreCheckingImageMapping.Store == null ? null : new GeneralMobile_StoreDTO(StoreCheckingImageMapping.Store);
            this.Errors = StoreCheckingImageMapping.Errors;
        }
    }

    public class GeneralMobile_ImageStoreCheckingMappingFilterDTO : FilterDTO
    {

        public IdFilter ImageId { get; set; }

        public IdFilter StoreCheckingId { get; set; }

        public IdFilter AlbumId { get; set; }

        public IdFilter StoreId { get; set; }

        public IdFilter SaleEmployeeId { get; set; }

        public DateFilter ShootingAt { get; set; }

        public ImageStoreCheckingMappingOrder OrderBy { get; set; }
    }
}