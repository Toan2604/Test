using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class ProblemStatusEnum
    {
        public static GenericEnum WAITING = new GenericEnum { Id = 1, Code = "WAITING", Name = "Chờ xử lý" };
        public static GenericEnum PROCESSING = new GenericEnum { Id = 2, Code = "PROCESSING", Name = "Đang xử lý" };
        public static GenericEnum DONE = new GenericEnum { Id = 3, Code = "DONE", Name = "Hoàn thành" };
        public static List<GenericEnum> ProblemStatusEnumList = new List<GenericEnum>()
        {
            WAITING, PROCESSING, DONE
        };
    }
}
