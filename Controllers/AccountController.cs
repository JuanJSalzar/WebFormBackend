using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebForm.Context;
using WebForm.DTOs;
using WebForm.IRepository;
using WebForm.Models;
using WebForm.Utils;

namespace WebForm.Controllers;

public class AccountController : ControllerBase
{
    private readonly WebFormContext _dbcontext;
    private readonly Utilities _utils;
    private readonly IPersonRepository _personRepository;

    public AccountController(WebFormContext dbcontext, Utilities utils, IPersonRepository personRepository)
    {
        _dbcontext = dbcontext;
        _utils = utils;
        _personRepository = personRepository;
    }

    [HttpPost]
    [Route("api/register")]
    public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
    {
        if (string.IsNullOrEmpty(registerDto.FirstName) || string.IsNullOrEmpty(registerDto.LastName) || 
            string.IsNullOrEmpty(registerDto.IdentificationNumber) || string.IsNullOrEmpty(registerDto.Email) || 
            string.IsNullOrEmpty(registerDto.IdentificationType) || string.IsNullOrEmpty(registerDto.Username) || 
            string.IsNullOrEmpty(registerDto.Password))
        {
            return BadRequest("All fields are required");
        }

        if (await _dbcontext.Users.AnyAsync(u => u.Username == registerDto.Username))
        {
            return BadRequest("Username already exists");
        }

        var user = new User
        {
            Username = registerDto.Username,
            Password = _utils.EncryptSHA256(registerDto.Password)
        };

        var person = new PersonDto
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IdentificationNumber = registerDto.IdentificationNumber,
            Email = registerDto.Email,
            IdentificationType = registerDto.IdentificationType
        };

        var transaction = await _dbcontext.Database.BeginTransactionAsync();
        try
        {
            var newPerson = await _personRepository.InsertPerson(person);
            if (newPerson == null)
            {
                return BadRequest(new { error = "Email already exists in the database, please choose another" });
            }

            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();

            await transaction.CommitAsync();
            user.Password = "";
            return Ok(user);
        }
        catch(Exception e)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, e.Message);
        }
    }
    // [HttpPost]
    // [Route("api/login")]
    // public async Task<IActionResult> Login(LoginDto loginDto)
    // {
    //     var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == _utils.EncryptSHA256(loginDto.Password));
    //     if (user == null)
    //     {
    //         return BadRequest("Invalid credentials");
    //     }
    //     else
    //     {
    //         return Ok(new { token = _utils.JwtGenerateToken(user) });
    //     }
    // }
    [HttpPost]
    [Route("api/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (loginDto == null || string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.Password))
        {
            return BadRequest("Username or Password cannot be empty");
        }

        try
        {
            var encryptedPassword = _utils.EncryptSHA256(loginDto.Password);
            var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Password == encryptedPassword);
        
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }
            else
            {
                return Ok(new { token = _utils.JwtGenerateToken(user) });
            }
        }
        catch (Exception ex)
        {
            // Logging the exception
            Console.WriteLine($"Exception occurred: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}