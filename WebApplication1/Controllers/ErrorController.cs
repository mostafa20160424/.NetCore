using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger; // to group all logs comming from ErrorController
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "sorry requeste not be found";
                    logger.LogWarning($"404 Error Ocurred Path = {statusCodeResult.OriginalPath}" +
                        $"and query string is = {statusCodeResult.OriginalQueryString}");
                    // then will create text file in C:\DemoLogs the file content is the error

                    break;

            }
            return View("NotFound");
        }

        [Route("Error")]
        [AllowAnonymous]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            logger.LogError($"the path {exceptionDetails.Path}" +
                $" threw {exceptionDetails.Error}");
            // then will create text file in C:\DemoLogs the file content is the error


            return View("Error");
        }
    }
}