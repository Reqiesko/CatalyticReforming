using FluentValidation;


namespace CatalyticReforming.ViewModels.DAL_VM.test;

public class AnswerValidator: AbstractValidator<AnswerVM>
{
    public AnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Текст вопроса не должен быть пустым");
    }
}
