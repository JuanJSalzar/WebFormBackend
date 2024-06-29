using WebForm.Models;

namespace WebForm.IRepository;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> SelectPeople();
    Task<Person> SelectPersonId(int id);
    Task<Person> InsertPeople(Person person);
    Task<Person> UpdatePeople(Person person);
    Task<Person> DeletePeople(int id);
}