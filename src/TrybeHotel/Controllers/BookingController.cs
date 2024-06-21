using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;

        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var email = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            try
            {
                var result = _repository.Add(bookingInsert, email!);
                return Created(string.Empty, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var email = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            try
            {
                var booking = _repository.GetBooking(Bookingid, email!);
                return Ok(booking);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}