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

namespace DMS.Services.MProvince
{
    public interface IProvinceService : IServiceScoped
    {
        Task<int> Count(ProvinceFilter ProvinceFilter);
        Task<List<Province>> List(ProvinceFilter ProvinceFilter);
        Task<Province> Get(long Id);
        Task<DataFile> Export(ProvinceFilter ProvinceFilter);
    }

    public class ProvinceService : BaseService, IProvinceService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IProvinceValidator ProvinceValidator;

        public ProvinceService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IProvinceValidator ProvinceValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ProvinceValidator = ProvinceValidator;
        }
        public async Task<int> Count(ProvinceFilter ProvinceFilter)
        {
            try
            {
                int result = await UOW.ProvinceRepository.Count(ProvinceFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProvinceService));
            }
            return 0;
        }

        public async Task<List<Province>> List(ProvinceFilter ProvinceFilter)
        {
            try
            {
                List<Province> Provinces = await UOW.ProvinceRepository.List(ProvinceFilter);
                return Provinces;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ProvinceService));
            }
            return null;
        }
        public async Task<Province> Get(long Id)
        {
            Province Province = await UOW.ProvinceRepository.Get(Id);
            if (Province == null)
                return null;
            return Province;
        }

        public async Task<DataFile> Export(ProvinceFilter ProvinceFilter)
        {
            List<Province> Provinces = await UOW.ProvinceRepository.List(ProvinceFilter);
            MemoryStream MemoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = CurrentContext.UserName;
                excelPackage.Workbook.Properties.Title = nameof(Province);
                excelPackage.Workbook.Properties.Created = StaticParams.DateTimeNow;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                int StartColumn = 1;
                int StartRow = 2;
                int IdColumn = 0 + StartColumn;
                int NameColumn = 1 + StartColumn;
                int PriorityColumn = 2 + StartColumn;
                int StatusIdColumn = 3 + StartColumn;

                worksheet.Cells[1, IdColumn].Value = nameof(Province.Id);
                worksheet.Cells[1, NameColumn].Value = nameof(Province.Name);
                worksheet.Cells[1, PriorityColumn].Value = nameof(Province.Priority);
                worksheet.Cells[1, StatusIdColumn].Value = nameof(Province.StatusId);

                for (int i = 0; i < Provinces.Count; i++)
                {
                    Province Province = Provinces[i];
                    worksheet.Cells[i + StartRow, IdColumn].Value = Province.Id;
                    worksheet.Cells[i + StartRow, NameColumn].Value = Province.Name;
                    worksheet.Cells[i + StartRow, PriorityColumn].Value = Province.Priority;
                    worksheet.Cells[i + StartRow, StatusIdColumn].Value = Province.StatusId;
                }
                excelPackage.Save();
            }

            DataFile DataFile = new DataFile
            {
                Name = nameof(Province),
                Content = MemoryStream,
            };
            return DataFile;
        }
    }
}
