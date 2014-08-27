using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Web.Controllers
{
    public class CustomJsonResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = MimeTypes.Json;
            if (Data != null)
            {
                var jsonSerializerSettings = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};
                response.Write(JsonConvert.SerializeObject(Data, jsonSerializerSettings));
            }
        }

        public object Data { get; set; }
    }
}