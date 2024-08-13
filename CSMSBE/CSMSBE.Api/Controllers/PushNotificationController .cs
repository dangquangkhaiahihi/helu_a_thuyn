using CSMS.Model.DTO.BaseFilterRequest;
using CSMS.Model;
using CSMSBE.Core.Helper;
using CSMSBE.Core.Resource;
using CSMSBE.Core;
using CSMSBE.Services.Implements;
using CSMSBE.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        private readonly IPushNotificationService _pushNotificationService;

        public PushNotificationController(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        [HttpGet("GetListNotifications/{userId}")]
        public async Task<IActionResult> GetListNotificationsByUserId(string userId)
        {
            try
            {
                var result = await _pushNotificationService.GetListNotificationsByUserId(userId);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
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

        [HttpGet("GetNotificationById/{id:guid}")]
        public async Task<IActionResult> GetNotificationById(Guid id)
        {
            try
            {
                var result = await _pushNotificationService.GetNotificationById(id);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
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

        [HttpPut("MarkAsRead/{id:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid id)
        {
            try
            {
                var result = await _pushNotificationService.MarkAsRead(id);
                //CreateLogHistory(ActionEnum.VIEW.GetHashCode(), "GetLookUpRole");
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
