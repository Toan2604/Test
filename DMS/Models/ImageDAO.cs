using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ImageDAO
    {
        public ImageDAO()
        {
            AlbumImageMappings = new HashSet<AlbumImageMappingDAO>();
            Banners = new HashSet<BannerDAO>();
            Categories = new HashSet<CategoryDAO>();
            ItemImageMappings = new HashSet<ItemImageMappingDAO>();
            LuckyDrawAvatarImages = new HashSet<LuckyDrawDAO>();
            LuckyDrawImages = new HashSet<LuckyDrawDAO>();
            ProblemImageMappings = new HashSet<ProblemImageMappingDAO>();
            ProductImageMappings = new HashSet<ProductImageMappingDAO>();
            ShowingCategories = new HashSet<ShowingCategoryDAO>();
            ShowingItemImageMappings = new HashSet<ShowingItemImageMappingDAO>();
            StoreCheckingImageMappings = new HashSet<StoreCheckingImageMappingDAO>();
            StoreImageMappings = new HashSet<StoreImageMappingDAO>();
            StoreScoutingImageMappings = new HashSet<StoreScoutingImageMappingDAO>();
            SurveyQuestionImageMappings = new HashSet<SurveyQuestionImageMappingDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Đường dẫn Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Đường dẫn Url
        /// </summary>
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Ngày xoá
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual ICollection<AlbumImageMappingDAO> AlbumImageMappings { get; set; }
        public virtual ICollection<BannerDAO> Banners { get; set; }
        public virtual ICollection<CategoryDAO> Categories { get; set; }
        public virtual ICollection<ItemImageMappingDAO> ItemImageMappings { get; set; }
        public virtual ICollection<LuckyDrawDAO> LuckyDrawAvatarImages { get; set; }
        public virtual ICollection<LuckyDrawDAO> LuckyDrawImages { get; set; }
        public virtual ICollection<ProblemImageMappingDAO> ProblemImageMappings { get; set; }
        public virtual ICollection<ProductImageMappingDAO> ProductImageMappings { get; set; }
        public virtual ICollection<ShowingCategoryDAO> ShowingCategories { get; set; }
        public virtual ICollection<ShowingItemImageMappingDAO> ShowingItemImageMappings { get; set; }
        public virtual ICollection<StoreCheckingImageMappingDAO> StoreCheckingImageMappings { get; set; }
        public virtual ICollection<StoreImageMappingDAO> StoreImageMappings { get; set; }
        public virtual ICollection<StoreScoutingImageMappingDAO> StoreScoutingImageMappings { get; set; }
        public virtual ICollection<SurveyQuestionImageMappingDAO> SurveyQuestionImageMappings { get; set; }
    }
}
