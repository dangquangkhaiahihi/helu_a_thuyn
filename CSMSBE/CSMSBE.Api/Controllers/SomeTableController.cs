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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CSMSBE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeTableController : ControllerBase
    {
        private readonly ISomeTableService _someTableService;
        private readonly ILogger<SomeTableController> _logger;
        public SomeTableController(ISomeTableService someTableService, ILogger<SomeTableController> logger)
        {
            _someTableService = someTableService;
            _logger = logger;
        }
        // GET: api/<SomeTableController>
        [HttpGet("GetLookup")]
        public ActionResult<ResponseItem<IEnumerable<SomeTableDTO>>> GetAllData()
        {
            try
            {
                var results = _someTableService.GetLookupSomeTable();
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
        public async Task<ActionResult<ResponseItem<IPagedList<SomeTableDTO>>>> GetFilteredData([FromQuery] SomeTableFilterRequest filter)
        {
            try
            {
                filter.ValidateInput();
                var results = await _someTableService.FilterSomeTable(filter);
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

        // GET api/<SomeTableController>/5
        [HttpGet("GetById/{id}")]
        public ActionResult<ResponseItem<SomeTableDTO>> Get(int id)
        {
            try
            {
                var result = _someTableService.GetSomeTableById(id);
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

        // POST api/<SomeTableController>
        [HttpPost("Create")]
        public ActionResult<ResponseItem<SomeTableDTO>> CreateSomeTable(CreateSomeTableDTO dto)
        {
            try
            {
                dto.ValidateInput();
                var createResult = _someTableService.CreateSomeTable(dto);
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

        // PUT api/<SomeTableController>/5
        [HttpPut("Update")]
        public ActionResult<ResponseItem<SomeTableDTO>> Put([FromBody] UpdateSomeTableDTO updateData)
        {
            try
            {
                updateData.ValidateInput();
                //var updateResult = _someTableService.UpdateSomeTable(updateData);
                return Ok(new ResponseData() { Content = updateData });
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

        // DELETE api/<SomeTableController>/5
        [HttpDelete("Remove/{id}")]
        public ActionResult<ResponseItem<bool>> Delete(int id)
        {
            try
            {
                var result = _someTableService.RemoveSomeTable(id);
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
