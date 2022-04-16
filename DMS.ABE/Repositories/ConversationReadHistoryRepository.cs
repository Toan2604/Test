using TrueSight.Common;
using DMS.ABE.Common;
using DMS.ABE.Helpers;
using DMS.ABE.Entities;
using DMS.ABE.Models;
using DMS.ABE.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;

namespace DMS.ABE.Repositories
{
    public interface IConversationReadHistoryRepository
    {
        Task<List<ConversationReadHistory>> List(List<long> ConversationIds, long GlobalUserId);
        Task<ConversationReadHistory> Get(long ConversationId, long GlobalUserId);
        Task<bool> Read(long ConversationId, long GlobalUserId);
    }
    public class ConversationReadHistoryRepository : IConversationReadHistoryRepository
    {
        private DataContext DataContext;
        public ConversationReadHistoryRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }
        public async Task<List<ConversationReadHistory>> List(List<long> ConversationIds, long GlobalUserId)
        {
            IdFilter IdFilter = new IdFilter { In = ConversationIds };

            IQueryable<ConversationReadHistoryDAO> query = DataContext.ConversationReadHistory.AsNoTracking();
            query = query.Where(q => q.ConversationId, IdFilter);
            query = query.Where(q => q.GlobalUserId == GlobalUserId);
            List<ConversationReadHistory> ConversationReadHistories = await query.AsNoTracking()
            .Select(x => new ConversationReadHistory()
            {
                Id = x.Id,
                ConversationId = x.ConversationId,
                GlobalUserId = x.GlobalUserId,
                ReadAt = x.ReadAt,
                CountUnread = x.CountUnread,
            }).ToListAsync();
            
            return ConversationReadHistories;
        }

        public async Task<ConversationReadHistory> Get(long ConversationId, long GlobalUserId)
        {
            ConversationReadHistory ConversationReadHistory = await DataContext.ConversationReadHistory.AsNoTracking()
            .Where(x => x.ConversationId == ConversationId && x.GlobalUserId == GlobalUserId)
            .Select(x => new ConversationReadHistory()
            {
                Id = x.Id,
                ConversationId = x.ConversationId,
                GlobalUserId = x.GlobalUserId,
                ReadAt = x.ReadAt,
                CountUnread = x.CountUnread,
            }).FirstOrDefaultAsync();

            if (ConversationReadHistory == null)
                return null;
            return ConversationReadHistory;
        }
        public async Task<bool> Read(long ConversationId, long GlobalUserId)
        {
            ConversationReadHistoryDAO ConversationReadHistoryDAO = await DataContext.ConversationReadHistory
                .Where(x => x.ConversationId == ConversationId && x.GlobalUserId == GlobalUserId)
                .FirstOrDefaultAsync();
            if (ConversationReadHistoryDAO == null)
            {
                ConversationReadHistoryDAO = new ConversationReadHistoryDAO();
                ConversationReadHistoryDAO.ConversationId = ConversationId;
                ConversationReadHistoryDAO.GlobalUserId = GlobalUserId;
                DataContext.ConversationReadHistory.Add(ConversationReadHistoryDAO);
            }
            ConversationReadHistoryDAO.CountUnread = 0;
            ConversationReadHistoryDAO.ReadAt = StaticParams.DateTimeNow;
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
