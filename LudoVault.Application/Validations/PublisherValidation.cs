using FluentValidation;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Validations
{
  public class PublisherValidation : AbstractValidator<PublisherRequest>
  {
    public PublisherValidation()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome da publisher não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Nome da publisher deve ter no mínimo 3 caracteres.").WithErrorCode("400")
        .MaximumLength(60).WithMessage("Nome da publisher deve ter no máximo 60 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Nome da publisher não pode conter tags HTML.").WithErrorCode("400");
    }
  }
}
