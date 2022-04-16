using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MSurvey
{
    public interface ISurveyValidator : IServiceScoped
    {
        Task<bool> Create(Survey Survey);
        Task<bool> Update(Survey Survey);
        Task<bool> Delete(Survey Survey);
        Task<bool> SaveForm(Survey Survey);
    }

    public class SurveyValidator : ISurveyValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            EndDateWrong,
            TitleEmpty,
            StartDateEmpty,
            QuestionIsMandatory,
            ContentEmpty,
            SurveyInUsed,
            ContentSurveyOptionsEmpty,
            SurveyQuestionsEmpty,
            SurveyOptionsEmpty,
            OrganizationEmpty,
            OrganizationNotExisted
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public SurveyValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Survey Survey)
        {
            SurveyFilter SurveyFilter = new SurveyFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Survey.Id },
                Selects = SurveySelect.Id
            };

            int count = await UOW.SurveyRepository.Count(SurveyFilter);
            if (count == 0)
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateSurveyQuestions(Survey Survey)
        {
            if (Survey.SurveyQuestions == null || !Survey.SurveyQuestions.Any())
            {
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.SurveyQuestions), ErrorCode.SurveyQuestionsEmpty);
            }
            else
            {
                foreach (var SurveyQuestion in Survey.SurveyQuestions)
                {
                    if (string.IsNullOrWhiteSpace(SurveyQuestion.Content))
                        SurveyQuestion.AddError(nameof(SurveyValidator), nameof(SurveyQuestion.Content), ErrorCode.ContentEmpty);
                    if (SurveyQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                    {

                    }
                    else
                    {
                        if (SurveyQuestion.SurveyOptions == null || !SurveyQuestion.SurveyOptions.Any())
                        {
                            SurveyQuestion.AddError(nameof(SurveyValidator), nameof(SurveyQuestion.SurveyOptions), ErrorCode.SurveyOptionsEmpty);
                        }
                        else
                            foreach (var SurveyOption in SurveyQuestion.SurveyOptions)
                            {
                                if (string.IsNullOrWhiteSpace(SurveyOption.Content))
                                    SurveyOption.AddError(nameof(SurveyValidator), nameof(SurveyOption.Content), ErrorCode.ContentSurveyOptionsEmpty);
                            }
                    }
                }
            }
            return Survey.IsValidated;
        }

        private async Task<bool> ValidateTitle(Survey Survey)
        {
            if (string.IsNullOrWhiteSpace(Survey.Title))
            {
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.Title), ErrorCode.TitleEmpty);
            }
            return Survey.IsValidated;
        }

        private async Task<bool> ValidateDate(Survey Survey)
        {
            if (Survey.StartAt == default(DateTime))
            {
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.StartAt), ErrorCode.StartDateEmpty);
            }
            else
            {
                if (Survey.EndAt.HasValue)
                {
                    if (Survey.EndAt.Value.Date < StaticParams.DateTimeNow.Date)
                    {
                        Survey.AddError(nameof(SurveyValidator), nameof(Survey.EndAt), ErrorCode.EndDateWrong);
                    }
                    else if (Survey.EndAt.Value < Survey.StartAt)
                    {
                        Survey.AddError(nameof(SurveyValidator), nameof(Survey.EndAt), ErrorCode.EndDateWrong);
                    }
                }

            }
            return Survey.IsValidated;
        }

        private async Task<bool> ValidateMandatoryQuestion(Survey Survey)
        {
            var MandatoryQuestions = Survey.SurveyQuestions.Where(x => x.IsMandatory).ToList();
            foreach (var MandatoryQuestion in MandatoryQuestions)
            {
                if (MandatoryQuestion.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.QUESTION_SINGLE_CHOICE.Id
                || MandatoryQuestion.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.QUESTION_MULTIPLE_CHOICE.Id)
                {
                    if (MandatoryQuestion.ListResult.All(x => x.Value == false))
                        MandatoryQuestion.AddError(nameof(SurveyValidator), nameof(MandatoryQuestion.IsMandatory), ErrorCode.QuestionIsMandatory);
                }
                else if (MandatoryQuestion.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.TABLE_MULTIPLE_CHOICE.Id
                || MandatoryQuestion.SurveyQuestionTypeId == Enums.SurveyQuestionTypeEnum.TABLE_SINGLE_CHOICE.Id)
                {
                    foreach (var Row in MandatoryQuestion.TableResult)
                    {
                        if (Row.Value.All(x => x.Value == false))
                        {
                            MandatoryQuestion.AddError(nameof(SurveyValidator), nameof(MandatoryQuestion.IsMandatory), ErrorCode.QuestionIsMandatory);
                            break;
                        }
                    }
                }
                if (MandatoryQuestion.SurveyQuestionTypeId == SurveyQuestionTypeEnum.QUESTION_TEXT.Id)
                {
                    if (string.IsNullOrWhiteSpace(MandatoryQuestion.TextResult))
                        MandatoryQuestion.AddError(nameof(SurveyValidator), nameof(MandatoryQuestion.IsMandatory), ErrorCode.QuestionIsMandatory);
                }
            }
            return Survey.IsValidated;
        }
        private async Task<bool> ValidateOrganization(Survey Survey)
        {
            if (Survey.OrganizationId == 0)
            {
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                int count = await UOW.OrganizationRepository.CountAll(new OrganizationFilter
                {
                    Id = new IdFilter { Equal = Survey.OrganizationId },
                });
                if (count == 0)
                {
                    Survey.AddError(nameof(SurveyValidator), nameof(Survey.Organization), ErrorCode.OrganizationNotExisted);
                }
            }
            return true;
        }

        private async Task<bool> CanDelete(Survey Survey)
        {
            SurveyResultFilter SurveyResultFilter = new SurveyResultFilter
            {
                SurveyId = new IdFilter { Equal = Survey.Id }
            };

            int count = await UOW.SurveyResultRepository.Count(SurveyResultFilter);
            if (count > 0)
                Survey.AddError(nameof(SurveyValidator), nameof(Survey.Id), ErrorCode.SurveyInUsed);
            return Survey.IsValidated;
        }

        public async Task<bool> Create(Survey Survey)
        {
            await ValidateSurveyQuestions(Survey);
            await ValidateTitle(Survey);
            await ValidateDate(Survey);
            await ValidateOrganization(Survey);

            return Survey.IsValidated;
        }

        public async Task<bool> Update(Survey Survey)
        {
            if (await ValidateId(Survey))
            {
                await ValidateSurveyQuestions(Survey);
                await ValidateTitle(Survey);
                await ValidateDate(Survey);
                await ValidateOrganization(Survey);
            }
            return Survey.IsValidated;
        }

        public async Task<bool> Delete(Survey Survey)
        {
            if (await ValidateId(Survey))
            {
            }
            return Survey.IsValidated;
        }

        public async Task<bool> SaveForm(Survey Survey)
        {
            if (await ValidateId(Survey))
            {
                await ValidateMandatoryQuestion(Survey);
            }
            return Survey.IsValidated;
        }
    }
}
