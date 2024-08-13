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
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.IssueDTO;
using CSMSBE.Core.Interfaces;
using CSMS.Model.DTO.ModelDTO;
using CSMS.Model.Issue;
using CSMS.Model.Model;
using CSMSBE.Api.PermissionAttribute;
using CSMS.Entity.CSMS_Entity;
using CSMSBE.Infrastructure.Implements;
using Microsoft.AspNetCore.SignalR;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public ModelController(IModelService modelService, 
            IMapper mapper, 
            IWorkerService workerService, 
            ISecurityMatrixProjectService securityMatrixProjectService
            )
        {
            _mapper = mapper;
            _workerService = workerService;
            _modelService = modelService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }
        
        [HttpGet("GetTreeModel")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<ModelDTO>>>> GetTreeModelByParentId([FromBody] TreeModelDTO dto)
        {
            try
            {
                var results = await _modelService.GetTreeModelByParentId(dto.Ids, dto.IncludeUploaded);
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
        [HttpGet("GetTreeModelByProjectId")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<ModelDTO>>>> GetTreeModelByProjectId(string projectId, bool includeUploaded)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, projectId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var results = await _modelService.GetTreeModelByProjectId(projectId, includeUploaded);
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
        [HttpGet("GetDirectChildren/{parentId}")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<ModelDTO>>>> GetFilteredChildrenData(string parentId, [FromQuery] ModelFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, parentId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var results = await _modelService.GetDirectChildrenByParentId(parentId, filter);
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
        [HttpGet("FilterModelByProjectId/{projectId}")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<ModelDTO>>>> FilterModelByProjectId(string projectId, [FromQuery] ModelFilterRequest filter)
        {
            
            try
            {
                filter.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, projectId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var results = await _modelService.FilterModelByProjectId(projectId, filter);
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
        [HttpGet("Filter")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<IPagedList<ModelDTO>>>> GetFilteredData([FromQuery] ModelFilterRequest filter)
        {
            try
            {
                /*var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, filter.ProjectID, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();*/
                var results = await _modelService.FilterModel(filter);
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
        public async Task<ActionResult<ResponseItem<ModelDTO>>> Get(string id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = _modelService.GetModelById(id);
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
        public async Task<ActionResult<ResponseItem<ModelDTO>>> CreateModel(CreateModelDTO dto)
        {
            try
            {
                dto.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, dto.ProjectID, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();

                var createResult = await _modelService.CreateModel(dto, userId);
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
        //[RequiresPermission(RoleHelper.Action.Update, RoleHelper.Screen.Model)]
        public async Task<ActionResult<ResponseItem<ModelDTO>>> Put([FromBody] UpdateModelDTO updateData)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, updateData.Id, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                var updateResult = await _modelService.UpdateModel(updateData, userId);
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
        public async Task<ActionResult<ResponseItem<bool>>> Delete(string id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, id, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                var result = await _modelService.RemoveModel(id);
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

        [HttpPost("Move")]
        //[RequiresPermission(RoleHelper.Action.Delete, RoleHelper.Screen.Project)]
        public async Task<ActionResult<ResponseItem<ModelDTO>>> MoveModel(MoveModelDTO dto)
        {
            try
            {
                dto.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByModelIdAsync(userId, dto.Id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var moveResult = await _modelService.MoveModel(dto, userId);
                //CreateLogHistory(ActionEnum.CREATE.GetHashCode(), "CreateProject");
                return Ok(new ResponseData() { Content = moveResult });
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

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadIFC([FromForm] UploadFileRequest request)
        {
            var userId = _workerService.GetCurrentUser()?.Id;
            if (userId == null) return Unauthorized();
            var permission = await _securityMatrixProjectService
                .HasPermissionByModelIdAsync(userId, request.ModelId, RoleHelper.ActionProjects.EditResource);
            if (!permission) return Forbid();

            if (request.File == null || request.File.Length == 0)
                return BadRequest("Không có file nào được upload!");

            // Kiểm tra định dạng file
            var allowedExtensions = new[] { ".ifc", ".obj", ".stl" };
            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.FileNotAllow,
                        $"{StringMessage.ErrorMessages.FileNotValid}")
                    }
                );
            }

            // Tạo tên file tạm thời với phần mở rộng .ifc
            var tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(request.File.FileName)}");
            try
            {
                // Lưu tạm file lên server
                using (var stream = new FileStream(tempFileName, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                var result = await _modelService.UploadFileIFC(request.ModelId, tempFileName, userId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    new ResponseData()
                    {
                        Content = null,
                        Err = new ResponseErrorData(ErrorTypeConstant.DataNotValid, 
                        $"{StringMessage.ErrorMessages.DataNotValid} {ex}")
                    }
                );
            }
            finally
            {
                // Xóa file tạm sau khi upload xong
                if (System.IO.File.Exists(tempFileName))
                {
                    System.IO.File.Delete(tempFileName);
                }
            }
        }

        [HttpGet("GetSpeckleModelsInfo")]
        //[RequiresPermission(RoleHelper.Action.View, RoleHelper.Screen.IssueManagment)]
        public async Task<ActionResult<ResponseItem<SpeckleModelInfoDTO>>> GetSpeckleModelsInfo([FromQuery] SpeckleModelInfoRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, filter.projectId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var results = await _modelService.GetSpeckleModelsInfo(filter);
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
    }
}
