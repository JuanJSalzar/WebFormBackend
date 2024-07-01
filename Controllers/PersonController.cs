using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebForm.DTOs;
using WebForm.IRepository;
using WebForm.Models;

namespace WebForm.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly IPersonRepository _personRepository;
    public PersonController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Person>>> Select()
    {
        var people = await _personRepository.SelectPeople();
        return Ok(people);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> SelectById(int id)
    {
        var person = await _personRepository.SelectPersonId(id);
        if (person is not null) return Ok(person);
        
        return NotFound();
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<Person>> Update([FromRoute] int id, [FromBody] PersonUpdateDto personUpdateDto)
    {
        try
        {
            var updatedPerson = await _personRepository.UpdatePerson(id, personUpdateDto);

            if (updatedPerson != null)
            {
                return Ok(updatedPerson);
            }
            else
            {
                return NotFound();
            }
        }
        catch (InvalidOperationException ex) when (ex.Message == "Email already exists")
        {
            return BadRequest("Email already exists in the database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error.");
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<Person>> Delete([FromRoute] int id)
    {
        var deletePerson = await _personRepository.DeletePerson(id);
        if (deletePerson is not null) return Ok(deletePerson);
        
        return NotFound();
    }
}