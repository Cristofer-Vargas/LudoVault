using FluentValidation;
using LudoVault.DTO.Requests;
using LudoVault.Validations.Base;

namespace LudoVault.Validations
{
  public class DeveloperValidation : AbstractValidator<DeveloperRequest>
  {
    public DeveloperValidation()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome da developer não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Nome da developer deve ter no mínimo 3 caracteres.").WithErrorCode("400")
        .MaximumLength(60).WithMessage("Nome da developer deve ter no máximo 60 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Nome da developer não pode conter tags HTML.").WithErrorCode("400");

      RuleFor(x => x.FundationAt)
        .NotNull().WithMessage("A data de fundação é obrigatória.")
      .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
        .WithMessage("A data de fundação não pode estar no futuro.");
    }
  }
}