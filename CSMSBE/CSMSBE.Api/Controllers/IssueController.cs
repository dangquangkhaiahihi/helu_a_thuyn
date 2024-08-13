using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.Role;
using CSMSBE.Api.PermissionAttribute;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMSBE.Services.Interfaces;
using CSMS.Model;
using AutoMapper;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.SecurityMatrixDTO;
using CSMSBE.Core.Interfaces;
using CSMSBE.Services.Implements;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.DTO.ProjectDTO;
using CSMS.Model.Issue;
using System.Linq;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public IssueController(IIssueService issueService, IMapper mapper, IWorkerService workerService, ISecurityMatrixProjectService securityMatrixProjectService)
        {
            _issueService = issueService;
            _mapper = mapper;
            _workerService = workerService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }

        [HttpGet("Filter")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<IssueDTO>>>> GetFilteredData([FromQuery] IssueFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var results = await _issueService.FilterIssue(filter);
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
        [HttpGet("GetById/{id}")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<IssueDTO>>> Get(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByIssueIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = _issueService.GetIssueById(id);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetByIdProject");
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
        //[RequiresPermission(RoleHelper.Action.Create, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<IssueDTO>>> CreateIssue([FromForm] CreateIssueDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, dto.ModelId, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                if (dto.File != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(dto.File.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Định dạng File không đúng. Các định dạng File hình ảnh: JPG, JPEG, PNG, GIF.");
                    }
                }
                dto.ValidateInput();
                var createResult = _issueService.CreateIssue(dto, userId);
                //CreateLogHistory(ActionEnum.CREATE.GetHashCode(), "CreateProject");
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
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<IssueDTO>>> Put([FromForm] UpdateIssueDTO updateData)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByIssueIdAsync(userId, updateData.Id, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                if (updateData.File != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(updateData.File.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return BadRequest("Định dạng File không đúng. Các định dạng File hình ảnh: JPG, JPEG, PNG, GIF.");
                    }
                }
                updateData.ValidateInput();
                var updateResult = _issueService.UpdateIssue(updateData, userId);
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateProject");
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
        //[RequiresPermission(RoleHelper.Action.Delete, RoleHelper.Screen.Project)]
        public async Task <ActionResult<ResponseItem<bool>>> Delete(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByIssueIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = _issueService.RemoveIssue(id);
                //CreateLogHistory(ActionEnum.DELETE.GetHashCode(), "RemoveProject");
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
