using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.ABE.Entities;
using DMS.ABE.Repositories;
using DMS.ABE.Helpers;
using System.IO;

namespace DMS.ABE.Services.MFile
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
        public PublicFileService(IUOW UOW, ILogging Logging)
        {
            this.UOW = UOW;
            this.Logging = Logging;
        }


        public async Task<PublicFile> Download(long Id)
        {
            return null;
        }

        public async Task<PublicFile> Create(PublicFile PublicFile)
        {
            return null;
        }

        public async Task<bool> Delete(long Id)
        {
            return true;
        }
    }
}
