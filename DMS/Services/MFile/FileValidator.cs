using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MFile
{
    public interface IFileValidator : IServiceScoped
    {
        Task<bool> Create(File File);
        Task<bool> Update(File File);
        Task<bool> Delete(File File);
        Task<bool> BulkDelete(List<File> Files);
        Task<bool> Import(List<File> Files);
    }

    public class FileValidator : IFileValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public FileValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(File File)
        {
            FileFilter FileFilter = new FileFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = File.Id },
                Selects = FileSelect.Id
            };

            int count = await UOW.FileRepository.Count(FileFilter);
            if (count == 0)
                File.AddError(nameof(FileValidator), nameof(File.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(File File)
        {
            return File.IsValidated;
        }

        public async Task<bool> Update(File File)
        {
            if (await ValidateId(File))
            {
            }
            return File.IsValidated;
        }

        public async Task<bool> Delete(File File)
        {
            if (await ValidateId(File))
            {
            }
            return File.IsValidated;
        }

        public async Task<bool> BulkDelete(List<File> Files)
        {
            return true;
        }

        public async Task<bool> Import(List<File> Files)
        {
            return true;
        }
    }
}
