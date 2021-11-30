
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskPb.Services;

namespace TestTaskPb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {        

        private readonly ILogger<APIController> _logger;
        private readonly IRepository _repository;

        public APIController(
            ILogger<APIController> logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }


        [HttpGet]
        [Route("stored-cash-reqest-by-id")]
        public IActionResult GetStoredCashRequestById(string id)
        {
            try
            {
                var result = _repository.GetStoredCashRequest(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[{Date}] Unhendeled exception: {InnerException}, {message}", ex.Data, ex.InnerException, ex.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("stored-cash-reqests")]
        public IActionResult GetStoredCashRequest(string client_id, string department_adress)
        {
            try
            {
                var result = _repository.GetStoredCashRequest(client_id, department_adress);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("[{Date}] Unhendeled exception: {InnerException}, {message}", ex.Data, ex.InnerException, ex.Message);
                return BadRequest();
            }
        }
    }
}
