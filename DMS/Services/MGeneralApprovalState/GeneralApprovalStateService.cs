﻿using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MGeneralApprovalState
{
    public interface IGeneralApprovalStateService : IServiceScoped
    {
        Task<int> Count(GeneralApprovalStateFilter GeneralApprovalStateFilter);
        Task<List<GeneralApprovalState>> List(GeneralApprovalStateFilter GeneralApprovalStateFilter);
    }

    public class GeneralApprovalStateService : BaseService, IGeneralApprovalStateService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IGeneralApprovalStateValidator GeneralApprovalStateValidator;

        public GeneralApprovalStateService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IGeneralApprovalStateValidator GeneralApprovalStateValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.GeneralApprovalStateValidator = GeneralApprovalStateValidator;
        }
        public async Task<int> Count(GeneralApprovalStateFilter GeneralApprovalStateFilter)
        {
            try
            {
                int result = await UOW.GeneralApprovalStateRepository.Count(GeneralApprovalStateFilter);
                return result;
            }
            catch (Exception ex)
            {

                Logging.CreateSystemLog(ex, nameof(GeneralApprovalStateService));
            }
            return 0;
        }

        public async Task<List<GeneralApprovalState>> List(GeneralApprovalStateFilter GeneralApprovalStateFilter)
        {
            try
            {
                List<GeneralApprovalState> GeneralApprovalStates = await UOW.GeneralApprovalStateRepository.List(GeneralApprovalStateFilter);
                return GeneralApprovalStates;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(GeneralApprovalStateService));
            }
            return null;
        }
    }
}
