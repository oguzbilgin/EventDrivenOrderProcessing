namespace EventDriven.OrderProcessing.Domain.Exceptions;
public sealed class GenericDomainException : DomainException
{
    public string Code { get; }

    public GenericDomainException(string code, string message) : base(message)
    {
        Code = code;
    }
}
