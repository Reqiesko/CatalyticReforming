using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.test;

public class QuestionValidator : AbstractValidator<QuestionVM>
{
    public QuestionValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Текст вопроса не должен быть пустым");
    }
}
