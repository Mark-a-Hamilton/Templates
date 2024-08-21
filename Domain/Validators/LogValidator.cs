namespace Domain.Validators;

public class LogValidator : AbstractValidator<Domain.Models.DmnLog>
{
    public LogValidator()
    {
        RuleFor(p => p.Message).NotEmpty();
        RuleFor(p => p.Level).NotEmpty();
    }
}

//  Other validator clases go below