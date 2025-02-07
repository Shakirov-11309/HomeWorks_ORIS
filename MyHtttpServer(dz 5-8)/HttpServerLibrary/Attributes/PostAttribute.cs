namespace HttpServerLibrary
{
    /// <summary>
    /// Атрибут Post
    /// </summary>
    public sealed class PostAttribute : Attribute
    {
        /// <summary>
        /// Получаем путь к запросу
        /// </summary>
        public string Route { get; }

        /// <summary>
        ///  Конструктор атрибута
        /// </summary>
        /// <param name="route">путь к запросу</param>
        public PostAttribute(string route)
        {
            Route = route;
        }
    }
}
