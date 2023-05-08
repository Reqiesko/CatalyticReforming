using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class InstallationValidator : AbstractValidator<InstallationVM>
{
    public InstallationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");

        RuleFor(x => x.Reactor)
            .NotNull()
            .WithMessage("Установка должна иметь реактор");
    }
}
