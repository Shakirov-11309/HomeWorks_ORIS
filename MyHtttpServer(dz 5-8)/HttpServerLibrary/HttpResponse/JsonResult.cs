using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServerLibrary
{
    /// <summary>
    /// Класс для формирования JSONL-ответа на HTTP-запрос.
    /// </summary>
    internal class JsonResult : IHttpResponseResult
    {
        /// <summary>
        /// Содержит JSON-контент, который будет отправлен в ответе.
        /// </summary>
        private readonly object _data;

        /// <summary>
        /// Инициализирует новый экземпляр класса с указанным JSON-контентом.
        /// </summary>
        /// <param name="data">JSON-контент, который будет отправлен в качестве ответа</param>
        public JsonResult(object data)
        {
            _data = data;
        }

        /// <summary>
        /// Выполняет отправку JSON-контента в HTTP-ответе
        /// </summary>
        /// <param name="response">HTTP ответ</param>
        public void Execute(HttpListenerResponse response)
        {
            response.Headers.Add("Content-Type", "application/json");
            var json = JsonSerializer.Serialize(_data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = buffer.Length;
            using Stream output = response.OutputStream;
            output.Write(buffer);
            output.Flush();
        }
    }
}
