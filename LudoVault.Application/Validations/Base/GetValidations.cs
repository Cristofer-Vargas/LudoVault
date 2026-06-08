using FluentValidation.Results;

namespace LudoVault.Application.Validations.Base
{
  public static class GetValidations
  {
    public static Response GetErrors(this ValidationResult result)
    {
      var response = new Response();
      
      if (!result.IsValid)
      {
        foreach (var error in result.Errors)
        {
          response.Report.Add(Report.Create(error.ErrorMessage, int.Parse(error.ErrorCode)));
        }
        return response;
      }

      return response;
    }

    public static bool NotContainHtml(string? value)
    {
      if (string.IsNullOrEmpty(value)) return true;
      return !value.Contains('<') || !value.Contains('>');
    }
  }
}
