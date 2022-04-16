using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreImageDTO : DataDTO
    {
        public long ImageId { get; set; }
        public long AlbumId { get; set; }
        public long StoreId { get; set; }
        public long SaleEmployeeId { get; set; }
        public DateTime ShootingAt { get; set; }
        public decimal? Distance { get; set; }
        public string AlbumName { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public GeneralMobile_AlbumDTO Album { get; set; }
        public GeneralMobile_AppUserDTO SaleEmployee { get; set; }
        public GeneralMobile_ImageDTO Image { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }
    }
    public class GeneralMobile_StoreImageFilterDTO : FilterDTO
    {
        public IdFilter Time { get; set; }
        public IdFilter StoreId { get; set; }
    }
}
