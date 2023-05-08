using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.auth;

public class TestConfigValidator : AbstractValidator<TestConfigVM>
{
    public TestConfigValidator()
    {
        RuleFor(x => x.NumberOfQuestions)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .GreaterThan(0)
            .WithMessage("Количество вопросов должно быть больше 0");

        RuleFor(x => x.NumberOfQuestionsToPass)
            .NotEmpty()
            .WithMessage("Поле не должно быть пустым")
            .GreaterThan(0)
            .WithMessage("Количество вопросов для успешного прохождения должно быть больше 0")
            .LessThanOrEqualTo(x => x.NumberOfQuestions)
            .WithMessage("Количество вопросов для успешного прохождения должно быть меньше или равно количеству вопросов");

    }
}
