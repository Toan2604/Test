using TrueSight.Common;
using DMS.ABE.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using DMS.ABE.Repositories;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Common;
using DMS.ABE.Handlers.Configuration;
using RestSharp;
using System.Net;

namespace DMS.ABE.Services.MConversation
{
    public interface IConversationService : IServiceScoped
    {
        Task<int> Count(ConversationFilter ConversationFilter);
        Task<List<Conversation>> List(ConversationFilter ConversationFilter);
        Task<Conversation> Get(long Id);
        Task<Conversation> Create(Conversation Conversation);
        Task<Conversation> Update(Conversation Conversation);
        Task<Conversation> Delete(Conversation Conversation);
    }

    public class ConversationService : IConversationService
    {
        private IUOW UOW;
        private IRabbitManager RabbitManager;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IConversationValidator ConversationValidator;

        public ConversationService(
            IUOW UOW,
            IRabbitManager RabbitManager,
            ICurrentContext CurrentContext,
            IConversationValidator ConversationValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.RabbitManager = RabbitManager;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ConversationValidator = ConversationValidator;
        }
        public async Task<int> Count(ConversationFilter ConversationFilter)
        {
            try
            {
                GlobalUser GlobalUser = await UOW.GlobalUserRepository.Get(CurrentContext.UserRowId);
                if (GlobalUser != null)
                    ConversationFilter.GlobalUserId = GlobalUser.Id;
                int result = await UOW.ConversationRepository.Count(ConversationFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationService));
            }
            return 0;
        }

        public async Task<List<Conversation>> List(ConversationFilter ConversationFilter)
        {
            try
            {
                GlobalUser GlobalUser = await UOW.GlobalUserRepository.Get(CurrentContext.UserRowId);
                if (GlobalUser != null)
                {
                    ConversationFilter.GlobalUserId = GlobalUser.Id;
                    List<Conversation> Conversations = await UOW.ConversationRepository.List(ConversationFilter);
                    List<long> ConversationIds = Conversations.Select(x => x.Id).ToList();
                    List<ConversationReadHistory> ConversationReadHistories = await UOW.ConversationReadHistoryRepository.List(ConversationIds, GlobalUser.Id);
                    foreach (Conversation Conversation in Conversations)
                    {
                        Conversation.CountUnread = ConversationReadHistories.Where(x => x.ConversationId == Conversation.Id)
                            .Select(x => x.CountUnread)
                            .FirstOrDefault();
                    }
                    return Conversations;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationService));
            }
            return null;
        }

        public async Task<Conversation> Get(long Id)
        {
            Conversation Conversation = await UOW.ConversationRepository.Get(Id);
            if (Conversation == null)
                return null;
            return Conversation;
        }

        public async Task<Conversation> Create(Conversation Conversation)
        {
            if (!await ConversationValidator.Create(Conversation))
                return Conversation;

            try
            {
                List<long> GlobalUserIds = Conversation.ConversationParticipants
                    .Select(x => x.GlobalUserId)
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                string hash = string.Join(";", GlobalUserIds);
                int count = await UOW.ConversationRepository.Count(new ConversationFilter
                {
                    Hash = new StringFilter { Equal = hash },
                });
                Conversation.Hash = hash;
                if (count == 0)
                {
                    Conversation.ConversationTypeId = 1;
                    Conversation = await CallRpc(Conversation, "rpc/utils/conversation/create");
                }
                else
                {
                    Conversation = await UOW.ConversationRepository.Get(hash);
                }
                return Conversation;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationService));
            }
            return null;
        }

        public async Task<Conversation> Update(Conversation Conversation)
        {
            if (!await ConversationValidator.Update(Conversation))
                return Conversation;
            try
            {
                List<long> GlobalUserIds = Conversation.ConversationParticipants
                   .Select(x => x.GlobalUserId)
                   .Distinct()
                   .OrderBy(x => x)
                   .ToList();
                string hash = string.Join(";", GlobalUserIds);
                int count = await UOW.ConversationRepository.Count(new ConversationFilter
                {
                    Hash = new StringFilter { Equal = hash },
                });
                Conversation.Hash = hash;
                Conversation = await CallRpc(Conversation, "rpc/utils/conversation/update");
                return Conversation;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationService));
            }
            return null;
        }

        public async Task<Conversation> Delete(Conversation Conversation)
        {
            if (!await ConversationValidator.Delete(Conversation))
                return Conversation;

            try
            {
                Conversation = await CallRpc(Conversation, "rpc/utils/conversation/delete");
                return Conversation;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationService));
            }
            return null;
        }

        private async Task<Conversation> CallRpc(Conversation Conversation, string path)
        {
            IRestClient RestClient = new RestClient(InternalServices.UTILS);
            IRestRequest RestRequest = new RestRequest(path);
            RestRequest.AddJsonBody(Conversation);
            RestRequest.AddHeader("Cookie", "Token=" + CurrentContext.Token);
            IRestResponse<Conversation> result = RestClient.Post<Conversation>(RestRequest);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Data;
            }
            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
            else
            {
                return null;
            }

        }    
    }
}
