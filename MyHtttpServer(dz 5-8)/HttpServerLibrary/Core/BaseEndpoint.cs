using HttpServerLibrary.HttpResponse;

namespace HttpServerLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseEndpoint
    {
        /// <summary>
        /// 
        /// </summary>
        protected HttpRequestContext Context { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        internal void SetContext(HttpRequestContext context)
        {
            Context = context;
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="responseText"></param>
        /// <returns></returns>
        protected IHttpResponseResult Html(string responseText) => new HtmlResult(responseText);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IHttpResponseResult Json(object data) => new JsonResult(data);


        protected IHttpResponseResult Redirect(string location) => new RedirectResult(location);


    }
}
