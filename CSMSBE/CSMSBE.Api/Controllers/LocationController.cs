using CSMSBE.Core;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Interfaces;
using CSMSBE.Core.Resource;
using CSMS.Model.DTO.FilterRequest;
using CSMS.Model.DTO.SomeTableDTO;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Net.WebSockets;
using System.Text;
using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model.DTO.Province;
using CSMS.Model.DTO.District;
using AutoMapper;
using CSMS.Model;
using CSMS.Model.User;
using CSMSBE.Core.Enum;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationController> _logger;
        private readonly IMapper _mapper;
        private readonly IWorkerService _workerService;
        private readonly ILogHistoryService _logHistoryService;
        public LocationController(ILocationService locationService, ILogger<LocationController> logger, IMapper mapper, IWorkerService workerService, ILogHistoryService logHistoryService)
        {
            _locationService = locationService;
            _logger = logger;
            _mapper = mapper;
            _workerService = workerService;
            _logHistoryService = logHistoryService;
        }
        // GET: api/<SomeTableController>
        [HttpPost("SyncData")]
        public async Task<ActionResult<ResponseItem<string>>> SyncData()
        {
            try
            {
                await _locationService.SyncVNDataLocation();
                //CreateLogHistory(ActionEnum.UPDATE.GetHashCode(), "SyncData");
                return Ok(new ResponseData() { Content = "Đồng bộ dữ liệu thành phố, tỉnh, quận, huyện, phường, xã thành công" });
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

        [HttpGet("GetLookUpProvince")]
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookUpProvince([FromQuery]KeywordDto? keywordDto)
        {
            try
            {
                var lookupProvincesDto= await _locationService.GetLookUpProvince(keywordDto);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpProvince");
                return Ok(new ResponseData() { Content = lookupProvincesDto });
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

        [HttpGet("GetLookUpDistrict")]
        public async Task<ActionResult<ResponseItem<IList<LookupDTO>>>> GetLookUpDistrict([FromQuery] KeywordDto? keywordDto, [FromQuery]int provinceId)
        {
            try
            {
                var lookupDistrictsDto = await _locationService.GetLookUpDistrict(keywordDto, provinceId);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpDistrict");
                return Ok(new ResponseData() { Content = lookupDistrictsDto });
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

        [HttpGet("GetLookUpCommune")]
        public async Task<ActionResult<ResponseItem<LookupDTO>>> GetLookUpCommune([FromQuery] KeywordDto? keywordDto, [FromQuery]int districtId)
        {
            try
            {
                var lookupCommunesDto = await _locationService.GetLookupCommune(keywordDto, districtId);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpCommune");
                return Ok(new ResponseData() { Content = lookupCommunesDto });
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
