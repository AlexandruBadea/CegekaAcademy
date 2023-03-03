namespace PetShelter.BusinessLayer.Models;

public class Person
{
    public string Name { get; set; }
    public string IdNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public static implicit operator Person(DataAccessLayer.Models.Person v)
    {
        throw new NotImplementedException();
    }
}