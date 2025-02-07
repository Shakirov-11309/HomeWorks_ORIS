using System.Net;

namespace HttpServerLibrary
{
    /// <summary>
    /// Интерфейс, представляющий результат выполнения HTTP-запроса.
    /// Определяет метод для отправки ответа клиенту.
    /// </summary>
    public interface IHttpResponseResult
    {
        /// <summary>
        /// Выполняет отправку HTTP-ответа клиенту.
        /// </summary>
        /// <param name="response">HTTP-ответ</param>
        void Execute(HttpListenerResponse response);
    }
}
