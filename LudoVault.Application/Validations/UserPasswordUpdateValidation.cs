using FluentValidation;
using LudoVault.Application.DTO.Requests;

namespace LudoVault.Application.Validations
{
  public class UserPasswordUpdateValidation : AbstractValidator<UserPasswordUpdateRequest>
  {
    public UserPasswordUpdateValidation()
    {
      RuleFor(x => x.OldPassword)
        .NotEmpty().WithMessage("A senha antiga não pode ser vazia.").WithErrorCode("400");

      RuleFor(x => x.NewPassword)
        .NotEmpty().WithMessage("A nova senha não pode ser vazia.").WithErrorCode("400")
        .MinimumLength(8).WithMessage("A nova senha deve conter mínimo de 8 caracteres.").WithErrorCode("400")
        .MaximumLength(100).WithMessage("A nova senha deve conter máximo de 100 caracteres.").WithErrorCode("400")
        .Matches(@"[A-Z]").WithMessage("A nova senha deve conter pelo menos uma letra maiúscula.").WithErrorCode("400")
        .Matches(@"[a-z]").WithMessage("A nova senha deve conter pelo menos uma letra minúscula.").WithErrorCode("400")
        .Matches(@"[0-9]").WithMessage("A nova senha deve conter pelo menos um número.").WithErrorCode("400")
        .Matches(@"[\!\@\#\$\%\^\&\*\(\)\+\-\=\[\]\{\}\;\:\'\""\,\<\>\.\?\/\~\\\|]").WithMessage("A nova senha deve conter pelo menos um caractere especial.").WithErrorCode("400");
    }
  }
}
