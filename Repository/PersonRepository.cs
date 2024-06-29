using Microsoft.EntityFrameworkCore;
using WebForm.Context;
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
    public async Task<IEnumerable<Person>> SelectPeople()
    { 
        return await _dbcontext.People.FromSqlRaw("EXEC dbo.select_people").ToListAsync();
    }

    public Task<Person> SelectPersonId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Person> InsertPeople(Person person)
    {
        throw new NotImplementedException();
    }

    public Task<Person> UpdatePeople(Person person)
    {
        throw new NotImplementedException();
    }

    public Task<Person> DeletePeople(int id)
    {
        throw new NotImplementedException();
    }
}