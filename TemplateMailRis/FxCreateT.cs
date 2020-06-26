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
using System.Net;
using TemplateMailRis.models;
using TemplateMailRis.controllers;

namespace TemplateMailRis
{
    public static class FxCreateT
    {
        [FunctionName("FxCreateTemplatemail")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = "createTemplateMail")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            HttpResponseMessage respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest);
            mdlTemplateemail dataReq = new mdlTemplateemail();
            string reqBody = await new StreamReader(req.Body).ReadToEndAsync();
            dataReq = JsonConvert.DeserializeObject<mdlTemplateemail>(reqBody);
            if (dataReq.mail_from.Length == 0 || dataReq.product_code.Length == 0 || dataReq.service_code.Length == 0 
                || dataReq.mail_mask.Length == 0 || dataReq.template_content.Length == 0 || dataReq.client_code.Length == 0)
            {
                respuesta = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("\"message\" : \"El servidor No pudo procesar la solicitud le faltan campos \" }")
                };
            }
            else
            {
                
                cnxCreateTempalte creoTemplate = new cnxCreateTempalte(dataReq);
                respuesta = creoTemplate.insertTemplate();
            }
            return respuesta;
        }
    }
}
