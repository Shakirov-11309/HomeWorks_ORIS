namespace HttpServerLibrary
{
    /// <summary>
    /// Атрибут Get
    /// </summary>
    public sealed class GetAttribute : Attribute
    {
        /// <summary>
        /// Получаем путь к запросу
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Конструктор атрибута
        /// </summary>
        /// <param name="route">путь к запросу</param>

        public GetAttribute(string route)
        {
            Route = route;
        }
    }
}
