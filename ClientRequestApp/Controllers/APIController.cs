using EventBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ClientRequestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APIController : ControllerBase
    {

        private readonly ILogger<APIController> _logger;
        private readonly IEventBus _eventBus;

        public APIController(
            ILogger<APIController> logger,
            IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        /// <summary>
        /// Post client`s requests to get cash in some department.
        /// </summary>
        /// <param name="request"></param>
        [Route("client-cash-request/")]
        [HttpPost]
        public IActionResult Post(Request request)
        {
            try
            {
                //Checking that model is valid
                if (!ModelState.IsValid) return BadRequest();   

                //Log model 
                var info = JsonConvert.SerializeObject(request);
                _logger.LogInformation("[{Date}] Incoming request: {info}", DateTime.Now.ToString(), info);

                //Set client Ip to request
                FullRequest fullRequest = new FullRequest(request);
                fullRequest.IP = HttpContext.Connection.RemoteIpAddress?.ToString();

                //Send msg to rebbit
                var message = JsonConvert.SerializeObject(fullRequest);
                _eventBus.Publish(message);
                return Ok();
            }
            catch (Exception ex)
            {                
                _logger.LogInformation("[{Date}] Unhendeled exception: {InnerException}, {message}", ex.Data, ex.InnerException, ex.Message); 
                return BadRequest();                
            }
        }
    }
}
