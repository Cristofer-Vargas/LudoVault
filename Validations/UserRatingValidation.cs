using FluentValidation;
using LudoVault.DTO.Requests;
using LudoVault.Validations.Base;

namespace LudoVault.Validations
{
  public class UserRatingValidation : AbstractValidator<UserRatingRequest>
  {
    public UserRatingValidation()
    {
      RuleFor(x => x.Rating)
        .NotNull().WithMessage("Avaliação de jogo não pode ser nula.").WithErrorCode("400")
        .InclusiveBetween(0m, 5m).WithMessage("Avaliação do jogo deve ser entre 0 e 5.").WithErrorCode("400");
      RuleFor(x => x.Comment)
        .NotEmpty().WithMessage("O comentário não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(10).WithMessage("O comentário deve ter no mínimo 10 caracteres.").WithErrorCode("400")
        .MaximumLength(1200).WithMessage("O comentário deve ter no máximo 1200 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("O comentário não pode conter tags HTML.").WithErrorCode("400");
    }
  }
}
