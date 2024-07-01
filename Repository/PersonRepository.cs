using Microsoft.Data.SqlClient;
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
        var parameters = new SqlParameter("@Id", id);
        var query = await _dbcontext.People.FromSqlRaw("EXEC dbo.select_person_by_id @Id", parameters).ToListAsync();
        return query.FirstOrDefault();
    }


    public async Task<Person> InsertPerson(PersonDto person)
    {
        if (await _dbcontext.People.AnyAsync(p => p.Email == person.Email))
        {
            return null;
        }

        var parameters = new[]
        {
            new SqlParameter("@FirstName", person.FirstName),
            new SqlParameter("@LastName", person.LastName),
            new SqlParameter("@IdentificationNumber", person.IdentificationNumber),
            new SqlParameter("@Email", person.Email),
            new SqlParameter("@IdentificationType", person.IdentificationType)
        };

        var insertedPerson = _dbcontext.People
            .FromSqlRaw("EXEC dbo.insert_person @FirstName, @LastName, @IdentificationNumber, @Email, @IdentificationType", parameters)
            .AsEnumerable()
            .FirstOrDefault();

        return insertedPerson;
    }

    public async Task<Person> UpdatePerson(int id, PersonUpdateDto personUpdateDto)
    {
        try
        {
            var existingPerson = await _dbcontext.People.FindAsync(id);

            if (existingPerson == null) return null;

            var parameters = new[]
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@FirstName", personUpdateDto.FirstName),
                new SqlParameter("@LastName", personUpdateDto.LastName),
                new SqlParameter("@Email", personUpdateDto.Email),
                new SqlParameter("@IdentificationType", personUpdateDto.IdentificationType)
            };

            var updatedPerson = _dbcontext.People
                .FromSqlRaw("EXEC dbo.update_person @Id, @FirstName, @LastName, @Email, @IdentificationType", parameters)
                .AsEnumerable()
                .FirstOrDefault();

            return updatedPerson;
        }
        catch (SqlException ex) when (ex.Number == 51002) 
        {
            throw new InvalidOperationException("Email already exists", ex);
        }
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