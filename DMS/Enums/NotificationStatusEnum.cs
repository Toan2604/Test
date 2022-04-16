using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Enums
{
    public class NotificationStatusEnum
    {
        public static GenericEnum UNSEND = new GenericEnum { Id = 0, Code = "UNSEND", Name = "Chưa gửi" };
        public static GenericEnum SENT = new GenericEnum { Id = 1, Code = "SENT", Name = "Đã gửi" };
        public static List<GenericEnum> NotificationStatusEnumList = new List<GenericEnum>()
        {
            UNSEND, SENT
        };
    }

    public class NotificationContentEnum
    {
        public static GenericEnum CREATE = new GenericEnum { Id = ((long)NotificationType.CREATE), Code = "CREATE", Name = "Tạo mới", Value = "tạo mới trên hệ thống" };
        public static GenericEnum UPDATE = new GenericEnum { Id = ((long)NotificationType.UPDATE), Code = "UPDATE", Name = "Cập nhật", Value = "cập nhật trên hệ thống" };
        public static GenericEnum DELETE = new GenericEnum { Id = ((long)NotificationType.DELETE), Code = "DELETE", Name = "Xóa", Value = "xóa trên hệ thống" };
        public static GenericEnum SEND = new GenericEnum { Id = ((long)NotificationType.SEND), Code = "SEND", Name = "Gửi", Value = "gửi lên hệ thống" };
        public static GenericEnum FINISH = new GenericEnum { Id = ((long)NotificationType.FINISH), Code = "FINISH", Name = "Chuyển trạng thái hoàn thành", Value = "chuyển trạng thái hoàn thành" };
        public static GenericEnum WAIT = new GenericEnum { Id = ((long)NotificationType.WAIT), Code = "WAIT", Name = "Chuyển trạng thái chờ xử lý", Value = "chuyển trạng thái chờ xử lý" };
        public static List<GenericEnum> NotificationContentEnumList = new List<GenericEnum>()
        {
            CREATE, UPDATE, DELETE, SEND, FINISH, WAIT
        };
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NotificationType
    {
        CREATE = 0,
        UPDATE = 1,
        DELETE = 2,
        SEND = 3,
        APPROVE = 4,
        REJECT = 5,
        FINISH = 6,
        WAIT = 7,
        TOCREATOR = 8,
        TOUPDATER = 9,
        TODELETER = 10,
        TOAPPROVER = 11,
        TOREJECTER = 12,
        TOSENDER = 13,
        TOSALEEMPLOYEE = 14,
        TOCHANGER = 15, //người thay đổi loại đại lý
        CHANGE = 16, //thay đổi loại đại lý
        COMMENT_ORDER = 16, //thay đổi loại đại lý
        COMMENT_PROBLEM = 17,
        COMMENT_STORESCOUTING = 18,
        COMMENT_STORESCOUTING_RELATED = 19,
        COMMENT_ORDER_RELATED = 20,
        COMMENT_PROBLEM_RELATED = 21,
        UPDATE_ERP = 22,
    }
}
