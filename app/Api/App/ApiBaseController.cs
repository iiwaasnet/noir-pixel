using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.App
{
    public abstract class ApiBaseController : ApiController
    {
        protected ApiErrorResult<T> ApiError<T>(HttpStatusCode statusCode, T content)
        {
            return new ApiErrorResult<T>(statusCode, content, this);
        }

        protected IHttpActionResult ApiError(HttpStatusCode statusCode)
        {
            return new ErrorResult(statusCode, this);
        }

        protected void ApiException<T>(HttpStatusCode statusCode, T error)
        {
            throw new HttpResponseException(Request.CreateResponse(statusCode, error));
        }

        protected string MakeAbsoluteUrl(string relativeUrl)
        {
            return string.Format("{0}://{1}/{2}",
                                 Request.RequestUri.Scheme,
                                 Request.RequestUri.Host,
                                 relativeUrl);
        }
    }
}