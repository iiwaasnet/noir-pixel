using System.Net;
using System.Web.Http;

namespace Api.App
{
    public abstract class ApiBaseController : ApiController
    {
        protected internal ApiErrorResult<T> ApiError<T>(HttpStatusCode statusCode, T content)
        {
            return new ApiErrorResult<T>(statusCode, content, this);
        }
    }
}