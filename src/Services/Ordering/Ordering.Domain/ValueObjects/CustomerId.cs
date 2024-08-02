

namespace Ordering.Domain.ValueObjects;
//Strongly Typed to Avoid primitive obsession
public record CustomerId
{
    public Guid Value { get; }

    private CustomerId(Guid value) =>Value = value;
     public static CustomerId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("CustomerId Cannot be empty");
        }
        return new CustomerId(value);
    }

}
