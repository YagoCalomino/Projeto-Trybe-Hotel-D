using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
[ApiController]
[Route("hotel")]
public class HotelController : Controller
{
    private readonly IHotelRepository _repository;

    public HotelController(IHotelRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetHotels()
    {
        var hotels = _repository.GetHotels();
        return Ok(hotels);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult PostHotel([FromBody] Hotel hotel)
    {
        var result = _repository.AddHotel(hotel);
        return Created(string.Empty, result);
    }
}

}