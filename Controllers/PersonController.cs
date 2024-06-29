using Microsoft.AspNetCore.Mvc;
using WebForm.IRepository;
using WebForm.Models;

namespace WebForm.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    public PersonController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Person>>> Get()
    {
        var people = await _personRepository.SelectPeople();
        return Ok(people);
    }
}