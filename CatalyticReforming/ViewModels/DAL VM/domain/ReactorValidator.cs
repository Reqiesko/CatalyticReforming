using System.Data;

using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.domain;

public class ReactorValidator: AbstractValidator<ReactorVM>
{
    public ReactorValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Название не должно быть пустым");

        RuleFor(x => x.Pressure)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым"); // todo проверить
    }
}
