using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
[ApiController]
[Route("user")]
public class UserController : Controller
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult GetUsers()
    {
        try
        {
            var users = _repository.GetUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost]
    public IActionResult Add([FromBody] UserDtoInsert user)
    {
        try
        {
            var newUser = _repository.Add(user);
            return Created(string.Empty, newUser);
        }
        catch (Exception ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
}