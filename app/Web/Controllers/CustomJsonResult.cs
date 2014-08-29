using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Web.Controllers
{
    public class CustomJsonResult : ActionResult
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings;

        static CustomJsonResult()
        {
            jsonSerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                         Converters = {new JavaScriptDateTimeConverter()}
                                     };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = MimeTypes.Json;
            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data, jsonSerializerSettings));
            }
        }

        public object Data { get; set; }
    }
}