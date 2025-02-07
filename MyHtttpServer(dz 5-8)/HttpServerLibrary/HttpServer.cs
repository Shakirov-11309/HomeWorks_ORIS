using System.Net;
namespace HttpServerLibrary;

public class HttpServer
{
    private readonly StaticFilesHandler _staticFilesHandler;
    private readonly EndpointsHandler _endpointsHandler;

    private HttpListener _listener = new HttpListener();

    public HttpServer(string[] prefixes, string File)
    {
        _listener = new HttpListener();
        foreach (var prefix in prefixes)
        {
            Console.WriteLine($"Server started on {prefix}");
            _listener.Prefixes.Add(prefix);
        }
        _staticFilesHandler = new StaticFilesHandler();
        _endpointsHandler = new EndpointsHandler();
    }

    public async Task StartAsync()
    {
        _listener.Start();
        while (_listener.IsListening)
        {
            var context = await _listener.GetContextAsync();
            var httpRequestContext = new HttpRequestContext(context);
            await ProcessRequestAsync(httpRequestContext);
        }
    }

    private async Task ProcessRequestAsync(HttpRequestContext context)
    {
        _staticFilesHandler.Successor = _endpointsHandler;
        _staticFilesHandler.HandleRequest(context);
    }


    private async void ProcessPostMethod(HttpListenerContext context, string path)
    {
        var request = context.Request;
        var body = new StreamReader(request.InputStream).ReadToEnd();
        var content = body.Split("&"); // 'email=test@mail.com', 'password=123'
        if (content.Length != 2) return;

        var email = content[0].Replace("email=", "").Replace("%40", "@");
        var message = content[1].Replace("password=", "");

        if (email.Length == 0 || message.Length == 0 || (email.Contains("@") == false && email.Contains("%40")) || !email.Contains("."))
        {
            context.Response.StatusCode = 400;
            context.Response.Close();
        }

        Console.WriteLine("Получено сообщение: ");
        Console.WriteLine(body);


        //MailService mailService = new MailService();

        Console.WriteLine(email + " | " + message);

        //await mailService.SendAsync(email, message, path);

        Console.WriteLine("Отправлено сообщение");

        context.Response.StatusCode = 200;
        context.Response.Close();
    }

    private void Stop()
    {
        _listener.Stop();
        Console.WriteLine("Server closed");
    }
}
