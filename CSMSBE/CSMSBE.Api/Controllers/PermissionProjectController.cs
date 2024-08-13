using AutoMapper;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Services.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.DTO.SecurityMatrixProjectDTO;
using CSMS.Model.Role;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Core.Interfaces;
using CSMS.Entity.CSMS_Entity;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public PermissionProjectController(IMapper mapper, IWorkerService workerService, ISecurityMatrixProjectService securityMatrixProjectService)
        {
            _mapper = mapper;
            _workerService = workerService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }
        [HttpGet("GetUsersInProject/{projectId}")]
        public async Task<IActionResult> GetUsersWithRoles(string projectId)
        {
            try
            {
                var usersWithRoles = await _securityMatrixProjectService.GetUsersWithRolesByProjectIdAsync(projectId);
                return Ok(new ResponseData() { Content = usersWithRoles });
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
        [HttpGet("FilterRoleByProjectId/{projectId}")]
        public async Task<ActionResult<ResponseItem<IPagedList<RoleProjectDTO>>>> FilterRoleByProjectId(string projectId, [FromQuery] RoleProjectFilterRequest filter)
        {

            try
            {
                var results = await _securityMatrixProjectService.FilterRoleByProjectId(projectId, filter);
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
        [HttpGet("GetLookupRoleByProject")]
        public async Task<ActionResult<ResponseItem<LookupDTO>>> GetLookupRoleByProjectAsync([FromQuery] KeywordDto? keywordDto, string projectId)
        {
            try
            {
                var results = await _securityMatrixProjectService.GetLookupRoleByProject(keywordDto, projectId);
                var lookupDTO = await Task.Run(() => results.AsParallel().Select(dto => _mapper.Map<LookupDTO>(dto)).ToList());
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookupProject");
                return Ok(new ResponseData() { Content = lookupDTO });
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
        [HttpGet("GetLookupAction")]
        public async Task<ActionResult<ResponseItem<LookupDTO>>> GetLookupAction([FromQuery] KeywordDto? keywordDto)
        {

            try
            {
                var roleDto = await _securityMatrixProjectService.GetLookupAction(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = _mapper.Map<List<LookupDTO>>(roleDto) });
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

        [HttpGet("GetListProjectByUserId")]
        public async Task<ActionResult<ResponseItem<LookupDTO>>> GetListProjectByUserId([FromQuery] KeywordDto? keywordDto)
        {

            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(userId.ToString())) return Unauthorized();

                var projectDto = await _securityMatrixProjectService.GetLookupProject(keywordDto, userId.ToString());
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = _mapper.Map<List<LookupDTO>>(projectDto) });
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
        [HttpPost("InviteUser")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<AssignUserDTO>>> InviteUser(AssignUserDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(userId.ToString())) return Unauthorized();
                var assignUser = await _securityMatrixProjectService.AssignUser(dto, userId.ToString());
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = assignUser });
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
        [HttpPost("AcceptOrRejectInvitation/{projectId}")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult> AcceptOrRejectInvitation([FromRoute] string projectId, [FromQuery] bool isApproved)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(userId.ToString())) return Unauthorized();
                var updateResult = await _securityMatrixProjectService.UpdateProjectApproval(projectId, userId.ToString(), isApproved);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = updateResult });
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
        [HttpPost("UpdateUserRole")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<IActionResult> SetRoleForUser(UpdateUserRoleDTO dto)
        {

            try
            {
                /*var currentUserId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(userId)) return Unauthorized();*/

                var assignRoleForUser = await _securityMatrixProjectService.UpdateUserRole(dto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
                return Ok(new ResponseData() { Content = assignRoleForUser });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.UnableHandleException, $"{ex.Message}")
                    }
                );
            }
        }
        [HttpPost("CreateOrUpdateRole")]
        public async Task<IActionResult> CreateOrUpdateRoleWithActions(CreateUpdateRoleProjectDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var result = await _securityMatrixProjectService.CreateOrUpdateRoleWithActions(dto, userId);
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

        [HttpDelete("RemoveUserFromProject")]
        public async Task<IActionResult> RemoveUserFromProject(string userId, string projectId)
        {
            try
            {
                var currentUserId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(currentUserId.ToString())) return Unauthorized();

                var result = await _securityMatrixProjectService.RemoveUserProject(userId, projectId);
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

        [HttpDelete("RemoveRole")]
        public async Task<IActionResult> RemoveRole(string roleId)
        {
            try
            {
                var currentUserId = _workerService.GetCurrentUser()?.Id;
                if (string.IsNullOrEmpty(currentUserId.ToString())) return Unauthorized();
                var result = await _securityMatrixProjectService.RemoveRole(roleId);
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
    }
}
