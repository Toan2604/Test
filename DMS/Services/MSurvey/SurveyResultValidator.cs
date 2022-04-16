using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSurveyResult
{
    public interface ISurveyResultValidator : IServiceScoped
    {
        Task<bool> Create(SurveyResult SurveyResult);
        Task<bool> Delete(SurveyResult SurveyResult);
    }

    public class SurveyResultValidator : ISurveyResultValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            EndDateWrong,
            TitleEmpty,
            DescriptionEmpty,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public SurveyResultValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(SurveyResult SurveyResult)
        {
            SurveyResultFilter SurveyResultFilter = new SurveyResultFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = SurveyResult.Id },
                Selects = SurveyResultSelect.Id
            };

            int count = await UOW.SurveyResultRepository.Count(SurveyResultFilter);
            if (count == 0)
                SurveyResult.AddError(nameof(SurveyResultValidator), nameof(SurveyResult.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> Create(SurveyResult SurveyResult)
        {

            return SurveyResult.IsValidated;
        }

        public async Task<bool> Delete(SurveyResult SurveyResult)
        {
            if (await ValidateId(SurveyResult))
            {
            }
            return SurveyResult.IsValidated;
        }
    }
}
