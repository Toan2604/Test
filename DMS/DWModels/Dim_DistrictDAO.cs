using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_DistrictDAO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long DistrictId { get; set; }
        /// <summary>
        /// Mã quận huyện
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên quận huyện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Thứ tự ưu tiên
        /// </summary>
        public long? Priority { get; set; }
        /// <summary>
        /// Tỉnh phụ thuộc
        /// </summary>
        public long ProvinceId { get; set; }
        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public long StatusId { get; set; }
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
        /// <summary>
        /// Trường để đồng bộ
        /// </summary>
        public Guid RowId { get; set; }
    }
}
