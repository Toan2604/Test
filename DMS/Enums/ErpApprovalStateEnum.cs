using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class ErpApprovalStateEnum
    {
        public static GenericEnum APPROVED = new GenericEnum { Id = 1, Code = "APPROVED", Name = "Duyệt" };
        public static GenericEnum PENDING = new GenericEnum { Id = 2, Code = "PENDING", Name = "Treo" };
        public static GenericEnum INPROGRESS = new GenericEnum { Id = 3, Code = "INPROGRESS", Name = "Đang xuất" };
        public static GenericEnum FINISHED = new GenericEnum { Id = 4, Code = "FINISHED", Name = "Hoàn thành" };
        public static GenericEnum CLOSED = new GenericEnum { Id = 5, Code = "CLOSED", Name = "Đóng" };


        public static List<GenericEnum> ErpApprovalStateEnumList = new List<GenericEnum> {
            APPROVED, PENDING, INPROGRESS, FINISHED, CLOSED
        };
    }
}
