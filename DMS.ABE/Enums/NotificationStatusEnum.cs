using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Enums
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
    }
}
