using System;
using System.Collections;
using System.ComponentModel;

using FluentValidation;


namespace CatalyticReforming.ViewModels;

public abstract class ValidatableViewModel<ValidatorType>: ViewModelBase, INotifyDataErrorInfo where ValidatorType: IValidator, new()
{
    public ValidatorType Validator { get; set; } = new ValidatorType();
    public IEnumerable GetErrors(string? propertyName)
    {
        //честно говоря, я немного удивлен что это так работает, кстати очередной пример слабости дженериков в .net,
        //если бы можно было писать T<> в секции where, то здесь бы я написал ValidatorType: AbstractValidator<> вместо ValidatorType: IValidator
        var errors = Validator
                     .Validate(new ValidationContext<ValidatableViewModel<ValidatorType>>(this))
                     .GetErrors(propertyName);

        return errors;
    }

    public bool HasErrors
    {
        get
        {
            var res = Validator.Validate(new ValidationContext<ValidatableViewModel<ValidatorType>>(this));

            return !res.IsValid;
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
}
