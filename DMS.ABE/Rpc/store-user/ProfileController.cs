using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Services.MProduct;
using DMS.ABE.Services.MStoreUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc.store_user
{
    public class ProfileRoot
    {
        public const string Login = "rpc/dms-abe/account/login";
        public const string Logged = "rpc/dms-abe/account/logged";
        public const string Get = "rpc/dms-abe/profile/get";
        public const string Update = "rpc/dms-abe/profile/update";
        public const string SaveImage = "rpc/dms-abe/profile/save-image";
        public const string ChangePassword = "rpc/dms-abe/profile/change-password";
        public const string ForgotPassword = "rpc/dms-abe/profile/forgot-password";
        public const string VerifyOtpCode = "rpc/dms-abe/profile/verify-otp-code";
        public const string RecoveryPassword = "rpc/dms-abe/profile/recovery-password";
        public const string ToggleFavoriteProduct = "rpc/dms-abe/account/toggle-favorite-product"; // đổi trạng thái thích hoặc không thích sản phẩm
    }
    public class ProfileController : SimpleController
    {
        private IProductService ProductService;
        private IStoreUserService StoreUserService;
        private IStoreUserProfileService StoreUserProfileService;
        private ICurrentContext CurrentContext;
        public ProfileController(
            IProductService ProductService,
            IStoreUserService StoreUserService,
            IStoreUserProfileService StoreUserProfileService,
            ICurrentContext CurrentContext
            )
        {
            this.StoreUserService = StoreUserService;
            this.StoreUserProfileService = StoreUserProfileService;
            this.ProductService = ProductService;
            this.CurrentContext = CurrentContext;
        }

        [AllowAnonymous]
        [Route(ProfileRoot.Login), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> Login([FromBody] StoreUser_LoginDTO StoreUser_LoginDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            StoreUser StoreUser = new StoreUser
            {
                Username = StoreUser_LoginDTO.Username,
                Password = StoreUser_LoginDTO.Password,
                BaseLanguage = "vi",
            };
            StoreUser = await StoreUserProfileService.Login(StoreUser);
            StoreUser_StoreUserDTO StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);

            if (StoreUser.IsValidated)
            {
                Response.Cookies.Append("Token", StoreUser.Token);
                StoreUser_StoreUserDTO.Token = StoreUser.Token;
                return StoreUser_StoreUserDTO;
            }
            else
                return BadRequest(StoreUser_StoreUserDTO);
        }

        [Route(ProfileRoot.Logged), HttpPost]
        public bool Logged()
        {
            return true;
        }
        [Route(ProfileRoot.ChangePassword), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> ChangePassword([FromBody] StoreUser_ProfileChangePasswordDTO StoreUser_ProfileChangePasswordDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            this.CurrentContext.UserId = ExtractUserId();
            StoreUser StoreUser = new StoreUser
            {
                Id = CurrentContext.UserId,
                Password = StoreUser_ProfileChangePasswordDTO.OldPassword,
                NewPassword = StoreUser_ProfileChangePasswordDTO.NewPassword,
            };
            StoreUser = await StoreUserProfileService.ChangePassword(StoreUser);
            StoreUser_StoreUserDTO StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return StoreUser_StoreUserDTO;
            else
                return BadRequest(StoreUser_StoreUserDTO);
        }

        #region Forgot Password
        [AllowAnonymous]
        [Route(ProfileRoot.ForgotPassword), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> ForgotPassword([FromBody] StoreUser_ForgotPassword StoreUser_ForgotPassword)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreUser StoreUser = new StoreUser
            {
                Username = StoreUser_ForgotPassword.Username,
            };

            StoreUser = await StoreUserProfileService.ForgotPassword(StoreUser);
            StoreUser_StoreUserDTO StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
            {
                return StoreUser_StoreUserDTO;
            }
            else
                return BadRequest(StoreUser_StoreUserDTO);
        }

        [AllowAnonymous]
        [Route(ProfileRoot.VerifyOtpCode), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> VerifyCode([FromBody] StoreUser_VerifyOtpDTO StoreUser_VerifyOtpDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StoreUser StoreUser = new StoreUser
            {
                Username = StoreUser_VerifyOtpDTO.Username,
                OtpCode = StoreUser_VerifyOtpDTO.OtpCode,
            };
            StoreUser = await StoreUserProfileService.VerifyOtpCode(StoreUser);
            StoreUser_StoreUserDTO StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
            {
                HttpContext.Response.Cookies.Append("Token", StoreUser.Token);
                return StoreUser_StoreUserDTO;
            }

            else
                return BadRequest(StoreUser_StoreUserDTO);
        }

        [Route(ProfileRoot.RecoveryPassword), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> RecoveryPassword([FromBody] StoreUser_RecoveryPassword StoreUser_RecoveryPassword)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var UserId = ExtractUserId();
            StoreUser StoreUser = new StoreUser
            {
                Id = UserId,
                Password = StoreUser_RecoveryPassword.Password,
            };
            StoreUser = await StoreUserProfileService.RecoveryPassword(StoreUser);
            if (StoreUser == null)
                return Unauthorized();
            StoreUser_StoreUserDTO StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);
            return StoreUser_StoreUserDTO;
        }
        #endregion

        [Route(ProfileRoot.SaveImage), HttpPost]
        public async Task<ActionResult<string>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray()
            };
            CurrentContext.Token = Request.Cookies["Token"];
            string str = await StoreUserService.SaveImage(Image);
            return str;
        }

        [Route(ProfileRoot.Get), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> GetMe()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            var UserId = ExtractUserId();
            StoreUser StoreUser = await StoreUserService.Get(UserId);
            return new StoreUser_StoreUserDTO(StoreUser);
        }

        [Route(ProfileRoot.Update), HttpPost]
        public async Task<ActionResult<StoreUser_StoreUserDTO>> UpdateMe([FromBody] StoreUser_StoreUserDTO StoreUser_StoreUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            this.CurrentContext.UserId = ExtractUserId();
            StoreUser StoreUser = ConvertDTOToEntity(StoreUser_StoreUserDTO);
            StoreUser.Id = CurrentContext.UserId;
            StoreUser = await StoreUserService.Update(StoreUser);
            StoreUser_StoreUserDTO = new StoreUser_StoreUserDTO(StoreUser);
            if (StoreUser.IsValidated)
                return StoreUser_StoreUserDTO;
            else
                return BadRequest(StoreUser_StoreUserDTO);
        }

        [Route(ProfileRoot.ToggleFavoriteProduct), HttpPost]
        public async Task<bool> ToggleFavoriteProduct([FromBody] StoreUser_FavoriteProductDTO StoreUser_FavoriteProductDTO)
        {
            long ProductId = StoreUser_FavoriteProductDTO.Id;
            bool IsFavorite = StoreUser_FavoriteProductDTO.IsFavorite;
            long StoreUserId = CurrentContext.StoreUserId;
            if (ProductId == 0)
            {
                return false;
            }
            if (StoreUserId == 0)
            {
                return false;
            }
            bool result = await StoreUserProfileService.ToggleFavoriteProduct(ProductId, StoreUserId, IsFavorite);
            return result;
        }

        private long ExtractUserId()
        {
            return long.TryParse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
        }

        private StoreUser ConvertDTOToEntity(StoreUser_StoreUserDTO StoreUser_StoreUserDTO)
        {
            StoreUser StoreUser = new StoreUser();
            StoreUser.Id = StoreUser_StoreUserDTO.Id;
            StoreUser.StoreId = StoreUser_StoreUserDTO.StoreId;
            StoreUser.Username = StoreUser_StoreUserDTO.Username;
            StoreUser.Password = StoreUser_StoreUserDTO.Password;
            StoreUser.DisplayName = StoreUser_StoreUserDTO.DisplayName;
            StoreUser.StatusId = StoreUser_StoreUserDTO.StatusId;
            StoreUser.Status = StoreUser_StoreUserDTO.Status == null ? null : new Status
            {
                Id = StoreUser_StoreUserDTO.Status.Id,
                Code = StoreUser_StoreUserDTO.Status.Code,
                Name = StoreUser_StoreUserDTO.Status.Name,
            };
            StoreUser.Store = StoreUser_StoreUserDTO.Store == null ? null : new Store
            {
                Id = StoreUser_StoreUserDTO.Store.Id,
                Code = StoreUser_StoreUserDTO.Store.Code,
                Name = StoreUser_StoreUserDTO.Store.Name,
            };
            StoreUser.BaseLanguage = CurrentContext.Language;
            return StoreUser;
        }
    }
}
