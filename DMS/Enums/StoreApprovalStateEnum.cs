using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class StoreApprovalStateEnum
    {
        public static GenericEnum PENDING = new GenericEnum { Id = 1, Code = "PENDING", Name = "Chờ đại lý phê duyệt" };
        public static GenericEnum APPROVED = new GenericEnum { Id = 2, Code = "APPROVED", Name = "Đại lý phê duyệt" };
        public static GenericEnum REJECTED = new GenericEnum { Id = 3, Code = "REJECTED", Name = "Đại lý từ chối" };
        public static GenericEnum DRAFT = new GenericEnum { Id = 4, Code = "DRAFT", Name = "Nháp" };


        public static List<GenericEnum> StoreApprovalStateEnumList = new List<GenericEnum> {
            PENDING, APPROVED, REJECTED, DRAFT
        };
    }
}
