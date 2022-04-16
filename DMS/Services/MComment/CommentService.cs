using TrueSight.Common;
using DMS.Helpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Entities;
using DMS.Repositories;
using DMS.Common;
using DMS.Enums;
using DMS.Handlers.Configuration;
using DMS.Services.MComment;

namespace DMS.Service.MComment
{
    public interface ICommentService : IServiceScoped
    {
        Task<int> Count(Guid DiscussionId);
        Task<List<Comment>> List(Guid DiscussionId, OrderType OrderType);
        Task<Comment> Create(Comment Comment);
        Task<Comment> Update(Comment Comment);
        Task<bool> Delete(Comment Comment);
    }
    public class CommentService : ICommentService
    {
        private IUOW UOW;
        private ICurrentContext CurrentContext;
        private ICommentTemplate CommentTemplate;
        private IRabbitManager RabbitManager;
        public CommentService(
            IUOW UOW, 
            ICurrentContext CurrentContext, 
            ICommentTemplate CommentTemplate,
            IRabbitManager RabbitManager
         )
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
            this.CommentTemplate = CommentTemplate;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(Guid DiscussionId)
        {
            return await UOW.CommentRepository.Count(DiscussionId);
        }
        public async Task<List<Comment>> List(Guid DiscussionId, OrderType OrderType)
        {
            List<Comment> Comments = await UOW.CommentRepository.List(DiscussionId, OrderType);
            var CurrentUserRowId = CurrentContext.UserRowId;
            var CurrentUser = await UOW.GlobalUserRepository.Get(CurrentUserRowId);
            foreach (Comment Comment in Comments)
            {
                if (Comment.CreatorId == CurrentUser.Id)
                    Comment.IsOwner = true;
                else
                    Comment.IsOwner = false;

            }    
            return Comments;
        }

        public async Task<Comment> Create(Comment Comment)
        {
            var CurrentUserRowId = CurrentContext.UserRowId;
            var CurrentUser = await UOW.GlobalUserRepository.Get(CurrentUserRowId);
            Comment.CreatorId = CurrentUser.Id;
            await UOW.CommentRepository.Create(Comment);
            await SendNotifications(Comment);
            await UOW.CommentRepository.Update(Comment);
            return await UOW.CommentRepository.Get(Comment.Id);
        }

        public async Task<Comment> Update(Comment Comment)
        {
            await UOW.CommentRepository.Update(Comment);
            await SendNotifications(Comment);
            return await UOW.CommentRepository.Get(Comment.Id);
        }

        public async Task<bool> Delete(Comment Comment)
        {
            await UOW.CommentRepository.Delete(Comment);
            return true;
        }

        private async Task SendNotifications(Comment Comment)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(Comment.Content);
                
