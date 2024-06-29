using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [JsonIgnore]
    public string IdentificationNumberConcat { get; private set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [JsonIgnore]
    public string FullNameConcat { get; private set; }
}