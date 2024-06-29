using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebForm.Models;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdentificationNumber { get; set; }
    public string Email { get; set; }
    public string IdentificationType { get; set; }
    public DateTime CreationDate { get; set; }
    public string IdentificationNumberConcat { get; private set; }
    public string FullNameConcat { get; private set; }
}