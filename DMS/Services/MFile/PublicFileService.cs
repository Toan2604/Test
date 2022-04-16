using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Entities;
using DMS.Repositories;
using DMS.Helpers;
using System.IO;
using RestSharp;
using DMS.Common;
using System.Net;

namespace DMS.Services.MFile
{
    public interface IPublicFileService : IServiceScoped
    {
        Task<PublicFile> Download(long Id);
        Task<PublicFile> Create(PublicFile PublicFile);
        Task<bool> Delete(long Id);
    }
    public class PublicFileService : IPublicFileService
    {
        public IUOW UOW;
        public ILogging Logging;
        public ICurrentContext CurrentContext;
        public PublicFileService(
                    IUOW UOW, 
                    ILogging Logging,
                    ICurrentContext CurrentContext
            )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
        }


        public async Task<PublicFile> Download(long Id)
        {
            return null;
        }

        public async Task<PublicFile> Create(PublicFile PublicFile)
        {
            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest("/rpc/utils/public-file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", PublicFile.Content, PublicFile.Name);
            restRequest.AddParameter("path", PublicFile.Path);
            try
            {
                var response = restClient.Execute<PublicFile>(restRequest);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    PublicFile.Id = response.Data.Id;
                    PublicFile.Path = "/rpc/utils/public-file/download" + response.Data.Path;
                    PublicFile.RowId = response.Data.RowId;
                    return PublicFile;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(long Id)
        {
            return true;
        }
    }
}
