using System.Web.Mvc;
using Newtonsoft.Json;

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
                response.Write(JsonConvert.SerializeObject(Data));
            }
        }

        public object Data { get; set; }
    }
}