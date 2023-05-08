using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class CatalystValidator: AbstractValidator<CatalystVM>
{
    public CatalystValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");

        RuleFor(x => x.Density)
            .GreaterThan(0)
            .WithMessage("Плотность должна быть больше 0");
        RuleFor(x => x.StrengthFactor)
            .GreaterThan(0)
            .WithMessage("Коэффициент прочности должен быть больше 0"); // todo мб и может, потом уточнить
    }
}
