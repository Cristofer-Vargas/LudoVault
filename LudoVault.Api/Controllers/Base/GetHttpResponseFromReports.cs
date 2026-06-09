using System.Runtime.Serialization;
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
        var successStatusCode = response.Status;

        if (successStatusCode != 0)
        {
          if (successStatusCode == 204)
          {
            return controllerBase.NoContent();
          }
          return controllerBase.StatusCode(successStatusCode, response);
        }
        return controllerBase.Ok(response);
      }

      var firstReport = response.Report.FirstOrDefault();
      var statusCode = firstReport.Code ?? 400;
      response.Status = statusCode;
      return controllerBase.StatusCode(statusCode, response);

    }
  }
}