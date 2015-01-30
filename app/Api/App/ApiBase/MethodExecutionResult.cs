using System.Collections.Generic;
using System.Web.Http;

namespace Api.App.ApiBase
{
    public class MethodExecutionResult<T>
    {
        public MethodExecutionResult(T data)
        {
            Data = data;
        }

        public MethodExecutionResult(IHttpActionResult error)
        {
            Error = error;
        }

        public T Data { get; private set; }
        public IHttpActionResult Error { get; private set; }

        public bool Succeeded
        {
            get { return !EqualityComparer<T>.Default.Equals(Data, default(T)) && Error == null; }
        }
    }
}