using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using RestSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace DMS.ABE.Services.MImage
{
    public interface IImageService : IServiceScoped
    {
        Task<List<DMS.ABE.Entities.Image>> List(ImageFilter ImageFilter);
        Task<DMS.ABE.Entities.Image> Get(long Id);
        Task<DMS.ABE.Entities.Image> Create(DMS.ABE.Entities.Image Image, string path);
        Task<DMS.ABE.Entities.Image> Create(DMS.ABE.Entities.Image Image, string path, string thumbnailPath, int width, int height);
        Task<DMS.ABE.Entities.Image> Delete(DMS.ABE.Entities.Image Image);
    }

    public class ImageService : BaseService, IImageService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IImageValidator ImageValidator;

        public ImageService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IImageValidator ImageValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ImageValidator = ImageValidator;
        }
        public async Task<int> Count(ImageFilter ImageFilter)
        {
            try
            {
                int result = await UOW.ImageRepository.Count(ImageFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ImageService));
            }
            return 0;
        }

        public async Task<List<DMS.ABE.Entities.Image>> List(ImageFilter ImageFilter)
        {
            try
            {
                List<DMS.ABE.Entities.Image> Images = await UOW.ImageRepository.List(ImageFilter);
                return Images;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ImageService));
            }
            return null;
        }
        public async Task<DMS.ABE.Entities.Image> Get(long Id)
        {
            DMS.ABE.Entities.Image Image = await UOW.ImageRepository.Get(Id);
            if (Image == null)
                return null;
            return Image;
        }

        public async Task<DMS.ABE.Entities.Image> Delete(DMS.ABE.Entities.Image Image)
        {
            if (!await ImageValidator.Delete(Image))
                return Image;

            try
            {
                await UOW.Begin();
                await UOW.ImageRepository.Delete(Image);
                await UOW.Commit();
                Logging.CreateAuditLog(new { }, Image, nameof(ImageService));
                return Image;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

                Logging.CreateSystemLog(ex, nameof(ImageService));
            }
            return null;
        }

        public async Task<DMS.ABE.Entities.Image> Create(DMS.ABE.Entities.Image Image, string path)
        {
            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest("/rpc/utils/file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", Image.Content, Image.Name);
            restRequest.AddParameter("path", path);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Image.Id = response.Data.Id;
                    Image.Url = "/rpc/utils/file/download" + response.Data.Path;
                }
                await UOW.Begin();
                await UOW.ImageRepository.Create(Image);
                await UOW.Commit();
                return Image;
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<DMS.ABE.Entities.Image> Create(DMS.ABE.Entities.Image Image, string path, string thumbnailPath, int width, int height)
        {
            // save original image
            RestClient restClient = new RestClient(InternalServices.UTILS);
            RestRequest restRequest = new RestRequest("/rpc/utils/file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", Image.Content, Image.Name);
            restRequest.AddParameter("path", path);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Image.Id = response.Data.Id;
                    Image.Url = "/rpc/utils/file/download" + response.Data.Path;
                }
            }
            catch
            {
                return null;
            }

            // save thumbnail image
            MemoryStream output = new MemoryStream();
            MemoryStream input = new MemoryStream(Image.Content);
            using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(input, out SixLabors.ImageSharp.Formats.IImageFormat format))
            {
                image.Mutate(x => x
                     .Resize(width, height));
                image.Save(output, format); // Automatic encoder selected based on extension.
            }

            restClient = new RestClient(InternalServices.UTILS);
            restRequest = new RestRequest("/rpc/utils/file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", output.ToArray(), $"thumbs_{Image.Name}");
            restRequest.AddParameter("path", thumbnailPath);
            try
            {
                var response = restClient.Execute<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Image.ThumbnailUrl = "/rpc/utils/file/download" + response.Data.Path;
                }
            }
            catch
            {
                return null;
            }

            await UOW.Begin();
            await UOW.ImageRepository.Create(Image);
            await UOW.Commit();
            return Image;
        }

        public class File
        {
            public long Id { get; set; }
            public string Path { get; set; }
        }
    }
}
