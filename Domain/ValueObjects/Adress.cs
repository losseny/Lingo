namespace Domain.ValueObjects;

/// <summary>
/// There is no need for ValueObjects but the sake of explaining it somehow i will be using this
/// So Like i explained in <see cref="ValueObject"/>. Value objects is a value that holds values
/// </summary>
public class Adress : ValueObject
{
    
    private string Street { get; set; }


    public Adress(string street)
    {
        Street = street;
    }

    public void moveToAnotherStreet(string newStreet)
    {
        if (Street.Equals(newStreet))
        {
            throw new Exception("Cannot move to the street you already living in!");
        }
        
        Street = newStreet;

    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}