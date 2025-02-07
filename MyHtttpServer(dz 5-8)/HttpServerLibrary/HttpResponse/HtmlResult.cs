using System.Net;
using System.Text;

namespace HttpServerLibrary
{
    /// <summary>
    /// Класс для формирования HTML-ответа на HTTP-запрос.
    /// </summary>
    internal class HtmlResult : IHttpResponseResult
    {
        /// <summary>
        /// Содержит HTML-контент, который будет отправлен в ответе.
        /// </summary>
        private readonly string _html;

        /// <summary>
        /// Инициализирует новый экземпляр класса с указанным HTML-контентом.
        /// </summary>
        /// <param name="html">HTML-контент, который будет отправлен в качестве ответа</param>
        public HtmlResult(string html) 
        {
            _html = html;
        }

        /// <summary>
        /// Выполняет отправку HTML-контента в HTTP-ответе.
        /// </summary>
        /// <param name="response">HTTP ответ</param>
        public void Execute(HttpListenerResponse response)
        {
            response.Headers.Add("Content-Type", "text/html");
            byte[] buffer = Encoding.UTF8.GetBytes(_html);
            response.ContentLength64 = buffer.Length;
            using Stream output = response.OutputStream;
            output.Write(buffer);
            output.Flush();
        }
    }
}
