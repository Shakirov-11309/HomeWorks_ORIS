using HttpServerLibrary;

namespace MyHtttpServer
{
    internal class Program 
    {
        // TODO в зависимости от типа файла подставлять соответсвующий content-type (js, css, html)
        // добавить логику в EndPointHandler что если метод ничего не возрашает тогда деать так же Rsponse пустой
        // не должно быть долгово лоадаре если вызвался просто метод Post IhttpResponseResult

        // TODO: проверить вышеописанный метод на тесте который написали на занятии
        // [Теория] Razor Шаблонитор
        // [Теория] MSTest
        // [Теория] Moq

        // 23/11/2024

        // TODO: Необходимо реализовать метод GetHtmlByTemplate в классе CustomTemplator таким образом,
        // чтобы он обрабатывал шаблон следующего вида:
        // "if(gender){<h1>Да мой господин, {name}</h1> }else{ <h1>Да моя госпожа, {name}</h1>}"


        static async Task Main(string[] args) 
        {
            await AppConfig.Instance.LoadConfigAsync();
            var prefixes = new[] { $"http://{AppConfig.Instance.Domain}:{AppConfig.Instance.Port}/" };
            var server = new HttpServer(prefixes, AppConfig.Instance.StaticDirectoryPath);
            await server.StartAsync();
        }
    }
}