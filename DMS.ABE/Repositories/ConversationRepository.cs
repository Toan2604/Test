using TrueSight.Common;
using DMS.ABE.Helpers;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Repositories
{
    public interface IConversationRepository
    {
        Task<int> Count(ConversationFilter ConversationFilter);
        Task<List<Conversation>> List(ConversationFilter ConversationFilter);
        Task<List<Conversation>> List(List<long> Ids);
        Task<Conversation> Get(long Id);
        Task<Conversation> Get(string Hash);
        Task<Conversation> Get(Guid RowId);
    }
    public class ConversationRepository : IConversationRepository
    {
        private DataContext DataContext;
        public ConversationRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        private IQueryable<ConversationDAO> DynamicFilter(IQueryable<ConversationDAO> query, ConversationFilter filter)
        {
            if (filter == null)
                return query.Where(q => false);
            if (filter.GlobalUserId.HasValue)
                query = query.Where(q => q.ConversationParticipants.Any(x => x.GlobalUserId == filter.GlobalUserId.Value));
            query = query.Where(q => !q.DeletedAt.HasValue);
            query = query.Where(q => q.CreatedAt, filter.CreatedAt);
            query = query.Where(q => q.UpdatedAt, filter.UpdatedAt);
            query = query.Where(q => q.Id, filter.Id);
            query = query.Where(q => q.Name, filter.Name);
            query = query.Where(q => q.Hash, filter.Hash);
            query = query.Where(q => q.Avatar, filter.Avatar);
            return query;
        }

        private IQueryable<ConversationDAO> DynamicOrder(IQueryable<ConversationDAO> query, ConversationFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ConversationOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case ConversationOrder.Name:
                            query = query.OrderBy(q => q.Name);
                            break;
                        case ConversationOrder.Avatar:
                            query = query.OrderBy(q => q.Avatar);
                            break;
                        case ConversationOrder.UpdatedAt:
                            query = query.OrderBy(q => q.UpdatedAt);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ConversationOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case ConversationOrder.Name:
                            query = query.OrderByDescending(q => q.Name);
                            break;
                        case ConversationOrder.Avatar:
                            query = query.OrderByDescending(q => q.Avatar);
                            break;
                        case ConversationOrder.UpdatedAt:
                            query = query.OrderByDescending(q => q.UpdatedAt);
                            break;
                    }
                    break;
            }
            query = query.Skip(filter.Skip).Take(filter.Take);
            return query;
        }

        private async Task<List<Conversation>> DynamicSelect(IQueryable<ConversationDAO> query, ConversationFilter filter)
        {
            List<Conversation> Conversations = await query.Select(q => new Conversation()
            {
                Id = q.Id,
            }).ToListAsync();
            List<long> ConversationIds = Conversations.Select(x => x.Id).ToList();

            Conversations = await List(ConversationIds);
            Conversations = Conversations.OrderByDescending(x => x.UpdatedAt).ToList();
            return Conversations;
        }

        public async Task<int> Count(ConversationFilter filter)
        {
            IQueryable<ConversationDAO> Conversations = DataContext.Conversation.AsNoTracking();
            Conversations = DynamicFilter(Conversations, filter);
            return await Conversations.CountAsync();
        }

        public async Task<List<Conversation>> List(ConversationFilter filter)
        {
            if (filter == null) return new List<Conversation>();
            IQueryable<ConversationDAO> ConversationDAOs = DataContext.Conversation.AsNoTracking();
            ConversationDAOs = DynamicFilter(ConversationDAOs, filter);
            ConversationDAOs = DynamicOrder(ConversationDAOs, filter);
            List<Conversation> Conversations = await DynamicSelect(ConversationDAOs, filter);
            return Conversations;
        }

        public async Task<List<Conversation>> List(List<long> Ids)
        {
            IdFilter IdFilter = new IdFilter { In = Ids };
            List<Conversation> Conversations = await DataContext.Conversation.AsNoTracking()
            .Where(x => x.Id, IdFilter)
            .Select(x => new Conversation()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                Name = x.Name,
                Avatar = x.Avatar,
                LatestContent = x.LatestContent,
                LatestGlobalUserId = x.LatestGlobalUserId,
                RowId = x.RowId,
                ConversationTypeId = x.ConversationTypeId,
                Hash = x.Hash,
                LatestGlobalUser = x.LatestGlobalUser == null ? null : new GlobalUser
                {
                    Id = x.LatestGlobalUser.Id,
                    Username = x.LatestGlobalUser.Username,
                    DisplayName = x.LatestGlobalUser.DisplayName,
                },
            }).ToListAsync();

            List<ConversationReadHistory> ConversationReadHistories = await DataContext.ConversationReadHistory.AsNoTracking()
               .Where(x => x.ConversationId, IdFilter)
               .Select(x => new ConversationReadHistory
               {
                   Id = x.Id,
                   ConversationId = x.ConversationId,
                   GlobalUserId = x.GlobalUserId,
                   ReadAt = x.ReadAt,
                   GlobalUser = x.GlobalUser == null ? null : new GlobalUser
                   {
                       Id = x.GlobalUser.Id,
                       Username = x.GlobalUser.Username,
                       DisplayName = x.GlobalUser.DisplayName,
                       Avatar = x.GlobalUser.Avatar,
                       RowId = x.GlobalUser.RowId,
                   },
               }).ToListAsync();
            List<ConversationParticipant> ConversationParticipants = await DataContext.ConversationParticipant.AsNoTracking()
                .Where(x => x.ConversationId, IdFilter)
                .Select(x => new ConversationParticipant
                {
                    Id = x.Id,
                    ConversationId = x.ConversationId,
                    GlobalUserId = x.GlobalUserId,
                    GlobalUser = x.GlobalUser == null ? null : new GlobalUser
                    {
                        Id = x.GlobalUser.Id,
                        Username = x.GlobalUser.Username,
                        DisplayName = x.GlobalUser.DisplayName,
                        Avatar = x.GlobalUser.Avatar,
                        RowId = x.GlobalUser.RowId,
                    },
                }).ToListAsync();
            foreach (Conversation Conversation in Conversations)
            {
                Conversation.ConversationParticipants = ConversationParticipants
                    .Where(x => x.ConversationId == Conversation.Id)
                    .ToList();
                Conversation.ConversationReadHistories = ConversationReadHistories
                    .Where(x => x.ConversationId == Conversation.Id)
                    .ToList();
            }

            return Conversations;
        }

        public async Task<Conversation> Get(long Id)
        {
            Conversation Conversation = await DataContext.Conversation.AsNoTracking()
            .Where(x => x.Id == Id && x.DeletedAt == null)
            .Select(x => new Conversation()
            {
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Id = x.Id,
                ConversationConfigurationId = x.ConversationConfigurationId,
                ConversationTypeId = x.ConversationTypeId,
                Name = x.Name,
                Hash = x.Hash,
                Avatar = x.Avatar,
                LatestContent = x.LatestContent,
                LatestGlobalUserId = x.LatestGlobalUserId,
                RowId = x.RowId,
                ConversationConfiguration = x.ConversationConfiguration == null ? null : new ConversationConfiguration
                {
                    Id = x.ConversationConfiguration.Id,
                    AppId = x.ConversationConfiguration.AppId,
                    AppName = x.ConversationConfiguration.AppName,
                    OaId = x.ConversationConfiguration.OaId,
                },
            }).FirstOrDefaultAsync();
            if (Conversation == null)
                return null;

            Conversation.ConversationParticipants = await DataContext.ConversationParticipant.AsNoTracking()
                .Where(x => x.ConversationId == Conversation.Id)
                .Select(x => new ConversationParticipant
                {
                    Id = x.Id,
                    ConversationId = x.ConversationId,
                    GlobalUserId = x.GlobalUserId,
                    GlobalUser = new GlobalUser
                    {
                        Id = x.GlobalUser.Id,
                        Username = x.GlobalUser.Username,
                        DisplayName = x.GlobalUser.DisplayName,
                        Avatar = x.GlobalUser.Avatar,
                        RowId = x.GlobalUser.RowId,
                        GlobalUserTypeId = x.GlobalUser.GlobalUserTypeId,
                    },
                }).ToListAsync();

            return Conversation;
        }

        public async Task<Conversation> Get(string Hash)
        {
            ConversationDAO ConversationDAO = await DataContext.Conversation.AsNoTracking()
               .Where(x => x.Hash == Hash && x.DeletedAt == null)
               .FirstOrDefaultAsync();
            if (ConversationDAO == null)
                return null;
            return await Get(ConversationDAO.Id);
        }

        public async Task<Conversation> Get(Guid RowId)
        {
            ConversationDAO ConversationDAO = await DataContext.Conversation.AsNoTracking()
               .Where(x => x.RowId == RowId && x.DeletedAt == null)
               .FirstOrDefaultAsync();
            if (ConversationDAO == null)
                return null;
            return await Get(ConversationDAO.Id);
        }

    }
}
