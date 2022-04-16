using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MConversationMessage
{
    public interface IConversationMessageService : IServiceScoped
    {
        Task<int> Count(ConversationMessageFilter ConversationMessageFilter);
        Task<List<ConversationMessage>> List(ConversationMessageFilter ConversationMessageFilter);
        Task<ConversationMessage> Get(long Id);
        Task<ConversationMessage> CreateFromInside(ConversationMessage ConversationMessage);
        Task<ConversationMessage> Update(ConversationMessage ConversationMessage);
        Task<ConversationMessage> Delete(ConversationMessage ConversationMessage);
    }

    public class ConversationMessageService : IConversationMessageService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IConversationMessageValidator ConversationMessageValidator;

        public ConversationMessageService(
            IUOW UOW,
            ICurrentContext CurrentContext,
            IConversationMessageValidator ConversationMessageValidator,
            ILogging Logging
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ConversationMessageValidator = ConversationMessageValidator;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(ConversationMessageFilter ConversationMessageFilter)
        {
            try
            {
                int result = await UOW.ConversationMessageRepository.Count(ConversationMessageFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationMessageService));
            }
            return 0;
        }

        public async Task<List<ConversationMessage>> List(ConversationMessageFilter ConversationMessageFilter)
        {
            try
            {
                List<ConversationMessage> ConversationMessages = await UOW.ConversationMessageRepository.List(ConversationMessageFilter);
                GlobalUser GlobalUser = await UOW.GlobalUserRepository.Get(CurrentContext.UserRowId);
                if (GlobalUser != null)
                {
                    await UOW.ConversationReadHistoryRepository.Read(ConversationMessageFilter.ConversationId, GlobalUser.Id);
                }
                return ConversationMessages;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationMessageService));
            }
            return null;
        }

        public async Task<ConversationMessage> Get(long Id)
        {
            ConversationMessage ConversationMessage = await UOW.ConversationMessageRepository.Get(Id);
            if (ConversationMessage == null)
                return null;
            return ConversationMessage;
        }

        public async Task<ConversationMessage> CreateFromInside(ConversationMessage ConversationMessage)
        {
            if (!await ConversationMessageValidator.Create(ConversationMessage))
                return ConversationMessage;

            try
            {
                ConversationMessage = await CallRpc(ConversationMessage, "rpc/utils/conversation-message/create");
                return ConversationMessage;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationMessageService));
            }
            return null;
        }

        public async Task<ConversationMessage> Update(ConversationMessage ConversationMessage)
        {
            if (!await ConversationMessageValidator.Update(ConversationMessage))
                return ConversationMessage;
            try
            {
                var oldData = await UOW.ConversationMessageRepository.Get(ConversationMessage.Id);
                ConversationMessage = await CallRpc(ConversationMessage, "rpc/utils/conversation-message/update");
                return ConversationMessage;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationMessageService));
            }
            return null;
        }

        public async Task<ConversationMessage> Delete(ConversationMessage ConversationMessage)
        {
            if (!await ConversationMessageValidator.Delete(ConversationMessage))
                return ConversationMessage;

            try
            {
                ConversationMessage = await CallRpc(ConversationMessage, "rpc/utils/conversation-message/delete");
                return ConversationMessage;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ConversationMessageService));
            }
            return null;
        }

        private async Task<ConversationMessage> CallRpc(ConversationMessage ConversationMessage, string path)
        {
            IRestClient RestClient = new RestClient(InternalServices.UTILS);
            IRestRequest RestRequest = new RestRequest(path);
            RestRequest.AddJsonBody(ConversationMessage);
            RestRequest.AddHeader("Cookie", "Token=" + CurrentContext.Token);
            IRestResponse<ConversationMessage> result = RestClient.Post<ConversationMessage>(RestRequest);
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
