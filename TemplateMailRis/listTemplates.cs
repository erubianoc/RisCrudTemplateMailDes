using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using TemplateMailRis.controllers;
using System.Net;
using System.Text;

namespace TemplateMailRis
{
    public static class listTemplates
    {
        [FunctionName("FxListTemplates")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "listTemplatesMail")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("inicio lis templates");
            HttpResponseMessage respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
            string codPro = req.Query["product_code"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            codPro = codPro ?? data?.name;
            string codcli = req.Query["client_code"];
            requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic datacli = JsonConvert.DeserializeObject(requestBody);
            codcli = codcli ?? data?.name;
            cnxgetListTempalte listasTemplate;
            if (codcli == null && codPro == null)
            {
                listasTemplate = new cnxgetListTempalte();
            }
            else
            {
                listasTemplate = new cnxgetListTempalte(codcli, codPro);
            }
            (var registros, var errorSqlEx) = listasTemplate.GetTemplate();
            if (errorSqlEx.Length > 1)
            {
                respuesta = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("{\"message\" : \"la autenticacion ha fallado o todavia no se ha facilitado" + errorSqlEx + " \" }", Encoding.UTF8, "json/application")
                };
            }
            else
            {
                if (registros.Count > 0)
                {
                    string jsonSerializado = "";
                    jsonSerializado = JsonConvert.SerializeObject(registros);
                    respuesta = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(jsonSerializado, Encoding.UTF8, "json/application")
                    };
                }
                else
                {
                    respuesta = new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        Content = new StringContent("{\"message\" : \"ejecutado correctamente pero no encontro resultados\" }", Encoding.UTF8, "json/application")
                    };
                }
            }
            return respuesta;
        }
    }
}
