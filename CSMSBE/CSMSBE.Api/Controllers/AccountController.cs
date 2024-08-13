using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Core.Settings;
using CSMSBE.Services;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.IdentityAccess;
using CSMSBE.Services.Implements;
using CSMS.Entity;
using CSMSBE.Core.SendMailServices;
using CSMS.Entity.IdentityAccess;
using Hangfire.Common;
using Lucene.Net.Support;
using System.Runtime.CompilerServices;
using CSMS.Model;
using CSMS.Entity.IdentityExtensions.IdentityMapping;
using Microsoft.AspNetCore.Authorization;
using CSMS.Model.User;
using System.Net.Http.Headers;
using CSMSBE.Core.Enum;
using System.Security.Claims;
using System.Net;
using System.Security.Permissions;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHelper _webHelper;
        private readonly IWorkerService _workerService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILogHistoryService _logHistoryService;
        public AccountController
            (
            IAccountService accountService,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IWebHelper webHelper,
            IWorkerService workerService,
            IUserService userService,
            IRoleService roleService,
            ILogHistoryService logHistoryService
            )
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHelper = webHelper;
            _workerService = workerService;
            _userService = userService;
            _roleService = roleService;
            _logHistoryService = logHistoryService;
        }
        
            
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.DataNotValid,
                    ErrorMessage = StringMessage.ErrorMessages.DataNotValid,
                });
            }
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    return BadRequest(new ResponseErrorData
                    {
                        ErrorType = ErrorTypeConstant.EmailNotRegister,
                        ErrorMessage = string.Format(StringMessage.ErrorMessages.EmailNotRegister, model.Email),
                    });
                }
            }


            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorMessage = StringMessage.ErrorMessages.PasswordNotValid,
                    ErrorType = ErrorTypeConstant.PasswordNotValid,
                });
            }

            if (!user.Status)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.AccountNotActive,
                    ErrorMessage = StringMessage.ErrorMessages.AccountNotActive,
                });
            }

            ///get token to login
            var token = await _accountService.LogInAsync(model.Email);
            ///save to log table
            await _accountService.InsertUserLoginLog(new UserApiLogRequest
            {
                UserId = user.Id,
                UserName = user.UserName,
                ApiName = Constant.APIName.Login,
                FullName = user.FullName,
                IpAddress = model.uUid,
                CreateDate = DateTime.Now
            });

            //create refreshtoken
            var refreshToken = _accountService.GenerateRefreshToken();

            await _accountService.InsertRefreshTokens(user.UserName, refreshToken, model.uUid);

            if (string.IsNullOrWhiteSpace(token))
                return BadRequest(
                    new ResponseErrorData()
                    {
                        ErrorType = ErrorTypeConstant.AccountNotValid,
                        ErrorMessage = StringMessage.ErrorMessages.AccountNotValid
                    });

            return Ok(new ResponseData
            {
                Content = new
                {
                    Token = token,
                    ReturnUrl = model.ReturnUrl,
                    RefreshToken = refreshToken
                },
            });
        }
      
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken([FromBody] ExchangeRefreshTokenRequest request)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            ///Get device manage
            ///just using GetCurrentDevice when use Authorize
            var deviceType = HttpContext.GetCurrentDevice();
            if (string.IsNullOrEmpty(deviceType))
                deviceType = Constant.DeviceDefault;

            //var RemoteIpAddress = _webHelper.GetCurrentIpAddress();

            var refreshAccessToken = await _accountService.ExchangeRefreshToken(
                new ExchangeRefreshTokenRequest() {RefreshToken = request.RefreshToken },
                deviceType,
                request.uUid
                );

            return StatusCode(StatusCodes.Status200OK, refreshAccessToken);
        }

        [HttpPost("LogoutAll")]
        [AllowAnonymous]
        public async Task<ActionResult> LogoutAll(string userName)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            try
            {
                await _accountService.DeleteRefreshTokens(userName);
                return StatusCode(StatusCodes.Status200OK, StringMessage.SuccessMessage.DeleteRefreshToken);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = $"{ErrorTypeConstant.DataNotValid} {ex}",
                    ErrorMessage = StringMessage.ErrorMessages.ErrorProcess,
                });
            }
        }
        [HttpGet("GetMyInfo")]
        public async Task<ActionResult> GetMyInfo()
        {
            var userId = _workerService.GetCurrentUser()?.Id;
            if (userId == null) return Unauthorized();
            ///check membership neu het han se downgrade thanh free
            var user = _userService.GetByUserid(userId);

            var data = new DetailUserCmsModel
            {
                UserName = user.UserName,
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                RoleName = _roleService.GetRoleNameById(user.UserRoles.Select(x => x.RoleId).FirstOrDefault()),
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
            };
            return Ok(new ResponseData
            {
                Content = data
            });
        }
        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(/*ForgotPasswordDTO resetPasswordDTO*/)
        {            
            return Ok(
                new ResponseData
                {
                    Content = new
                    {
                        Status = true
                    }
                });
        }
       
        [HttpPost("UpdateUserAccount")]
        public async Task<ActionResult> UpdateUserAccount([FromForm] UpdateUserAccountDTO model)
        {
            var userId = _workerService.GetCurrentUser()?.Id;
            if (userId == null)  return Unauthorized();
            var user = _userService.GetByUserid(userId);
           
            user.ModifiedBy = user.FullName;
            user.ModifiedDate = DateTime.Now;
            user.FullName = model.FullName;
            if (model.DateOfBirth.HasValue)
                user.DateOfBirth = model.DateOfBirth;

            if (model.Gender.HasValue)
                user.Gender = model.Gender;
            user.Address = model.Address;
            user.PhoneNumber = model.PhoneNumber;

            var isUpdated = await _userManager.UpdateAsync(user);
            if (!isUpdated.Succeeded)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorMessage = StringMessage.ErrorMessages.ErrorProcess,
                    ErrorType = ErrorTypeConstant.ErrorProcess
                });
            }
            return Ok(new ResponseData
            {
                Content = new
                {
                    Status = true
                }
            });
        }

        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var userId = _workerService.GetCurrentUser()?.Id;
            if (userId == null) return Unauthorized();
            if (!model.ConfirmNewPassword.Equals(model.NewPassword)) 
                return BadRequest("Xác nhận mật khẩu mới không trùng khớp mật khẩu mới");
            var user = _userService.GetByUserid(userId);
            IdentityResult identity = null;
            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.CurrentPassword))
            {
                identity = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            }

            if (identity == null || !identity.Succeeded)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorMessage = identity.Errors.Select(x => x.Description).FirstOrDefault(),
                    ErrorType = ErrorTypeConstant.ErrorProcess
                });
            }

            return Ok(new ResponseData
            {
                Content = new
                {
                    Status = true
                }
            });
        }
        

        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout(string refreshToken, string uUid)
        {
            try
            {
                var user = _workerService.GetCurrentUser();
                if (user == null)
                {
                    return Unauthorized();
                }
                ///Get device manage
                var accessToken = HttpContext.GetCurrentToken();
                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken)) return Unauthorized();
                await _accountService.LogOut(user, refreshToken, uUid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = $"{ErrorTypeConstant.DataNotValid} {ex}",
                    ErrorMessage = StringMessage.ErrorMessages.ErrorProcess,
                });
            }
        }
    }
}
