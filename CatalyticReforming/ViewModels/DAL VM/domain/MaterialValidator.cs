using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class MaterialValidator : AbstractValidator<MaterialVM>
{
    public MaterialValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");

        RuleFor(x => x.AromaticHydrocarbonsContent)
            .GreaterThan(0)
            .WithMessage("Содержание ароматических углеводородов должно  быть больше 0"); // todo мб неправильно

        RuleFor(x => x.NaphthenicHydrocarbonsContent)
            .GreaterThan(0)
            .WithMessage("Содержание нафтеновых углеводородов должно  быть больше 0"); // todo мб неправильно
    }
}
