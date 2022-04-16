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

namespace DMS.Services.MWard
{
    public interface IWardService : IServiceScoped
    {
        Task<int> Count(WardFilter WardFilter);
        Task<List<Ward>> List(WardFilter WardFilter);
        Task<Ward> Get(long Id);
        Task<DataFile> Export(WardFilter WardFilter);
    }

    public class WardService : BaseService, IWardService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IWardValidator WardValidator;

        public WardService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IWardValidator WardValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.WardValidator = WardValidator;
        }
        public async Task<int> Count(WardFilter WardFilter)
        {
            try
            {
                int result = await UOW.WardRepository.Count(WardFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WardService));
            }
            return 0;
        }

        public async Task<List<Ward>> List(WardFilter WardFilter)
        {
            try
            {
                List<Ward> Wards = await UOW.WardRepository.List(WardFilter);
                return Wards;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(WardService));
            }
            return null;
        }
        public async Task<Ward> Get(long Id)
        {
            Ward Ward = await UOW.WardRepository.Get(Id);
            if (Ward == null)
                return null;
            return Ward;
        }

        public async Task<DataFile> Export(WardFilter WardFilter)
        {
            List<Ward> Wards = await UOW.WardRepository.List(WardFilter);
            MemoryStream MemoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                //Set some properties of the Excel document
                excelPackage.Workbook.Properties.Author = CurrentContext.UserName;
                excelPackage.Workbook.Properties.Title = nameof(Ward);
                excelPackage.Workbook.Properties.Created = StaticParams.DateTimeNow;

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");
                int StartColumn = 1;
                int StartRow = 2;
                int IdColumn = 0 + StartColumn;
                int NameColumn = 1 + StartColumn;
                int PriorityColumn = 2 + StartColumn;
                int DistrictIdColumn = 3 + StartColumn;
                int StatusIdColumn = 4 + StartColumn;

                worksheet.Cells[1, IdColumn].Value = nameof(Ward.Id);
                worksheet.Cells[1, NameColumn].Value = nameof(Ward.Name);
                worksheet.Cells[1, PriorityColumn].Value = nameof(Ward.Priority);
                worksheet.Cells[1, DistrictIdColumn].Value = nameof(Ward.DistrictId);
                worksheet.Cells[1, StatusIdColumn].Value = nameof(Ward.StatusId);

                for (int i = 0; i < Wards.Count; i++)
                {
                    Ward Ward = Wards[i];
                    worksheet.Cells[i + StartRow, IdColumn].Value = Ward.Id;
                    worksheet.Cells[i + StartRow, NameColumn].Value = Ward.Name;
                    worksheet.Cells[i + StartRow, PriorityColumn].Value = Ward.Priority;
                    worksheet.Cells[i + StartRow, DistrictIdColumn].Value = Ward.DistrictId;
                    worksheet.Cells[i + StartRow, StatusIdColumn].Value = Ward.StatusId;
                }
                excelPackage.Save();
            }

            DataFile DataFile = new DataFile
            {
                Name = nameof(Ward),
                Content = MemoryStream,
            };
            return DataFile;
        }
    }
}
