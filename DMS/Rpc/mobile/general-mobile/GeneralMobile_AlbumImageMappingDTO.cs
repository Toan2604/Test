using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_AlbumImageMappingDTO : DataDTO
    {
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long ImageId { get; set; }
        public long OrganizationId { get; set; }
        public long? Distance { get; set; }
        public long? SaleEmployeeId { get; set; }
        public GeneralMobile_AlbumDTO Album { get; set; }
        public GeneralMobile_OrganizationDTO Organization { get; set; }
        public GeneralMobile_AppUserDTO SaleEmployee { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public DateTime ShootingAt { get; set; }
        public GeneralMobile_AlbumImageMappingDTO() { }
        public GeneralMobile_AlbumImageMappingDTO(AlbumImageMapping AlbumImageMapping)
        {
            this.AlbumId = AlbumImageMapping.AlbumId;
            this.StoreId = AlbumImageMapping.StoreId;
            this.ImageId = AlbumImageMapping.ImageId;
            this.OrganizationId = AlbumImageMapping.OrganizationId;
            this.ShootingAt = AlbumImageMapping.ShootingAt;
            this.Distance = AlbumImageMapping.Distance;
            this.SaleEmployeeId = AlbumImageMapping.SaleEmployeeId;
            this.Album = AlbumImageMapping.Album == null ? null : new GeneralMobile_AlbumDTO(AlbumImageMapping.Album);
            this.Organization = AlbumImageMapping.Organization == null ? null : new GeneralMobile_OrganizationDTO(AlbumImageMapping.Organization);
            this.SaleEmployee = AlbumImageMapping.SaleEmployee == null ? null : new GeneralMobile_AppUserDTO(AlbumImageMapping.SaleEmployee);
            this.Image = AlbumImageMapping.Image == null ? null : new GeneralMobile_ImageDTO(AlbumImageMapping.Image);
        }
    }
}
