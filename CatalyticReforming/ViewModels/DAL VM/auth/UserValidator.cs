using CatalyticReforming.ViewModels.DAL_VM.auth;

using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM;

public class UserValidator: AbstractValidator<UserVM>
{
    public UserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Имя пользователя не должно быть пустым");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Пароль не должен быть пустым");
        RuleFor(x => x.Role)
            .NotNull()
            .WithMessage("Роль не должна быть пуста");
    }
}
