using CSMSBE.Core;
using CSMSBE.Core.Helper;
using CSMSBE.Infrastructure.Interfaces;
using CSMSBE.Core.Resource;
using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Net.WebSockets;
using System.Text;
using CSMS.Model.DTO.ProjectDTO;
using CSMSBE.Core.Interfaces;
using CSMSBE.Api.PermissionAttribute;
using CSMSBE.Core.Enum;
using CSMS.Model.User;
using CSMSBE.Services.Implements;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMS.Model.DTO.ModelDTO;
using Lucene.Net.Search;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;
        private readonly ILogHistoryService _logHistoryService;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger, ILogHistoryService logHistoryService, IWorkerService workerService
            , ISecurityMatrixProjectService securityMatrixProjectService)
        {
            _projectService = projectService;
            _logger = logger;
            _logHistoryService = logHistoryService;
            _workerService = workerService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }
        // GET: api/<ProjectController>
        [HttpGet("Filter")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<IPagedList<ProjectDTO>>>> GetFilteredData([FromQuery] ProjectFilterRequest filter)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                filter.ValidateInput();
                var results = await _projectService.FilterProject(filter, userId);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "FilterProject");
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

        // GET api/<ProjectController>/5
        [HttpGet("GetById/{id}")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<ProjectDTO>>> Get(string id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = await _projectService.GetProjectById(id);
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

        // POST api/<ProjectController>
        [HttpPost("Create")]
        //[RequiresPermission(RoleHelper.Action.Create, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<ProjectDTO>>> CreateProject(CreateProjectDTO dto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                dto.ValidateInput();
                var createResult = await _projectService.CreateProject(dto, userId);
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

        // PUT api/<ProjectController>/5
        [HttpPut("Update")]
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<ProjectDTO>>> Put([FromBody] UpdateProjectDTO updateData)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, updateData.Id, RoleHelper.ActionProjects.EditProject);
                if (!permission) return Forbid();
                updateData.ValidateInput();
                var updateResult = await _projectService.UpdateProject(updateData, userId);
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

        // DELETE api/<ProjectController>/5
        [HttpDelete("Remove/{id}")]
        //[RequiresPermission(RoleHelper.Action.Delete, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<bool>>> Delete(string id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, id, RoleHelper.ActionProjects.EditProject);
                if (!permission) return Forbid();
                var result = await _projectService.RemoveProject(id);
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
        
        [HttpGet("GetLookUpTypeProject")]
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookUpProvince([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var lookUpProjectTypeDto = await _projectService.GetLookUpProjectType(keywordDto);
                return Ok(new ResponseData() { Content = lookUpProjectTypeDto });
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
