using LudoVault.Application.Validations.Base;
using Microsoft.AspNetCore.Mvc;

namespace LudoVault.Api.Controllers.Base
{
  public static class GetHttpResponseFromReports
  {
    public static IActionResult GetResponse(this ControllerBase controllerBase, Response response)
    {
      if (response.IsSuccessul)
      {
        return controllerBase.Ok(response);
      }

      var firstReport = response.Report.FirstOrDefault();
      var statusCode = firstReport.Code ?? 400;
      return controllerBase.StatusCode(statusCode, response);

    }
  }
}