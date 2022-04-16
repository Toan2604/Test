using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MDistrict
{
    public interface IDistrictService : IServiceScoped
    {
        Task<int> Count(DistrictFilter DistrictFilter);
        Task<List<District>> List(DistrictFilter DistrictFilter);
        Task<District> Get(long Id);
        Task<DataFile> Export(DistrictFilter DistrictFilter);
    }

    public class DistrictService : BaseService, IDistrictService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IDistrictValidator DistrictValidator;

        public DistrictService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IDistrictValidator DistrictValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.DistrictValidator = DistrictValidator;
        }
        public async Task<int> Count(DistrictFilter DistrictFilter)
        {
            try
            {
                int result = await UOW.DistrictRepository.Count(DistrictFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DistrictService));
            }
            return 0;
        }

        public async Task<List<District>> List(DistrictFilter DistrictFilter)
        {
            try
            {
                List<District> Districts = await UOW.DistrictRepository.List(DistrictFilter);
                return Districts;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(DistrictService));
            }
            return null;
        }
        public async Task<District> Get(long Id)
        {
            District District = await UOW.DistrictRepository.Get(Id);
            if (District == null)
                return null;
            return District;
        }

        public async Task<DataFile> Export(DistrictFilter DistrictFilter)
        {
            List<District> Districts = await UOW.DistrictRepository.List(DistrictFilter);
            MemoryStream MemoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = CurrentContext.UserName;
                excelPackage.Workbook.Properties.Title = nameof(District);
                excelPackage.Workbook.Properties.Created = StaticParams.DateTimeNow;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                int StartColumn = 1;
                int StartRow = 2;
                int IdColumn = 0 + StartColumn;
                int NameColumn = 1 + StartColumn;
                int PriorityColumn = 2 + StartColumn;
                int ProvinceIdColumn = 3 + StartColumn;
                int StatusIdColumn = 4 + StartColumn;

                worksheet.Cells[1, IdColumn].Value = nameof(District.Id);
                worksheet.Cells[1, NameColumn].Value = nameof(District.Name);
                worksheet.Cells[1, PriorityColumn].Value = nameof(District.Priority);
                worksheet.Cells[1, ProvinceIdColumn].Value = nameof(District.ProvinceId);
                worksheet.Cells[1, StatusIdColumn].Value = nameof(District.StatusId);

                for (int i = 0; i < Districts.Count; i++)
                {
                    District District = Districts[i];
                    worksheet.Cells[i + StartRow, IdColumn].Value = District.Id;
                    worksheet.Cells[i + StartRow, NameColumn].Value = District.Name;
                    worksheet.Cells[i + StartRow, PriorityColumn].Value = District.Priority;
                    worksheet.Cells[i + StartRow, ProvinceIdColumn].Value = District.ProvinceId;
                    worksheet.Cells[i + StartRow, StatusIdColumn].Value = District.StatusId;
                }
                excelPackage.Save();
            }

            DataFile DataFile = new DataFile
            {
                Name = nameof(District),
                Content = MemoryStream,
            };
            return DataFile;
        }
    }
}
