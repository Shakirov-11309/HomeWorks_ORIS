namespace HttpServerLibrary
{
    /// <summary>
    /// Абстрактный класс для обрабатывания запросов 
    /// </summary>
    public abstract class Handler
    {
        /// <summary>
        /// Переходник 
        /// </summary>
        public Handler Successor { get; set; }

        /// <summary>
        /// Обрабатывает запрос 
        /// </summary>
        /// <param name="context"></param>
        public abstract void HandleRequest(HttpRequestContext context);
    }
}
