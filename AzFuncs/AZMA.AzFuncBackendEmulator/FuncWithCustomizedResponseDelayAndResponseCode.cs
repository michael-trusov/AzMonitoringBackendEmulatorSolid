using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AZMA.AzFuncBackendEmulator
{
    public static class FuncWithCustomizedResponseDelayAndResponseCode
    {
        [FunctionName("backend-emulator")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest httpRequest, ILogger log)
        {
            log.LogInformation("'backend-emulator' was triggered...");

            if (httpRequest.Query.ContainsKey("delay"))
            {
                int delay = int.Parse(httpRequest.Query["delay"]);
                log.LogDebug("'backend-emulator' delay is '{delay}'", delay);

                Thread.Sleep(delay);
            }

            if (httpRequest.Query.ContainsKey("responseStatusCode"))
            {
                int responseStatusCode = int.Parse(httpRequest.Query["responseStatusCode"]);
                log.LogDebug("'backend-emulator' response status code is '{responseStatusCode}'", responseStatusCode);

                if (Enum.IsDefined(typeof(HttpStatusCode), responseStatusCode))
                {
                    return new StatusCodeResult(responseStatusCode);
                }                
                else
                {
                    log.LogError("'backend-emulator' got invalid response status code: {responseStatusCode}", responseStatusCode);
                }
            }
                       
            return new OkResult();
        }
    }
}

