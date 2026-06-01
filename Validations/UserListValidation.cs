using FluentValidation;
using LudoVault.DTO.Requests;
using LudoVault.Validations.Base;

namespace LudoVault.Validations
{
  public class UserListValidation : AbstractValidator<UserListRequest>
  {
    public UserListValidation()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome de lista não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Nome de lista deve ter no mínimo 3 caracteres.").WithErrorCode("400")
        .MaximumLength(60).WithMessage("Nome de lista deve ter no máximo 60 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Nome de lista não pode conter tags HTML.").WithErrorCode("400");
    }
  }
}
