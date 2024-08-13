using CSMS.Model.DTO.ProjectDTO;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Services.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.User;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.Role;
using AutoMapper;
using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Core.Interfaces;
using CSMSBE.Api.PermissionAttribute;
using CSMSBE.Core.Enum;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ILogHistoryService _logHistoryService;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger, IMapper mapper, IWorkerService workerService, ILogHistoryService logHistoryService)
        {
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;
            _workerService = workerService;
            _logHistoryService = logHistoryService;
        }
        [HttpGet("GetLookup")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.RoleManagement)]
        public async Task<ActionResult<ResponseItem<RoleLookupDTO>>> GetLookUpRole([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var rolesDto = await _roleService.GetLookupRole(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = _mapper.Map<List<RoleLookupDTO>>(rolesDto) });
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
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.RoleManagement)]
        public async Task<ActionResult<ResponseItem<IPagedList<RoleDTO>>>> GetFilteredData([FromQuery] RoleFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var results = await _roleService.FilterRole(filter);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "FilterRole");
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
        [HttpGet("GetById/{id}")]
        [RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.RoleManagement)]
        public ActionResult<ResponseItem<RoleDTO>> Get(string id)
        {
            try
            {
                var result = _roleService.GetRoleById(id);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetById");
                return Ok(new ResponseData() { Content = result });

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
        [HttpPost("Create")]
        [RequiresPermission(RoleHelper.Action.Create, RoleHelper.Screen.RoleManagement)]
        public ActionResult<ResponseItem<RoleDTO>> CreateRole(CreateRoleDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                dto.ValidateInput();
                var createResult = _roleService.CreateRole(dto, userId);
                //CreateLogHistory(ActionEnum.CREATE.GetHashCode(), "CreateRole");
                return Ok(new ResponseData() { Content = createResult });
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
        [RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.RoleManagement)]
        public ActionResult<ResponseItem<RoleDTO>> Put([FromBody] UpdateRoleDTO updateData)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                updateData.ValidateInput();
                var updateResult = _roleService.UpdateRole(updateData, userId);
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateRole");
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
        [HttpDelete("Remove/{id}")]
        [RequiresPermission(RoleHelper.Action.Delete, RoleHelper.Screen.RoleManagement)]
        public ActionResult<ResponseItem<bool>> Delete(string id)
        {
            try
            {
                var result = _roleService.RemoveRole(id);
                //CreateLogHistory(ActionEnum.DELETE.GetHashCode(), "RemoveRole");
                return Ok(new ResponseData() { Content = result });
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
