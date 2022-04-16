using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.ABE.Enums
{
    public class GeneralApprovalStateEnum
    {
        public static GenericEnum NEW = new GenericEnum { Id = 1, Code = "NEW", Name = "Mới tạo" };
        public static GenericEnum PENDING = new GenericEnum { Id = 2, Code = "PENDING", Name = "Chờ duyệt" };
        public static GenericEnum REJECTED = new GenericEnum { Id = 3, Code = "REJECTED", Name = "Từ chối" };
        public static GenericEnum APPROVED = new GenericEnum { Id = 4, Code = "APPROVED", Name = "Hoàn thành" };
        public static GenericEnum STORE_PENDING = new GenericEnum { Id = 5, Code = "STORE_PENDING", Name = "Chờ đại lý phê duyệt" };
        public static GenericEnum STORE_REJECTED = new GenericEnum { Id = 6, Code = "STORE_REJECTD", Name = "Đại lý từ chối" };
        public static GenericEnum STORE_APPROVED = new GenericEnum { Id = 7, Code = "STORE_APPROVED", Name = "Đại lý phê duyệt" };
        public static GenericEnum STORE_DRAFT = new GenericEnum { Id = 8, Code = "STORE_DRAFT", Name = "Nháp" };

        public static List<GenericEnum> StoreApprovalStateEnumList = new List<GenericEnum> {
            NEW, PENDING, APPROVED, REJECTED, STORE_PENDING, STORE_REJECTED, STORE_APPROVED, STORE_DRAFT
        };
    }
}
