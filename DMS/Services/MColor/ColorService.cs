using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MColor
{
    public interface IColorService : IServiceScoped
    {
        Task<int> Count(ColorFilter ColorFilter);
        Task<List<Color>> List(ColorFilter ColorFilter);
    }

    public class ColorService : BaseService, IColorService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IColorValidator ColorValidator;

        public ColorService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IColorValidator ColorValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.ColorValidator = ColorValidator;
        }
        public async Task<int> Count(ColorFilter ColorFilter)
        {
            try
            {
                int result = await UOW.ColorRepository.Count(ColorFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ColorService));
                throw new MessageException(ex.InnerException);
            }
        }

        public async Task<List<Color>> List(ColorFilter ColorFilter)
        {
            try
            {
                List<Color> Colors = await UOW.ColorRepository.List(ColorFilter);
                return Colors;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(ColorService));
                throw new MessageException(ex.InnerException);
            }
        }
    }
}
