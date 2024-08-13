using AutoMapper;
using CSMSBE.Core.Enum;
using CSMSBE.Core.Helper;
using CSMSBE.Core;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.User;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMSBE.Core.Resource;
using CSMSBE.Services.Implements;
using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Core.Interfaces;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMS.Model.SecurityMatrix;
using CSMSBE.Api.PermissionAttribute;
using Microsoft.AspNetCore.Authorization;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly ISecurityMatrixService _securityMatrixService;
        private readonly ILogHistoryService _logHistoryService;
        private readonly IWorkerService _workerService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        public PermissionController(ISecurityMatrixService securityMatrixService,
           ILogHistoryService logHistoryService, IWorkerService workerService, IRoleService roleService, IMapper mapper)
        {
            _securityMatrixService = securityMatrixService;
            _logHistoryService = logHistoryService;
            _workerService = workerService;
            _roleService = roleService;
            _mapper = mapper;

        }
        [HttpGet("GetLookupAction")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.SecurityMatrix)]        
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookupAction([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var lookupActionDto = await _securityMatrixService.GetLookUpAction(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookupAction");
                return Ok(new ResponseData() { Content = lookupActionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.UnableHandleException, $"{StringMessage.ErrorMessages.ErrorProcess} {ex}")
                    }
                );
            }
        }

        [HttpGet("GetLookupScreen")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.SecurityMatrix)]
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookupScreen([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var lookupActionDto = await _securityMatrixService.GetLookUpScreen(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookupScreen");
                return Ok(new ResponseData() { Content = lookupActionDto });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.UnableHandleException, $"{StringMessage.ErrorMessages.ErrorProcess} {ex}")
                    }
                );
            }
        }
        
        [HttpGet("Filter")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.SecurityMatrix)]
        public async Task<ActionResult<ResponseItem<IPagedList<SecurityMatrixDTO>>>> GetFilteredData([FromQuery] SMFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var results = await _securityMatrixService.FilterSM(filter);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetFilteredData");
                return Ok(new ResponseData() { Content = results });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.UnableHandleException, $"{StringMessage.ErrorMessages.ErrorProcess} {ex}")
                    }
                );
            }
        }

        [HttpGet("GetSecurityMatrixDetail")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.SecurityMatrix)]
        public ActionResult GetSecurityMatrixDetail(string RoleId)
        {
            var securityMatrices = _securityMatrixService.GetDetailSecurityMatrix(RoleId);
            //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetSecurityMatrixDetail");
            return Ok(new ResponseData{ Content = securityMatrices });
        }

        [HttpPost("UpdateSecurityMatrix")]
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.SecurityMatrix)]//require admin permission
        public async Task<ActionResult> UpdateSecurityMatrix(CreateSecurityMatrixDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.DataNotValid,
                    ErrorMessage = StringMessage.ErrorMessages.DataNotValid
                });
            }
            var listIds = model.Screens.Select(entry => entry.ScreenId).ToList();
            if (listIds.Any() && listIds.Distinct().Count() != model.Screens.Count())
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorType = ErrorTypeConstant.DuplicateScreens,
                    ErrorMessage = StringMessage.ErrorMessages.DuplicateScreens
                });
            }

            var create = await _securityMatrixService.CreateSecurityMatrix(model);
            if (!create)
            {
                return BadRequest(new ResponseErrorData
                {
                    ErrorMessage = StringMessage.ErrorMessages.ErrorProcess,
                    ErrorType = ErrorTypeConstant.ErrorProcess
                });
            }
            //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateSecurityMatrix");
            return Ok(new ResponseData
            {
                Content = new
                {
                    Status = create
                }
            });
        }
        private void CreateLogHistory(int action, string description)
        {
            CurrentUserDTO currentUserModel = _workerService.GetCurrentUser();
            LogHistoryDTO logHistoryModel = new LogHistoryDTO
            {
                Action = action,
                Description = description,
                UserName = currentUserModel.FullName
            };
            _logHistoryService.Create(logHistoryModel, currentUserModel);
        }
    }
}
