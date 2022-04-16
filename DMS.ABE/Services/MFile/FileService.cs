using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.ABE.Services.MFile
{
    public interface IFileService : IServiceScoped
    {
        Task<List<DMS.ABE.Entities.File>> List(DMS.ABE.Entities.FileFilter FileFilter);
        Task<DMS.ABE.Entities.File> Get(long Id);
        Task<DMS.ABE.Entities.File> Create(DMS.ABE.Entities.File File, string path);
        Task<DMS.ABE.Entities.File> Delete(DMS.ABE.Entities.File File);
    }
    public class FileService : BaseService, IFileService
    {
        private IUOW UOW;
        private ILogging Logging;
        private IFileValidator FileValidator;
        private ICurrentContext CurrentContext;

        public FileService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IFileValidator FileValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.FileValidator = FileValidator;
        }
        public async Task<File> Create(File File, string path)
        {
            // save original File
            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest("/rpc/utils/file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", File.Content, File.Name);
            restRequest.AddParameter("path", path);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    File.Id = response.Data.Id;
                    File.Path = "/rpc/utils/file/download" + response.Data.Path;
                    File.RowId = response.Data.RowId;
                }

                await UOW.FileRepository.Create(File);

                return File;
            }
            catch
            {
                return null;
            }
        }

        public async Task<File> Delete(File File)
        {
            if (!await FileValidator.Delete(File))
                return File;

            try
            {

                await UOW.FileRepository.Delete(File);

                Logging.CreateAuditLog(new { }, File, nameof(FileService));
                return File;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(FileService));
            }
            return null;
        }

        public async Task<File> Get(long Id)
        {
            DMS.ABE.Entities.File File = await UOW.FileRepository.Get(Id);
            if (File == null)
                return null;
            return File;
        }

        public async Task<List<File>> List(FileFilter FileFilter)
        {
            try
            {
                List<DMS.ABE.Entities.File> Files = await UOW.FileRepository.List(FileFilter);
                return Files;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(FileService));
            }
            return null;
        }
    }
}
