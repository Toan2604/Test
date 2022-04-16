using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Repositories
{
    public interface IRequestHistoryRepository
    {
        Task<long> CountRequestHistory<T>(RequestHistoryFilter filter);
        Task<List<RequestHistory<T>>> ListRequestHistory<T>(RequestHistoryFilter filter);
        Task<RequestHistory<T>> GetRequestHistory<T>(Guid Id);
        Task<RequestHistory<T>> CreateRequestHistory<T>(long RequestId, long? AppUserId, T obj, Dictionary<string, object> displayFields, [CallerMemberName] string methodName = "") where T : DataEntity;
        Task<List<RequestHistory<T>>> BulkCreateRequestHistory<T>(Dictionary<long, T> Objects, long? AppUserId, [CallerMemberName] string methodName = "") where T : DataEntity;
    }
    public class RequestHistoryRepository : IRequestHistoryRepository
    {
        protected IMongoClient MongoClient;
        protected IMongoDatabase PreviewMongoDatabase;
        protected DataContext DataContext;
        public RequestHistoryRepository(DataContext DataContext, IMongoClient MongoClient)
        {
            this.MongoClient = MongoClient;
            this.PreviewMongoDatabase = MongoClient.GetDatabase("DMS_RequestHistory_Preview");
            this.DataContext = DataContext;
        }

        protected static Dictionary<string, string> Actions = new Dictionary<string, string>
        {
            { "Create", "Thêm mới" },
            { "Update", "Cập nhật" },
            { "Delete", "Xóa" },
            { "BulkDelete", "Xóa" },
            { "Import", "Nhập Excel" },
        };
        public static void AddAction(string Key, string Value)
        {
            if (Actions.ContainsKey(Key))
            {
                Actions[Key] = Value;
            }
            else
            {
                Actions.Add(Key, Value);
            }
        }

        public IMongoDatabase GetDetailDatabase(DateTime DateTime)
        {
            string dateString = DateTime.ToString("yyyyMMdd");
            string databaseName = $"DMS_RequestHistory_Detail_{dateString}";
            var database = MongoClient.GetDatabase(databaseName);
            return database;
        }

        public static string GetCollectionName<T>()
        {
            string collectionName = $"History_{typeof(T).Name}";
            return collectionName;
        }

        public async Task<long> CountRequestHistory<T>(RequestHistoryFilter filter)
        {
            string collectionName = GetCollectionName<T>();
            var previewCollection = PreviewMongoDatabase.GetCollection<RequestHistory<T>>(collectionName);

            FilterDefinition<RequestHistory<T>> BuilderFilter = Builders<RequestHistory<T>>.Filter.Empty;
            BuilderFilter = BuilderFilter.MgWhere(x => x.RequestId, new IdFilter { Equal = filter.RequestId });
            return await previewCollection.CountDocumentsAsync(BuilderFilter);
        }

        public async Task<List<RequestHistory<T>>> ListRequestHistory<T>(RequestHistoryFilter filter)
        {
            string collectionName = GetCollectionName<T>();
            var previewCollection = PreviewMongoDatabase.GetCollection<RequestHistory<T>>(collectionName);

            FilterDefinition<RequestHistory<T>> BuilderFilter = Builders<RequestHistory<T>>.Filter.Empty;
            BuilderFilter &= Builders<RequestHistory<T>>.Filter.Eq("RequestId", filter.RequestId);

            var fieldsBuilder = Builders<RequestHistory<T>>.Projection;
            var fields = fieldsBuilder.Exclude(d => d.OldContent).Exclude(d => d.Content);

            List<RequestHistory<T>> RequestHistories = await previewCollection
                .Find(BuilderFilter)
                .Project<RequestHistory<T>>(fields)
                .SortByDescending(e => e.SavedAt)
                .Skip(filter.Skip)
                .Limit(filter.Take)
                .ToListAsync();
            return RequestHistories;
        }

        public async Task<RequestHistory<T>> GetRequestHistory<T>(Guid Id)
        {
            string collectionName = GetCollectionName<T>();
            var previewCollection = PreviewMongoDatabase.GetCollection<RequestHistory<T>>(collectionName);

            RequestHistory<T> RequestHistory = await previewCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            DateTime savedAt = RequestHistory.SavedAt;
            var detailDatabase = GetDetailDatabase(savedAt);
            var detailCollection = detailDatabase.GetCollection<RequestHistory<T>>(collectionName);
            RequestHistory = await detailCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();

            return RequestHistory;
        }

        public async Task<RequestHistory<T>> CreateRequestHistory<T>(long RequestId, long? AppUserId, T obj, Dictionary<string, object> displayFields,
            [CallerMemberName] string methodName = "") where T : DataEntity
        {
            string collectionName = GetCollectionName<T>();
            var previewCollection = PreviewMongoDatabase.GetCollection<RequestHistory<T>>(collectionName);

            RequestHistory<T> OldRequestHistory = await previewCollection
                .Find(x => x.RequestId == RequestId)
                .SortByDescending(x => x.SavedAt)
                .FirstOrDefaultAsync();

            if (OldRequestHistory != null)
            {
                DateTime OldRequestHistorySavedAt = OldRequestHistory.SavedAt;
                var oldRequestHistoryDetailDatabase = GetDetailDatabase(OldRequestHistorySavedAt);
                var oldRequestHistoryDetailCollection = oldRequestHistoryDetailDatabase.GetCollection<RequestHistory<T>>(collectionName);
                OldRequestHistory = await oldRequestHistoryDetailCollection
                    .Find(x => x.RequestId == RequestId)
                    .SortByDescending(x => x.SavedAt)
                    .FirstOrDefaultAsync();
            }
            AppUser AppUser = null;
            AppUser = await DataContext.AppUser
                .Where(x => x.Id == AppUserId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                }).FirstOrDefaultWithNoLockAsync();

            var ActionName = Actions.ContainsKey(methodName) ? Actions[methodName] : methodName;
            RequestHistory<T> NewRequestHistory = new RequestHistory<T>
            {
                Id = Guid.NewGuid(),
                AppUserId = AppUserId,
                AppUser = AppUser,
                ActionName = ActionName,
                SavedAt = StaticParams.DateTimeNow,
                DisplayFields = displayFields,
                RequestId = RequestId,
            };
            await previewCollection.InsertOneAsync(NewRequestHistory);

            NewRequestHistory.Content = obj;
            NewRequestHistory.OldContent = OldRequestHistory?.Content;
            var newRequestHistoryDetailDatabase = GetDetailDatabase(NewRequestHistory.SavedAt);
            var newRequestHistoryDetailCollection = newRequestHistoryDetailDatabase.GetCollection<RequestHistory<T>>(collectionName);
            await newRequestHistoryDetailCollection.InsertOneAsync(NewRequestHistory);
            return NewRequestHistory;
        }

        public async Task<List<RequestHistory<T>>> BulkCreateRequestHistory<T>(Dictionary<long, T> Objects, long? AppUserId, [CallerMemberName] string methodName = "") where T : DataEntity
        {
            string collectionName = GetCollectionName<T>();
            var previewCollection = PreviewMongoDatabase.GetCollection<RequestHistory<T>>(collectionName);

            var RequestIds = Objects.Keys.ToList();
            RequestHistory<T> OldRequestHistories = await previewCollection
                .Find(x => RequestIds.Contains(x.RequestId))
                .SortByDescending(x => x.SavedAt)
                .FirstOrDefaultAsync();

            AppUser AppUser = null;
            AppUser = await DataContext.AppUser
                .Where(x => x.Id == AppUserId)
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                }).FirstOrDefaultWithNoLockAsync();

            var ActionName = Actions.ContainsKey(methodName) ? Actions[methodName] : methodName;
            //save preview
            List<RequestHistory<T>> NewRequestHistories = Objects.Select(x => new RequestHistory<T>
            {
                Id = Guid.NewGuid(),
                AppUserId = AppUserId,
                AppUser = AppUser,
                ActionName = ActionName,
                SavedAt = StaticParams.DateTimeNow,
                DisplayFields = null,
                RequestId = x.Key,
            }).ToList();
            await previewCollection.InsertManyAsync(NewRequestHistories);

            //save detail
            NewRequestHistories = Objects.Select(x => new RequestHistory<T>
            {
                Id = Guid.NewGuid(),
                AppUserId = AppUserId,
                AppUser = AppUser,
                Content = x.Value,
                OldContent = null,
                SavedAt = StaticParams.DateTimeNow,
                DisplayFields = null,
                RequestId = x.Key,
            }).ToList();

            var newRequestHistoryDetailDatabase = GetDetailDatabase(StaticParams.DateTimeNow);
            var newRequestHistoryDetailCollection = newRequestHistoryDetailDatabase.GetCollection<RequestHistory<T>>(collectionName);
            await newRequestHistoryDetailCollection.InsertManyAsync(NewRequestHistories);

            return NewRequestHistories;
        }
    }
}
