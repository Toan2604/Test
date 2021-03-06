using TrueSight.Common;
using DMS.Helpers;
using DMS.Entities;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Repositories
{
    public interface IConversationMessageRepository
    {
        Task<int> Count(ConversationMessageFilter ConversationMessageFilter);
        Task<List<ConversationMessage>> List(ConversationMessageFilter ConversationMessageFilter);
        Task<List<ConversationMessage>> List(List<long> Ids);
        Task<ConversationMessage> Get(long Id);
        Task<bool> Create(ConversationMessage ConversationMessage);
        Task<bool> Update(ConversationMessage ConversationMessage);
        Task<bool> Delete(ConversationMessage ConversationMessage);
        Task<bool> BulkMerge(List<ConversationMessage> ConversationMessages);
    }
    public class ConversationMessageRepository : IConversationMessageRepository
    {
        private DataContext DataContext;
        public ConversationMessageRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ConversationMessageDAO> DynamicFilter(IQueryable<ConversationMessageDAO> query, ConversationMessageFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.ConversationId == filter.ConversationId);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.GlobalUserId, filter.GlobalUserId);
            query = query.Where(q => q.Content, filter.Content);
            return query;
        }
      
        private IQueryable<ConversationMessageDAO> DynamicOrder(IQueryable<ConversationMessageDAO> query, ConversationMessageFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ConversationMessageOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ConversationMessageOrder.Conversation:
                            query = query.OrderBy(q => q.ConversationId);
                            break;
                        case ConversationMessageOrder.GlobalUser:
                            query = query.OrderBy(q => q.GlobalUserId);
                            break;
                        case ConversationMessageOrder.Content:
                            query = query.OrderBy(q => q.Content);
                            break;
                        case ConversationMessageOrder.CreatedAt:
                            query = query.OrderBy(q => q.CreatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ConversationMessageOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ConversationMessageOrder.Conversation:
                            query = query.OrderByDescending(q => q.ConversationId);
                            break;
                        case ConversationMessageOrder.GlobalUser:
                            query = query.OrderByDescending(q => q.GlobalUserId);
                            break;
                        case ConversationMessageOrder.Content:
                            query = query.OrderByDescending(q => q.Content);
                            break;
                        case ConversationMessageOrder.CreatedAt:
                            query = query.OrderByDescending(q => q.CreatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<ConversationMessage>> DynamicSelect(IQueryable<ConversationMessageDAO> query, ConversationMessageFilter filter)
        {
            List<ConversationMessage> ConversationMessages = await query.Select(q => new ConversationMessage()
            {
                Id = q.Id,
                ConversationId = q.ConversationId,
                GlobalUserId = q.GlobalUserId,
                Content = q.Content,
                Conversation = q.Conversation != null ? new Conversation
                {
                    Id = q.Conversation.Id,
                    Name = q.Conversation.Name,
                } : null,
                GlobalUser = q.GlobalUser != null ? new GlobalUser
                {
                    Id = q.GlobalUser.Id,
                    Username = q.GlobalUser.Username,
                    DisplayName = q.GlobalUser.DisplayName,
                    RowId = q.GlobalUser.RowId,
                    Avatar = q.GlobalUser.Avatar
                } : null,
                CreatedAt = q.CreatedAt,
                UpdatedAt = q.UpdatedAt,
            }).ToListWithNoLockAsync();
            List<long> Ids = ConversationMessages.Select(x => x.Id).ToList();

            List<ConversationAttachmentDAO> ConversationAttachments = await DataContext.ConversationAttachment
                .Where(x => Ids.Contains(x.ConversationMessageId))
                .ToListWithNoLockAsync();
            foreach (ConversationMessage ConversationMessage in ConversationMessages)
            {
                ConversationMessage.ConversationAttachments = ConversationAttachments
                   .Where(x => x.ConversationMessageId == ConversationMessage.Id)
                   .Select(x => new ConversationAttachment
                   {
                       Id = x.Id,
                       ConversationMessageId = x.ConversationMessageId,
                       ConversationAttachmentTypeId = x.ConversationAttachmentTypeId,
                       Url = x.Url,
                       Size = x.Size,
                       Name = x.Name,
                       Type = x.Type,
                       Thumbnail = x.Thumbnail,
                   }).ToList();
            }
            return ConversationMessages;
        }

        public async Task<int> Count(ConversationMessageFilter filter)
        {
            IQueryable<ConversationMessageDAO> ConversationMessages = DataContext.ConversationMessage.AsNoTracking();
            ConversationMessages = DynamicFilter(ConversationMessages, filter);
            return await ConversationMessages.CountWithNoLockAsync();
        }

        public async Task<List<ConversationMessage>> List(ConversationMessageFilter filter)
        {
            if (filter == null) return new List<ConversationMessage>();
            IQueryable<ConversationMessageDAO> ConversationMessageDAOs = DataContext.ConversationMessage.AsNoTracking();
            ConversationMessageDAOs = DynamicFilter(ConversationMessageDAOs, filter);
            ConversationMessageDAOs = DynamicOrder(ConversationMessageDAOs, filter);
            List<ConversationMessage> ConversationMessages = await DynamicSelect(ConversationMessageDAOs, filter);
            return ConversationMessages;
        }

        public async Task<List<ConversationMessage>> List(List<long> Ids)
        {
            List<ConversationMessage> ConversationMessages = await DataContext.ConversationMessage.AsNoTracking()
            .Where(x => Ids.Contains(x.Id)).Select(x => new ConversationMessage()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                ConversationId = x.ConversationId,
                GlobalUserId = x.GlobalUserId,
                Content = x.Content,
                Conversation = x.Conversation == null ? null : new Conversation
                {
                    Id = x.Conversation.Id,
                    Name = x.Conversation.Name,
                },
                GlobalUser = x.GlobalUser == null ? null : new GlobalUser
                {
                    Id = x.GlobalUser.Id,
                    Username = x.GlobalUser.Username,
                    DisplayName = x.GlobalUser.DisplayName,
                    Avatar = x.GlobalUser.Avatar,
                    RowId = x.GlobalUser.RowId,
                },
            }).ToListWithNoLockAsync();

            return ConversationMessages;
        }

        public async Task<ConversationMessage> Get(long Id)
        {
            ConversationMessage ConversationMessage = await DataContext.ConversationMessage.AsNoTracking()
            .Where(x => x.Id == Id)
            .Where(x => x.DeletedAt == null)
            .Select(x => new ConversationMessage()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Id = x.Id,
                ConversationId = x.ConversationId,
                GlobalUserId = x.GlobalUserId,
                Content = x.Content,
                Conversation = x.Conversation == null ? null : new Conversation
                {
                    Id = x.Conversation.Id,
                    Name = x.Conversation.Name,
                },
                GlobalUser = x.GlobalUser == null ? null : new GlobalUser
                {
                    Id = x.GlobalUser.Id,
                    Username = x.GlobalUser.Username,
                    DisplayName = x.GlobalUser.DisplayName,
                    Avatar = x.GlobalUser.Avatar,
                    RowId = x.GlobalUser.RowId,
                },
            }).FirstOrDefaultWithNoLockAsync();

            if (ConversationMessage == null)
                return null;
            ConversationMessage.ConversationAttachments = await DataContext.ConversationAttachment.AsNoTracking()
                .Where(x => x.ConversationMessageId == ConversationMessage.Id)
                .Select(x => new ConversationAttachment
                {
                    Id = x.Id,
                    ConversationMessageId = x.ConversationMessageId,
                    ConversationAttachmentTypeId = x.ConversationAttachmentTypeId,
                    Url = x.Url,
                    Size = x.Size,
                    Name = x.Name,
                    Type = x.Type,
                    Thumbnail = x.Thumbnail,
                }).ToListWithNoLockAsync();
            return ConversationMessage;
        }
        public async Task<bool> Create(ConversationMessage ConversationMessage)
        {
            ConversationMessageDAO ConversationMessageDAO = new ConversationMessageDAO();
            ConversationMessageDAO.ConversationId = ConversationMessage.ConversationId;
            ConversationMessageDAO.GlobalUserId = ConversationMessage.GlobalUserId;
            ConversationMessageDAO.Content = ConversationMessage.Content ?? string.Empty;
            ConversationMessageDAO.CreatedAt = ConversationMessage.CreatedAt;
            ConversationMessageDAO.UpdatedAt = ConversationMessage.UpdatedAt;
            ConversationMessageDAO.DeletedAt = ConversationMessage.DeletedAt;
            DataContext.ConversationMessage.Add(ConversationMessageDAO);
            await SaveReferences(ConversationMessage);
            return true;
        }

        public async Task<bool> Update(ConversationMessage ConversationMessage)
        {
            ConversationMessageDAO ConversationMessageDAO = DataContext.ConversationMessage.Where(x => x.Id == ConversationMessage.Id).FirstOrDefault();
            if (ConversationMessageDAO == null)
                return false;
            ConversationMessageDAO.ConversationId = ConversationMessage.ConversationId;
            ConversationMessageDAO.GlobalUserId = ConversationMessage.GlobalUserId;
            ConversationMessageDAO.Content = ConversationMessage.Content ?? string.Empty;
            ConversationMessageDAO.CreatedAt = ConversationMessage.CreatedAt;
            ConversationMessageDAO.UpdatedAt = ConversationMessage.UpdatedAt;
            ConversationMessageDAO.DeletedAt = ConversationMessage.DeletedAt;
            await DataContext.SaveChangesAsync();
            await SaveReferences(ConversationMessage);
            return true;
        }

        public async Task<bool> Delete(ConversationMessage ConversationMessage)
        {
            ConversationMessage.UpdatedAt = StaticParams.DateTimeNow;
            ConversationMessage.DeletedAt = StaticParams.DateTimeNow;
            await DataContext.ConversationMessage.Where(x => x.Id == ConversationMessage.Id)
                .UpdateFromQueryAsync(x => new ConversationMessageDAO
                {
                    UpdatedAt = ConversationMessage.UpdatedAt,
                    DeletedAt = ConversationMessage.DeletedAt
                });
            return true;
        }

        public async Task SaveReferences(ConversationMessage ConversationMessage)
        {
            await DataContext.ConversationAttachment
                .Where(x => x.ConversationMessageId == ConversationMessage.Id)
                .DeleteFromQueryAsync();
            if (ConversationMessage.ConversationAttachments != null)
            {
                List<ConversationAttachmentDAO> ConversationAttachmentDAOs = ConversationMessage.ConversationAttachments
                    .Select(x => new ConversationAttachmentDAO
                    {
                        ConversationMessageId = ConversationMessage.Id,
                        ConversationAttachmentTypeId = x.ConversationAttachmentTypeId,
                        Name = x.Name,
                        Size = x.Size,
                        Thumbnail = x.Thumbnail,
                        Url = x.Url,
                        Type = x.Type,
                    }).ToList();
                await DataContext.BulkMergeAsync(ConversationAttachmentDAOs);
            }
            List<ConversationParticipantDAO> ConversationParticipantDAOs = await DataContext.ConversationParticipant
                .Where(x => x.ConversationId == ConversationMessage.ConversationId)
                .ToListWithNoLockAsync();
            List<long> GlobalUserIds = ConversationParticipantDAOs.Select(x => x.GlobalUserId).ToList();
            List<ConversationReadHistoryDAO> ConversationReadHistoryDAOs = await DataContext.ConversationReadHistory
                .Where(x => x.Id, new IdFilter { In = GlobalUserIds })
                .ToListWithNoLockAsync();
            foreach(ConversationReadHistoryDAO ConversationReadHistoryDAO in ConversationReadHistoryDAOs)
            {
                ConversationReadHistoryDAO.CountUnread = await DataContext.ConversationMessage
                    .Where(x => x.ConversationId == ConversationMessage.ConversationId &&
                                x.UpdatedAt > ConversationReadHistoryDAO.ReadAt)
                    .LongCountWithNoLockAsync();
            }
            await DataContext.SaveChangesAsync();
        }
        public async Task<bool> BulkMerge(List<ConversationMessage> ConversationMessages)
        {
            List<long> Ids = ConversationMessages.Select(x => x.Id).ToList();
            List<ConversationMessageDAO> ConversationMessageDAOs = new List<ConversationMessageDAO>();
            List<ConversationAttachmentDAO> ConversationAttachmentDAOs = new List<ConversationAttachmentDAO>();

            foreach (ConversationMessage ConversationMessage in ConversationMessages)
            {
                ConversationMessageDAO ConversationMessageDAO = new ConversationMessageDAO();
                ConversationMessageDAO.Id = ConversationMessage.Id;
                ConversationMessageDAO.ConversationId = ConversationMessage.ConversationId;
                ConversationMessageDAO.GlobalUserId = ConversationMessage.GlobalUserId;
                ConversationMessageDAO.Content = ConversationMessage.Content ?? string.Empty;
                ConversationMessageDAO.CreatedAt = ConversationMessage.CreatedAt;
                ConversationMessageDAO.UpdatedAt = ConversationMessage.UpdatedAt;
                ConversationMessageDAO.DeletedAt = ConversationMessage.DeletedAt;
                ConversationMessageDAOs.Add(ConversationMessageDAO);

                if (ConversationMessageDAO.ConversationAttachments != null)
                {
                    foreach (ConversationAttachment ConversationAttachment in ConversationMessage.ConversationAttachments)
                    {
                        ConversationAttachmentDAO ConversationAttachmentDAO = new ConversationAttachmentDAO();
                        ConversationAttachmentDAO.Id = ConversationAttachment.Id;
                        ConversationAttachmentDAO.ConversationMessageId = ConversationAttachment.ConversationMessageId;
                        ConversationAttachmentDAO.ConversationAttachmentTypeId = ConversationAttachment.ConversationAttachmentTypeId;
                        ConversationAttachmentDAO.Url = ConversationAttachment.Url;
                        ConversationAttachmentDAO.Thumbnail = ConversationAttachment.Thumbnail;
                        ConversationAttachmentDAO.Size = ConversationAttachment.Size;
                        ConversationAttachmentDAO.Name = ConversationAttachment.Name;
                        ConversationAttachmentDAO.Checksum = ConversationAttachment.Checksum;
                        ConversationAttachmentDAO.Type = ConversationAttachment.Type;
                        ConversationAttachmentDAOs.Add(ConversationAttachmentDAO);
                    }
                }
            }
            await DataContext.BulkMergeAsync(ConversationMessageDAOs);
            await DataContext.ConversationAttachment.Where(x => Ids.Contains(x.ConversationMessageId)).DeleteFromQueryAsync();
            await DataContext.BulkMergeAsync(ConversationAttachmentDAOs);
            return true;
        }
    }
}
