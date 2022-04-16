using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Common
{
    public struct WorkflowParameterStruct
    {
        public string Code;
        public string Name;
        public long TypeId;
    }

    public class WorkflowActionEnum
    {
        public static GenericEnum NO_WORKFLOW = new GenericEnum { Id = 1, Code = "NO_WORKFLOW", Name = "NO_WORKFLOW" };
        public static GenericEnum NO_BEGIN_STEP = new GenericEnum { Id = 2, Code = "NO_BEGINSTEP", Name = "NO_BEGINSTEP" };
        public static GenericEnum NO_NEXTSTEP = new GenericEnum { Id = 3, Code = "NO_NEXTSTEP", Name = "NO_NEXTSTEP" };

        public static GenericEnum OK = new GenericEnum { Id = 100, Code = "OK", Name = "OK" };

    }
    public class RequestStateEnum
    {
        public static GenericEnum NEW = new GenericEnum { Id = 1, Code = "NEW", Name = "Mới tạo" };
        public static GenericEnum PENDING = new GenericEnum { Id = 2, Code = "PENDING", Name = "Chờ duyệt" };
        public static GenericEnum APPROVED = new GenericEnum { Id = 3, Code = "APPROVED", Name = "Hoàn thành" };
        public static GenericEnum REJECTED = new GenericEnum { Id = 4, Code = "REJECTED", Name = "Từ chối" };
        public static List<GenericEnum> RequestStateEnumList = new List<GenericEnum>()
        {
            NEW, PENDING, APPROVED, REJECTED
        };
    }

    public class WorkflowStateEnum
    {
        public static GenericEnum NEW = new GenericEnum { Id = 1, Code = "NEW", Name = "Mới" };
        public static GenericEnum PENDING = new GenericEnum { Id = 2, Code = "PENDING", Name = "Chờ duyệt" };
        public static GenericEnum APPROVED = new GenericEnum { Id = 3, Code = "APPROVED", Name = "Đã duyệt" };
        public static GenericEnum REJECTED = new GenericEnum { Id = 4, Code = "REJECTED", Name = "Đã từ chối" };
        public static List<GenericEnum> WorkflowStateEnumList = new List<GenericEnum>()
        {
            NEW, PENDING, APPROVED, REJECTED
        };
    }

    public class WorkflowParameterTypeEnum
    {
        public static GenericEnum ID = new GenericEnum { Id = 1, Code = "ID", Name = "ID" };
        public static GenericEnum STRING = new GenericEnum { Id = 2, Code = "STRING", Name = "STRING" };
        public static GenericEnum LONG = new GenericEnum { Id = 3, Code = "LONG", Name = "LONG" };
        public static GenericEnum DECIMAL = new GenericEnum { Id = 4, Code = "DECIMAL", Name = "DECIMAL" };
        public static GenericEnum DATE = new GenericEnum { Id = 5, Code = "DATE", Name = "DATE" };

        public static List<GenericEnum> List = new List<GenericEnum>()
        {
            ID, STRING, LONG, DECIMAL, DATE,
        };
    }

    public class WorkflowOperatorEnum
    {
        public static GenericEnum ID_EQ = new GenericEnum { Id = 101, Code = "ID_EQ", Name = "=" };
        public static GenericEnum ID_NE = new GenericEnum { Id = 102, Code = "ID_NE", Name = "!=" };
        public static GenericEnum ID_IN = new GenericEnum { Id = 103, Code = "ID_IN", Name = "Chứa" };
        public static GenericEnum ID_NI = new GenericEnum { Id = 104, Code = "ID_NI", Name = "Không chứa" };

        public static GenericEnum STRING_NE = new GenericEnum { Id = 201, Code = "STRING_NE", Name = "!=" };
        public static GenericEnum STRING_EQ = new GenericEnum { Id = 202, Code = "STRING_EQ", Name = "=" };
        public static GenericEnum STRING_SW = new GenericEnum { Id = 203, Code = "STRING_SW", Name = "Bắt đầu bởi" };
        public static GenericEnum STRING_NSW = new GenericEnum { Id = 204, Code = "STRING_NSW", Name = "Không bắt đầu bởi" };
        public static GenericEnum STRING_EW = new GenericEnum { Id = 205, Code = "STRING_EW", Name = "Kết thúc bởi" };
        public static GenericEnum STRING_NEW = new GenericEnum { Id = 206, Code = "STRING_NEW", Name = "Không kết thúc bởi" };
        public static GenericEnum STRING_CT = new GenericEnum { Id = 207, Code = "STRING_CT", Name = "Chứa" };
        public static GenericEnum STRING_NC = new GenericEnum { Id = 208, Code = "STRING_NC", Name = "Không chứa" };

        public static GenericEnum LONG_GT = new GenericEnum { Id = 301, Code = "LONG_GT", Name = ">" };
        public static GenericEnum LONG_GE = new GenericEnum { Id = 302, Code = "LONG_GE", Name = ">=" };
        public static GenericEnum LONG_LT = new GenericEnum { Id = 303, Code = "LONG_LT", Name = "<" };
        public static GenericEnum LONG_LE = new GenericEnum { Id = 304, Code = "LONG_LE", Name = "<=" };
        public static GenericEnum LONG_NE = new GenericEnum { Id = 305, Code = "LONG_NE", Name = "!=" };
        public static GenericEnum LONG_EQ = new GenericEnum { Id = 306, Code = "LONG_EQ", Name = "=" };

        public static GenericEnum DECIMAL_GT = new GenericEnum { Id = 401, Code = "DECIMAL_GT", Name = ">" };
        public static GenericEnum DECIMAL_GE = new GenericEnum { Id = 402, Code = "DECIMAL_GE", Name = ">=" };
        public static GenericEnum DECIMAL_LT = new GenericEnum { Id = 403, Code = "DECIMAL_LT", Name = "<" };
        public static GenericEnum DECIMAL_LE = new GenericEnum { Id = 404, Code = "DECIMAL_LE", Name = "<=" };
        public static GenericEnum DECIMAL_NE = new GenericEnum { Id = 405, Code = "DECIMAL_NE", Name = "!=" };
        public static GenericEnum DECIMAL_EQ = new GenericEnum { Id = 406, Code = "DECIMAL_EQ", Name = "=" };

        public static GenericEnum DATE_GT = new GenericEnum { Id = 501, Code = "DATE_GT", Name = ">" };
        public static GenericEnum DATE_GE = new GenericEnum { Id = 502, Code = "DATE_GE", Name = ">=" };
        public static GenericEnum DATE_LT = new GenericEnum { Id = 503, Code = "DATE_LT", Name = "<" };
        public static GenericEnum DATE_LE = new GenericEnum { Id = 504, Code = "DATE_LE", Name = "<=" };
        public static GenericEnum DATE_NE = new GenericEnum { Id = 505, Code = "DATE_NE", Name = "!=" };
        public static GenericEnum DATE_EQ = new GenericEnum { Id = 506, Code = "DATE_EQ", Name = "=" };


        public static List<GenericEnum> WorkflowOperatorEnumForID = new List<GenericEnum>()
        {
            ID_EQ, ID_NE, ID_IN, ID_NI,
        };

        public static List<GenericEnum> WorkflowOperatorEnumForSTRING = new List<GenericEnum>()
        {
           STRING_NE, STRING_EQ, STRING_SW, STRING_NSW, STRING_EW, STRING_NEW, STRING_CT, STRING_NC,
        };

        public static List<GenericEnum> WorkflowOperatorEnumForLONG = new List<GenericEnum>()
        {
            LONG_GT, LONG_GE, LONG_LT, LONG_LE, LONG_NE, LONG_EQ,
        };

        public static List<GenericEnum> WorkflowOperatorEnumForDECIMAL = new List<GenericEnum>()
        {
            DECIMAL_GT, DECIMAL_GE, DECIMAL_LT, DECIMAL_LE, DECIMAL_NE, DECIMAL_EQ,
        };

        public static List<GenericEnum> WorkflowOperatorEnumForDATE = new List<GenericEnum>()
        {
            DATE_GT, DATE_GE, DATE_LT, DATE_LE, DATE_NE, DATE_EQ,
        };
    }
}
