using CSMS.Model.DTO.FilterRequest;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CSMS.Model.DTO.Document;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMSBE.Services.Implements;
using Microsoft.AspNetCore.Authorization;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly ILogger<DocumentController> _logger;
        private readonly string[] _fileExtensionsAllowed;
        private readonly IWorkerService _workerService;
        private readonly ISecurityMatrixProjectService _securityMatrixProjectService;
        public DocumentController(IDocumentService DocumentService, ILogger<DocumentController> logger,
            string[] fileExtensionsAllowed, IWorkerService workerService
            , ISecurityMatrixProjectService securityMatrixProjectService)
        {
            _documentService = DocumentService;
            _logger = logger;
            _fileExtensionsAllowed = fileExtensionsAllowed;
            _workerService = workerService;
            _securityMatrixProjectService = securityMatrixProjectService;
        }
        // GET: api/<DocumentController>

        // GET api/<DocumentController>/5
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> Get(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = await _documentService.GetDocumentById(id);
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

        // POST api/<DocumentController>
        [HttpPost("Create")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> CreateDocument([FromForm] DocumentCreateDto dto)
        {
            try
            {
                dto.ValidateInput();
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, dto.ProjectId, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                var isDocumentAllowed = await _documentService.IsDocumentFileAllowed(dto);
                if(!isDocumentAllowed)
                {
                    throw new Exception("Vui lòng kiểm tra lại dữ liệu tạo document");
                }                                        
                var createResult = await _documentService.CreateDocument(dto, userId);
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

        [HttpGet("GetByParentId")]
        public async Task<ActionResult<ResponseItem<List<DocumentDto>>>> GetByParentId(int id)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, id, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var list = await _documentService.GetByParentId(id);
                return Ok(new ResponseData() { Content = list });
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
        [HttpGet("GetInit")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> GetInitDocumentTree(string projectId)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, projectId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = await _documentService.GetInitDocumentTree(projectId);
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
        [HttpPut("Review")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> ReviewDocument(int id, string status)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, id, RoleHelper.ActionProjects.ReviewResource);
                if (!permission) return Forbid();
                var result = await _documentService.ReviewDocument(id, status, userId);
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
        [HttpGet("DocumentTreeByProjectId")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> GetDocumentTreeByProjectId(string projectId)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByProjectIdAsync(userId, projectId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var result = await _documentService.GetAllDocumentByProjectId(projectId);
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

        [HttpGet("Preview")]
        public async Task<ActionResult> Preview(string filePath)
        {
            try
            {
                var fileResult = await _documentService.PreviewDocument(filePath);
                return fileResult;
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

        [HttpGet("Download/{documentId}")]
        public async Task<ActionResult> Download(int documentId)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, documentId, RoleHelper.ActionProjects.ViewResource);
                if (!permission) return Forbid();
                var fileResult = await _documentService.DownloadFile(documentId);
                return fileResult;
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

        // PUT api/<DocumentController>/5
        [HttpPut("Update")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> Update([FromBody] DocumentUpdateDto updateData)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, updateData.Id, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                updateData.ValidateInput();
                var updateResult = await _documentService.UpdateDocument(updateData, userId);
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

        // DELETE api/<DocumentController>/5
        [HttpDelete("Remove")]
        public async Task<ActionResult<ResponseItem<string>>> Delete([FromBody] int[] ids)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                foreach (var item in ids)
                {
                    var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, item, RoleHelper.ActionProjects.ViewResource);
                    if (!permission) return Forbid();
                }
                await _documentService.RemoveDocuments(ids);
                return Ok(new ResponseData() { Content = "Xóa thành công!" });
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

        [HttpPut("Move")]
        public async Task<ActionResult<ResponseItem<DocumentDto>>> Move([FromBody] DocumentMoveDto moveDto)
        {
            try
            {
                var userId = _workerService.GetCurrentUser()?.Id;
                if (userId == null) return Unauthorized();
                var permission = await _securityMatrixProjectService
                    .HasPermissionByDucumentIdAsync(userId, moveDto.DocumentId, RoleHelper.ActionProjects.EditResource);
                if (!permission) return Forbid();
                moveDto.ValidateInput();
                var moveResult = await _documentService.MoveDocument(moveDto.DocumentId, moveDto.DestinationId);
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

        [HttpGet("GetLookUpFileExtension")]
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookUpFileExtension([FromQuery] KeywordDto? keywordDto)
        {
            try
            {
                var lookupFileExtensionDto = await _documentService.GetLookUpFileExtension(keywordDto);
                return Ok(new ResponseData() { Content = lookupFileExtensionDto });
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
