using WebForm.DTOs;
using WebForm.Models;

namespace WebForm.IRepository;

public interface IPersonRepository
{
    Task<List<Person>> SelectPeople();
    Task<Person> SelectPersonId(int id);
    Task<Person> InsertPerson(PersonDto person);
    Task<Person> UpdatePerson(int id, PersonUpdateDto personDto);
    Task<Person> DeletePerson(int id);
}