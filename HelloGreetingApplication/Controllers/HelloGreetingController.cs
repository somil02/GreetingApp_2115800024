using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using BusinessLayer.Service;
using BusinessLayer.Interface;
using NLog;
using RepositoryLayer.Service;

namespace GreetingApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        /// <summary>
        /// Retrieves a welcome greeting from the API.
        /// </summary>
        /// <returns>A ResponseModel containing a welcome message and current date.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var data = new
            {
                Greeting = "Hello! Welcome to the API",
                Date = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Request successful",
                Data = data
            };
            return Ok(response);
        }

        /// <summary>
        /// Creates a personalized greeting based on the provided request data.
        /// </summary>
        /// <param name="request">The RequestModel containing first name, last name, and email.</param>
        /// <returns>A ResponseModel with a personalized greeting, email, and creation timestamp.</returns>
        [HttpPost]
        public IActionResult Post([FromBody] RequestModel request)
        {
            var data = new
            {
                Greeting = $"Hello {request.FirstName} {request.LastName}",
                Email = request.Email,
                ReceivedAt = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting created",
                Data = data
            };
            return Ok(response);
        }

        /// <summary>
        /// Updates a greeting with new user information.
        /// </summary>
        /// <param name="request">The RequestModel containing updated first name, last name, and email.</param>
        /// <returns>A ResponseModel with the updated full name, email, and update timestamp.</returns>
        [HttpPut]
        public IActionResult Put([FromBody] RequestModel request)
        {
            var data = new
            {
                FullName = $"{request.FirstName} {request.LastName}",
                Email = request.Email,
                UpdatedAt = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting updated",
                Data = data
            };
            return Ok(response);
        }

        /// <summary>
        /// Partially updates a greeting with the provided fields.
        /// </summary>
        /// <param name="request">The RequestModel with optional fields to update (first name, last name, or email).</param>
        /// <returns>A ResponseModel showing which fields were updated and the update timestamp.</returns>
        [HttpPatch]
        public IActionResult Patch([FromBody] RequestModel request)
        {
            var data = new
            {
                UpdatedFields = new
                {
                    FirstName = string.IsNullOrEmpty(request.FirstName) ? "Not updated" : request.FirstName,
                    LastName = string.IsNullOrEmpty(request.LastName) ? "Not updated" : request.LastName,
                    Email = string.IsNullOrEmpty(request.Email) ? "Not updated" : request.Email
                },
                UpdatedAt = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting partially updated",
                Data = data
            };
            return Ok(response);
        }

        /// <summary>
        /// Deletes a greeting.
        /// </summary>
        /// <returns>A ResponseModel confirming deletion with a timestamp.</returns>
        [HttpDelete]
        public IActionResult Delete()
        {
            var data = new
            {
                DeletedAt = DateTime.Now
            };

            var response = new ResponseModel<object>
            {
                Success = true,
                Message = "Greeting deleted",
                Data = data
            };
            return Ok(response);
        }
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IGreetingBL _greetingBL;

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
        }

        /// <summary>
        /// Greeting from service
        /// </summary>
        /// <returns>"Hello World</returns>

        [HttpGet("Greetings")]
        public IActionResult Greetings()
        {
            logger.Info("GET /Greetings request received.");

            var response = _greetingBL.Greet();

            logger.Info("GET /Greetings response: {@Response}", response);
            return Ok(response);
        }

        /// <summary>
        /// Greeting by name based on attribute provided by user
        /// </summary>
        /// <param name="greetRequest"></param>
        /// <returns>Greeting</returns>
        /// 
        [HttpPost("GreetingsByName")]

        public IActionResult GreetingByName(GreetingRequestModel greetRequest)
        {
            logger.Info("GET request received.");

            var response = _greetingBL.GreetByName(greetRequest);
            logger.Info("GET response: {@Response}", response);
            return Ok(response);
        }
    }
}