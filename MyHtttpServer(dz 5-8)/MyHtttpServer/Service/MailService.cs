using MyHtttpServer.Service;
using System.Net;
using System.Net.Mail;

namespace MyHtttpServer.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal class MailService : IMailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="message"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task SendAsync(string email, string message, string path)
        {   
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // pubovymminpauhob gmail: xdge imju alpf gdtd
            using (smtp)
            {
                smtp.Credentials = new NetworkCredential("codpromaster@yandex.ru", "aavyayvatcygytrn");
                smtp.EnableSsl = true;

                string attachPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HomeWork", "MyHtttpServer.zip");
                Console.WriteLine(path);
                using (MailMessage m = new MailMessage())
                {
                    m.From = new MailAddress("codpromaster@yandex.ru");
                    m.To.Add(email);
                    m.Subject = "Анимэ и Электриники";

                    switch (path.Split(@"\")[^1])
                    {
                        case "index_login.html":
                            m.Body = $"Добро пожаловать в систему ваш логин {email} ваш пароль {message}";
                            break;

                        case "lol.html":
                            m.Body =
                                $"Ха-ха вы попались, ваш логин {email} ваш пароль {message} теперь знаю я Низамов Алмаз";
                            break;

                        case "requests.html":
                            m.Body = $"Вы подписались на рассылку ваш логин {email} ваш пароль {message}";
                            break;

                        case "home-work.html":
                            m.Body = $"Мое ДЗ. Шакиров Ильхам Радикович. Архив на проект: 'https://drive.google.com/drive/folders/1I7p7yR_2tVvTwujmbmgErvyRhvOGMjuq?usp=sharing";
                            break;
                    }
                    
                    try
                    {
                        smtp.Send(m);
                        Console.WriteLine("Сообщение отправлено");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Ошибка отправки сообщения: {e.Message}");
                        Console.WriteLine($"Ошибка отправки сообщения: {e.StackTrace}");
                    }
                }
            }
        }
    }
}
