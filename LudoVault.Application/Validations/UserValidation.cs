using FluentValidation;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Validations
{
  public class UserValidation : AbstractValidator<UserRequest>
  {
    public UserValidation()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome de usuário não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Nome de usuário deve conter mínimo de 3 caracteres.").WithErrorCode("400")
        .MaximumLength(40).WithMessage("Nome de usuário deve conter máximo de 40 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Nome de usuário não pode conter tags HTML.").WithErrorCode("400");

      RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email de usuário não pode ser vazio.").WithErrorCode("400")
        .EmailAddress().WithMessage("Email deve ser preenchido corretamente.").WithErrorCode("400")
        .MaximumLength(100).WithMessage("Email de usuário deve ter no máximo 100 caracteres.").WithErrorCode("400");

      RuleFor(x => x.Bio)
        .NotEmpty().WithMessage("Biografia de usuário não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Biografia de usuário deve conter mínimo de 3 caracteres.").WithErrorCode("400")
        .MaximumLength(500).WithMessage("Biografia de usuário deve conter máximo de 500 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Biografia de usuário não pode conter tags HTML.").WithErrorCode("400");

      RuleFor(x => x.PasswordHash)
        .NotEmpty().WithMessage("Senha de usuário não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(8).WithMessage("Senha de usuário deve conter mínimo de 8 caracteres.").WithErrorCode("400")
        .MaximumLength(100).WithMessage("Senha de usuário deve conter máximo de 100 caracteres.").WithErrorCode("400")
        .Matches(@"[A-Z]").WithMessage("A senha deve conter pelo menos uma letra maiúscula.").WithErrorCode("400")
        .Matches(@"[a-z]").WithMessage("A senha deve conter pelo menos uma letra minúscula.").WithErrorCode("400")
        .Matches(@"[0-9]").WithMessage("A senha deve conter pelo menos um número.").WithErrorCode("400")
        .Matches(@"[\!\@\#\$\%\^\&\*\(\)\+\-\=\[\]\{\}\;\:\'\""\,\<\>\.\?\/\~\\\|]").WithMessage("A senha deve conter pelo menos um caractere especial.").WithErrorCode("400")
        .When(x => x.PasswordHash != null && !x.PasswordHash.StartsWith("$"));
    }
  }
}
