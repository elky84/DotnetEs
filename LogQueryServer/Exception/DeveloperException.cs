using System.Net;

namespace LogQueryServer.Exception
{
    public class DeveloperException : System.Exception
    {
        public Code.Result Result { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string Detail { get; set; }

        public DeveloperException(Code.Result result, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError, string detail = null)
        {
            Result = result;
            HttpStatusCode = httpStatusCode;
            Detail = detail;
        }
    }
}
