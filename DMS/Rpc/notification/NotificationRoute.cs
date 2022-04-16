using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.notification
{
    [DisplayName("Thông báo tới ứng dụng")]
    public class NotificationRoute : Root
    {
        public const string Parent = Module + "/alert";
        public const string Master = Module + "/alert/notification/notification-master";
        public const string Detail = Module + "/alert/notification/notification-detail/*";
        public const string Mobile = Module + ".notification.*";

        private const string Default = Rpc + Module + "/notification";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Send = Default + "/send";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-tempate";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListNotificationStatus = Default + "/filter-list-notification-status";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListNotificationStatus = Default + "/single-list-notification-status";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(NotificationFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(NotificationFilter.Title), FieldTypeEnum.STRING.Id },
            { nameof(NotificationFilter.Content), FieldTypeEnum.STRING.Id },
            { nameof(NotificationFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(NotificationFilter.NotificationStatusId), FieldTypeEnum.ID.Id },
        };


        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, FilterListNotificationStatus, FilterListAppUser } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, Detail, Create, Send, SingleListAppUser, SingleListOrganization, SingleListNotificationStatus } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, Detail, Update, Send, SingleListAppUser, SingleListOrganization, SingleListNotificationStatus } },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListOrganization, Detail, Delete, SingleListAppUser, SingleListOrganization, SingleListNotificationStatus } },
        };
    }
}
