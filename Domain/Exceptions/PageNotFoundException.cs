namespace Domain.Exceptions;

public class PageNotFoundException : Exception
{
    public PageNotFoundException(string message) : base(message)
    {
    }
}