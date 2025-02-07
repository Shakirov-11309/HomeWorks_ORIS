using System.Net;

namespace HttpServerLibrary
{
    /// <summary>
    /// Класс представляет контекст Http-запроса, содеражащий свойства запроса и ответа 
    /// </summary>
    public class HttpRequestContext
    {
        /// <summary>
        /// Задает и получает запрос 
        /// </summary>
        public HttpListenerRequest Request { get; set; }

        /// <summary>
        /// Задает и получает ответ
        /// </summary>
        public HttpListenerResponse Response { get; set; }

        /// <summary>
        /// Конструктор с зданным контекстом Http
        /// </summary>
        /// <param name="context">данные запроса и ответа</param>
        public HttpRequestContext(HttpListenerContext context)
        {
            Request = context.Request;
            Response = context.Response;
        }
    }
}
