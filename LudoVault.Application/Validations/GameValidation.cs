using FluentValidation;
using LudoVault.Application.DTO.Requests;
using LudoVault.Application.Validations.Base;

namespace LudoVault.Application.Validations
{
  public class GameValidation : AbstractValidator<GameRequest>
  {
    public GameValidation()
    {
      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Nome do jogo não pode ser vazio.").WithErrorCode("400")
        .MinimumLength(3).WithMessage("Nome do jogo deve conter mínimo de 3 caracteres.").WithErrorCode("400")
        .MaximumLength(100).WithMessage("Nome do jogo deve conter máximo de 100 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Nome do jogo não pode conter tags HTML.").WithErrorCode("400");

      RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Descrição não pode ser vazia.").WithErrorCode("400")
        .MaximumLength(500).WithMessage("Descrição do jogo deve conter máximo de 500 caracteres.").WithErrorCode("400")
        .Must(GetValidations.NotContainHtml).WithMessage("Descrição do jogo não pode conter tags HTML.").WithErrorCode("400");

      RuleFor(x => x.PublisherId)
        .GreaterThan(0).WithMessage("Uma Publisher deve ser informada!").WithErrorCode("400");

      RuleFor(x => x.PlatformIds)
        .NotEmpty().WithMessage("Deve ser informado ao menos uma plataforma!").WithErrorCode("400");
      RuleForEach(x => x.PlatformIds)
        .GreaterThan(0).WithMessage("Id de plataforma inválido.").WithErrorCode("400");

      RuleFor(x => x.GenreIds)
        .NotEmpty().WithMessage("Deve ser informado ao menos um gênero!").WithErrorCode("400");
      RuleForEach(x => x.GenreIds)
        .GreaterThan(0).WithMessage("Id de gênero inválido.").WithErrorCode("400");
    }
  }
}
