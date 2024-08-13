using AutoMapper;
using CSMS.Model.DTO.IssueDTO;
using CSMS.Model.Issue;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Services.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CSMSBE.Api.PermissionAttribute;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public CommentController(ICommentService commentService, IMapper mapper, IWorkerService workerService, ISecurityMatrixProjectService securityMatrixProjectService)
        {
            _commentService = commentService;
            _mapper = mapper;
            _workerService = workerService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }
        [HttpGet("GetById/{id}")]
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<CommentDTO>>> GetById(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByCommentIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = _commentService.GetCommentById(id);
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateProject");
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
        [HttpGet("GetIssueComments/{issueId}")]
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<CommentDTO>>> GetIssueComment(int issueId)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByIssueIdAsync(userId, issueId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = await _commentService.GetIssueComments(issueId);
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "UpdateProject");
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
        [HttpPost("Create")]
        //[RequiresPermission(RoleHelper.Action.Create, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<CommentDTO>>> CreateComment(CreateCommentDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByIssueIdAsync(userId, dto.IssueId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                if (userId == null) return Unauthorized();
                var createResult = _commentService.CreateComment(dto, userId);
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
        public async Task<ActionResult<ResponseItem<CommentDTO>>> Put([FromBody] UpdateCommentDTO updateData)
        {
            try
            {
                updateData.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByCommentIdAsync(userId, updateData.Id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var updateResult = _commentService.UpdateComment(updateData, userId);
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
        public async Task<ActionResult<ResponseItem<bool>>> Delete(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByCommentIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = _commentService.RemoveComment(id);
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
