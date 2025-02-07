namespace MyHttttpServer.Models
{

    /// <summary>
    /// Класс представялет пользователя с учетными данными
    /// </summary>
    public class User
    {
        /// <summary>
        /// Свойство задает и получает логин поль-ля
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Свойство задает и получает пароль поль-ля
        /// </summary>
        public string Password { get; set; }

        public int Id { get; set; }
    }
}