                //get link của đối tượng được comment
                var data_mobile = doc.DocumentNode.SelectNodes("//span[@data-mobile]").ToList();
                var data_web = doc.DocumentNode.SelectNodes("//span[@data-web]").ToList();
                List<string> mobile_links = data_mobile.Select(l => l.Attributes["data-mobile"].Value).ToList();
                List<string> web_links = data_web.Select(l => l.Attributes["data-web"].Value).ToList();
                string mobile_dms_link = mobile_links.Where(x => x.Contains("dms.")).FirstOrDefault();
                string mobile_abe_link = mobile_links.Where(x => x.Contains("dms-abe.")).FirstOrDefault();
                string web_dms_link = web_links.Where(x => x.Contains("dms/")).FirstOrDefault();
                string web_abe_link = web_links.Where(x => x.Contains("order-hub-external/")).FirstOrDefault();
                //get code và name của đối tượng được comment
                var data_content = doc.DocumentNode.SelectNodes("//span[@data-content]").ToList();
                var content = data_content.Select(l => l.Attributes["data-content"].Value).FirstOrDefault();
                var code = content.Split("/").FirstOrDefault();
                var name = content.Split("/").LastOrDefault();
                //get rowid của người được tag
                var links = doc.DocumentNode.SelectNodes("//span[@data-rowid]").ToList(); 
                List<Guid> RowIds = links.Select(l => l.Attributes["data-rowid"].Value).Select(x => Guid.TryParse(x, out Guid result) ? result : default(Guid)).Distinct().ToList();
                List<GlobalUser> GlobalUsers = await UOW.GlobalUserRepository.List(new GlobalUserFilter
                {
                    RowId = new GuidFilter { In = RowIds },
                    Skip = 0,
                    Take = RowIds.Count,
                    Selects = GlobalUserSelect.ALL
                });
                List<long> GlobalUserIds = GlobalUsers.Select(x => x.Id).Distinct().ToList();
                List<GlobalUserNotification> GlobalUserNotifications = new List<GlobalUserNotification>();
                List<GlobalUserCommentMapping> GlobalUserCommentMappings = new List<GlobalUserCommentMapping>();
                GlobalUser CurrentUser = await UOW.GlobalUserRepository.Get(CurrentContext.UserRowId);
                //gửi thông báo cho người được tag
                foreach (GlobalUser GlobalUser in GlobalUsers)  
                {
                    if (GlobalUser.GlobalUserTypeId == GlobalUserTypeEnum.APPUSER.Id)
                    {
                        if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("direct-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId, 
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_ORDER);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("indirect-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_ORDER);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("store-scouting"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_STORESCOUTING);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("problem"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_PROBLEM);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                    else if (GlobalUser.GlobalUserTypeId == GlobalUserTypeEnum.STOREUSER.Id)
                    {
                        if (!string.IsNullOrEmpty(web_abe_link) && web_abe_link.Contains("direct-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_abe_link, mobile_abe_link, CurrentUser,
                                NotificationType.COMMENT_ORDER);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                    GlobalUserCommentMapping GlobalUserCommentMapping = new GlobalUserCommentMapping
                    {
                        GlobalUserId = GlobalUser.Id
                    };
                    GlobalUserCommentMappings.Add(GlobalUserCommentMapping);
                }


                //gửi thông báo cho người liên quan
                var Comments = await UOW.CommentRepository.List(Comment.DiscussionId, OrderType.ASC);
                GlobalUserIds.Add(CurrentUser.Id);
                List<long> RelatedUserIds = Comments.Select(x => x.CreatorId).Distinct().ToList();
                RelatedUserIds = RelatedUserIds.Except(GlobalUserIds).ToList();
                var RelatedUsers = await UOW.GlobalUserRepository.List(RelatedUserIds);
                foreach (GlobalUser GlobalUser in RelatedUsers)
                {
                    if (GlobalUser.GlobalUserTypeId == GlobalUserTypeEnum.APPUSER.Id)
                    {
                        if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("direct-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_ORDER_RELATED);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("indirect-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_ORDER_RELATED);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("store-scouting"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_STORESCOUTING_RELATED);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                        else if (!string.IsNullOrEmpty(web_dms_link) && web_dms_link.Contains("problem"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_dms_link, mobile_dms_link, CurrentUser,
                                NotificationType.COMMENT_PROBLEM_RELATED);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                    else if (GlobalUser.GlobalUserTypeId == GlobalUserTypeEnum.STOREUSER.Id)
                    {
                        if (!string.IsNullOrEmpty(web_abe_link) && web_abe_link.Contains("direct-sales-order"))
                        {
                            var GlobalUserNotification = CommentTemplate.CreateNotification(
                                CurrentContext.UserRowId,
                                GlobalUser.RowId,
                                code, name, web_abe_link, mobile_abe_link, CurrentUser,
                                NotificationType.COMMENT_ORDER_RELATED);
                            GlobalUserNotifications.Add(GlobalUserNotification);
                        }
                    }
                }
                RabbitManager.PublishList(GlobalUserNotifications, RoutingKeyEnum.GlobalUserNotificationSend.Code);

                if (Comment.GlobalUserCommentMappings == null)
                {
                    Comment.GlobalUserCommentMappings = new List<GlobalUserCommentMapping>();
                    Comment.GlobalUserCommentMappings.AddRange(GlobalUserCommentMappings);
                }
                else
                    Comment.GlobalUserCommentMappings.AddRange(GlobalUserCommentMappings);
            }
            catch (Exception ex)
            {

            }
        }
    }    
}
