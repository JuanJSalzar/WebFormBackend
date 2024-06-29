using Microsoft.EntityFrameworkCore;
using WebForm.Context;
using WebForm.DTOs;
using WebForm.IRepository;
using WebForm.Models;

namespace WebForm.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly WebFormContext _dbcontext;
    public PersonRepository(WebFormContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public async Task<List<Person>> SelectPeople()
    { 
        return await _dbcontext.People.FromSqlRaw("EXEC dbo.select_people").ToListAsync();
    }

    public async Task<Person> SelectPersonId(int id)
    {
        return await _dbcontext.People.FindAsync(id);
    }

    public async Task<Person> InsertPerson(PersonDto person)
    {
        if (await _dbcontext.People.AnyAsync(p => p.Email == person.Email))
        {
            return null;
        }
        var createPerson = new Person
        {
            FirstName = person.FirstName,
            LastName = person.LastName,
            IdentificationNumber = person.IdentificationNumber,
            Email = person.Email,
            IdentificationType = person.IdentificationType,
            CreationDate = DateTime.Now
        };

        await _dbcontext.People.AddAsync(createPerson);
        await _dbcontext.SaveChangesAsync();
        return createPerson;
    }

    public async Task<Person> UpdatePerson(int id, PersonUpdateDto personUpdateDto)
    {
        var existingPerson = await _dbcontext.People.FindAsync(id);
        if (existingPerson is not null)
        {
            existingPerson.FirstName = personUpdateDto.FirstName;
            existingPerson.LastName = personUpdateDto.LastName;
            existingPerson.Email = personUpdateDto.Email;
            existingPerson.IdentificationType = personUpdateDto.IdentificationType;
            await _dbcontext.SaveChangesAsync();
            return existingPerson;
        }

        return null;
    }

    public async Task<Person> DeletePerson(int id)
    {
        var person = await _dbcontext.People.FindAsync(id);

        if (person is not null)
        {
            _dbcontext.People.Remove(person);
            await _dbcontext.SaveChangesAsync();
            return person;
        }

        return null;
    }
}