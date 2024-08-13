using CSMSBE.Core.Enum;
using CSMSBE.Core.Helper;
using CSMSBE.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.User;
using CSMSBE.Services.Interfaces;
using CSMS.Entity.IdentityAccess;
using CSMSBE.Core.Resource;
using CSMSBE.Core.SendMailServices;
using CSMSBE.Services.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
using CSMS.Model.IdentityAccess;

using CSMSBE.Core.Interfaces;

using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.UserDTO;
using static QRCoder.PayloadGenerator;
using System.Reflection;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.Province;
using CSMS.Model;
using AutoMapper;

using CSMSBE.Api.PermissionAttribute;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogHistoryService _logHistoryService;
        private readonly IWorkerService _workerService;
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ISecurityMatrixService _securityMatrixService;

        public UserController(IUserService userService,
            IWorkerService workerService, 
            ILogHistoryService logHistoryService,
            UserManager<User> userManager,
            IRoleService roleService,
            IMapper mapper,
            ISecurityMatrixService securityMatrixService)
        {
            _userService = userService;
            _workerService = workerService;
            _logHistoryService = logHistoryService;
            _userManager = userManager;
            _roleService = roleService;
            _mapper = mapper;
            _securityMatrixService = securityMatrixService;
        }

        [HttpPost("Create")]
        [RequiresPermission(RoleHelper.Action.Create, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult> CreateUser([FromForm] CreateUserDTO model)
        {
            try
            {
                model.ValidateInput();
                if (!ModelState.IsValid && ModelState.Root.GetModelStateForProperty("DateOfBirth").ValidationState != ModelValidationState.Invalid)
                {
                    return BadRequest(new ResponseErrorData
                    {
                        ErrorType = ErrorTypeConstant.DataNotValid,
                        ErrorMessage = StringMessage.ErrorMessages.DataNotValid
                    });
                }

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = model.Email,
                    UserName = model.UserName,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    Address = model.Address,
                    CreatedDate = DateTimeOffset.Now,
                    ModifiedDate = DateTimeOffset.Now,
                    CreatedBy = "ADMIN",
                    ModifiedBy = "ADMIN",
                    Status = true,
                    Description = "",
                    DateOfBirth = model.DateOfBirth,
                    UserType = Constant.UserType.REGISTERUSER,
                    SecretKey = "CSMS"
                };
                var userExits = await _userManager.FindByEmailAsync(model.Email);
                if (userExits != null)
                    return BadRequest(
                    new ResponseErrorData
                    {
                        ErrorType = ErrorTypeConstant.EmailAlreadyExits,
                        ErrorMessage = StringMessage.ErrorMessages.EmailAlreadyExits
                    });

                //default password
                string password = "123@123aA";

                var create = await _userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                {
                    return BadRequest(new ResponseErrorData
                    {
                        ErrorType = ErrorTypeConstant.UserAlreadyExits,
                        ErrorMessage = StringMessage.ErrorMessages.UserAlreadyExits
                    });
                }
                string roleId = null;
                string roleName = null;
                if (!string.IsNullOrWhiteSpace(model.RoleId))
                {
                    var role = _roleService.GetById(model.RoleId);
                    if (role != null)
                    {
                        roleId = role.Id;
                        roleName = role.Code;
                        _ = await _userManager.AddToRoleAsync(user, role.Code);
                    }
                }
                var screenData = _securityMatrixService.GetDetailSecurityMatrix(model.RoleId);
                // Add claim
                _ = await _userManager.AddClaimAsync(user, new Claim(Constant.RecordPermission, Constant.SeeOnlyMineRecord));

                // Create log history
                //CreateLogHistory(ActionEnum.CREATE.GetHashCode(), "CreateUser");

                var result = new UserResultDTO
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DateOfBirth = user.DateOfBirth,
                    Status = user.Status,
                    Gender = user.Gender,
                    Address = user.Address,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate,
                    CreatedBy = user.CreatedBy,
                    ModifiedBy = user.ModifiedBy,
                    Description = user.Description,
                    UserType = user.UserType,
                    RoleId = roleId,
                    RoleName = roleName,
                    Screens = screenData
                };

                return Ok(new ResponseData { Content = result });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                    }
                );
            }
            
        }
        [HttpGet("GetLookup")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult<ResponseItem<IList<UserLookup>>>> GetLookUpUser([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var userLookupDto = await _userService.GetLookUpUsers(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookupUsers");
                return Ok(new ResponseData() { Content = userLookupDto });
            }
            catch (Exception ex)
            {
                return BadRequest(
                     new ResponseData()
                     {
                         Content = null,
                         Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                     }
                 );
            }
        }

        [HttpGet("Filter")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult<ResponseItem<IPagedList<UserListViewDTO>>>> GetFilteredData([FromQuery] UserFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var results = await _userService.FilterUsers(filter);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "FilterUser");
                return Ok(new ResponseData() { Content = results });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                    }
                );
            }
        }

        [HttpGet("GetUserDetail")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.UserManagement)]
        public ActionResult GetUserDetail(string id)
        {
            try
            {
                var data = _userService.GetUserDetail(id);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetUserDetail");
                return Ok(new ResponseData() { Content = data });
            }
            catch (Exception ex)
            {
                return BadRequest(
                   new ResponseData()
                   {
                       Content = null,
                       Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                   }
               );
            }            
        }

        [HttpPut("Update")]
        [RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult<UserDTO>> UpdateUser([FromForm] UpdateUserDTO model)
        {
            try
            {
                model.ValidateInput();
                var updateResult = _userService.UpdateUser(model);
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateUser");
                return Ok(new ResponseData() { Content = updateResult });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                    }
                );
            }
        }

        [HttpPost("Active")]
        [RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult> ActiveUser(string id)
        {
            var user = _userService.GetByUserid(id);

            user.Status = true;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.ErrorProcess,
                    ErrorMessage = StringMessage.ErrorMessages.ErrorProcess
                });
            }
            /*var emailTemplate = _emailTemplateService.GetEmailTemplateByCode(Constant.EmailTemplate.ACTIVEUSER);

            var contentMail = emailTemplate.Description
                .Replace("${LOGIN}", _appSettings.UrlLogin);

            var returnMessage = string.Empty;
            var sendMail = _sendMailService.SendToEmail(user.Email, null, "Active User Successfully", contentMail,
                ref returnMessage);
            var dataGenerate = new CreateEmailGeneratedDto
            {
                ErrorMessage = returnMessage,
                CC = emailTemplate.CC,
                Description = contentMail,
                EmailType = emailTemplate.Code,
                ReferenceNumber = 0,
                ReferenceTypeId = ReferenceTypeEnum.ActiveUser.GetHashCode(),
                SendTo = user.Email,
                SentDate = DateTime.Now,
                Status = sendMail ? EmailGenerateStatusEnum.Success.GetHashCode() : EmailGenerateStatusEnum.Failure.GetHashCode(),
                Subject = emailTemplate.Title,
                Title = emailTemplate.Title,
                SendFrom = _emailConfiguration.EmailAdmin
            };
            _emailGeneratedService.Create(dataGenerate);*/
            //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "ActiveUser");
            return Ok(new ResponseData() { Content = result });
        }

        [HttpPost("DeActive")]
        [RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult> DeActiveUser(string id)
        {
            try
            {
                var user = _userService.GetByUserid(id);

                user.Status = false;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResponseErrorData
                    {
                        ErrorType = ErrorTypeConstant.ErrorProcess,
                        ErrorMessage = StringMessage.ErrorMessages.ErrorProcess
                    });
                }
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "DeActiveUser");
                return Ok(new ResponseData() { Content = result });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                    }
                );
            }
            
        }
      
        [HttpPost("ResetPassword")]
        [RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.UserManagement)]
        public async Task<ActionResult> ResetPassword(ResetPasswordAdminDTO model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(model);
            }

            var query = _userService.GetByUserid(model.UserId);
            var user = await _userManager.FindByEmailAsync(query.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.AccountNotValid,
                    ErrorMessage = StringMessage.ErrorMessages.AccountNotValid
                });
            }
            /*var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            var emailChangedPassword = _emailTemplateService.GetEmailTemplateByCode(Constant.EmailTemplate.ChangedPassword);
            var returnMessage = string.Empty;
            var contentMail = emailChangedPassword.Description
            .Replace("${EMAIL}", user.Email)
            .Replace("${PASSWORD}", model.Password);
            var sendMail = _sendMailService.SendToEmail(user.Email, null, emailChangedPassword.Title, contentMail, ref returnMessage);
            var dataGenerate = new CreateEmailGeneratedDto
            {
                ErrorMessage = returnMessage,
                CC = emailChangedPassword.CC,
                Description = contentMail,
                EmailType = emailChangedPassword.Code,
                ReferenceNumber = 0,
                ReferenceTypeId = ReferenceTypeEnum.ChangedPassword.GetHashCode(),
                SendTo = user.Email,
                SentDate = DateTime.Now,
                Status = sendMail ? EmailGenerateStatusEnum.Success.GetHashCode() : EmailGenerateStatusEnum.Failure.GetHashCode(),
                Subject = "Forgot Password",
                Title = emailChangedPassword.Title,
                SendFrom = _emailConfiguration.EmailAdmin
            };
            _emailGeneratedService.Create(dataGenerate);
            
            if (!sendMail)
                return Ok(new ResponseErrorData()
                {
                    ErrorType = ErrorTypeConstant.SendMailError,
                    ErrorMessage = StringMessage.ErrorMessages.SendMailError
                });*/
            //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "ResetPassword");
            return Ok(new ResponseData{Content = new { Status = true } });
        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult> UploadImage([FromForm] UploadAvatarDTO dto)
        {
            if (dto.Avatar == null || dto.Avatar.Length == 0) return BadRequest("Không có tệp nào được chọn.");
            if (dto.Avatar != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(dto.Avatar.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Định dạng File không đúng. Các định dạng File hình ảnh: JPG, JPEG, PNG, GIF.");
                }
            }
            try
            {
                var result = await _userService.UploadAvatar(dto);
                return Ok(new ResponseData() { Content = result });
            }
            catch (Exception ex)
            {
                return BadRequest(
                     new ResponseData()
                     {
                         Content = null,
                         Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                     }
                 );
            }
        }

        private void CreateLogHistory(int action, string description)
        {
            CurrentUserDTO currentUserModel = _workerService.GetCurrentUser();
            LogHistoryDTO logHistoryModel = new LogHistoryDTO
            {
                Action = action,
                Description = description,
                UserName = currentUserModel?.FullName
            };
            _logHistoryService.Create(logHistoryModel, currentUserModel);
        }
    }
}
