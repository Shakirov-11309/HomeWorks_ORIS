using System.Reflection;
using System.Web;

namespace HttpServerLibrary
{
    /// <summary>
    /// Класс обработчика эндпоинтов, отвечающий за маршрутизацию запросов и выполнение методов контроллеров.
    /// </summary>
    public class EndpointsHandler : Handler
    {
        /// <summary>
        /// Хранилище маршрутов, организованное в виде словаря, где ключ — путь, 
        /// а значение — список кортежей, содержащих HTTP-метод, обработчик (метод) и тип контроллера.
        /// </summary>
        private readonly Dictionary<string, List<(HttpMethod method, MethodInfo handler, Type endpointType)>> _routes = new();

        /// <summary>
        /// Конструктор по умолчанию, который автоматически регистрирует все контроллеры
        /// из текущей сборки.
        /// </summary>
        public EndpointsHandler() 
        { 
            RegisterEndpointsFromAssemblies(new[] {Assembly.GetEntryAssembly()});
        }

        /// <summary>
        ///  Обрабатывает запрос
        /// </summary>
        /// <param name="context">Контекст HTTP-запроса, содержащий информацию о запросе и ответе</param>
        public override void HandleRequest(HttpRequestContext context)
        {
            var url = context.Request.Url.LocalPath.Trim('/');
            var methodType = context.Request.HttpMethod;

            if (_routes.ContainsKey(url))
            {
                var route = _routes[url].FirstOrDefault(r => r.method.ToString().Equals(methodType, StringComparison.InvariantCultureIgnoreCase));
                if(route.handler != null)
                {
                    var endpointInstance = Activator.CreateInstance(route.endpointType) as BaseEndpoint;
                    if (endpointInstance != null)
                    {   
                        endpointInstance.SetContext(context);

                        // вызываем метод
                        var parametrs = GetParams(context, route.handler);
                        // TODO: подсказка, null - это параметры  (если это Get -> query, если Post -> formData)
                        var result = route.handler.Invoke(endpointInstance, parametrs) as IHttpResponseResult;
                        result?.Execute(context.Response);
                    }
                }
            }
            else if (Successor != null)
            {
                Successor.HandleRequest(context);
            }
            context.Response.Close();
        }

        /// <summary>
        /// Регистрируем ендпоинты с помошью их указанными сборками
        /// </summary>
        /// <param name="assemblies">Массив сборокб в которых необходимо искать эндпоинты</param>
        private void RegisterEndpointsFromAssemblies(Assembly[] assemblies) 
        { 
            foreach (var assembly in assemblies)
            {
                var endpointsTypes = assembly.GetTypes()
                    .Where(t => typeof(BaseEndpoint).IsAssignableFrom(t) && !t.IsAbstract);

                foreach (var endpointType in endpointsTypes)
                {
                    var methods = endpointType.GetMethods();

                    foreach(var method in methods)
                    {
                        // TODO: можно отрефакторить
                        var getAttribute = method.GetCustomAttribute<GetAttribute>();
                        if(getAttribute != null)
                        {
                            RegisterRoute(getAttribute.Route, HttpMethod.Get, method, endpointType);
                        }

                        var postAttribute = method.GetCustomAttribute<PostAttribute>();
                        if (postAttribute != null)
                        {
                            RegisterRoute(postAttribute.Route, HttpMethod.Post, method, endpointType);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Создает путь по маршруту
        /// </summary>
        /// <param name="route">путь маршрута</param>
        /// <param name="method">метод маршрута (GET или POST)</param>
        /// <param name="handler">метод-обработчик для заданного маршрута </param>
        /// <param name="endpointType">формат типа</param>
        private void RegisterRoute(string route, HttpMethod method, MethodInfo handler, Type endpointType)
        {
            if(!_routes.ContainsKey(route))
            {
                _routes[route] = new();
            }

            _routes[route].Add((method, handler, endpointType));
        }

        private object[] GetParams(HttpRequestContext context, MethodInfo handler)
        {
            var parameters = handler.GetParameters();
            var result = new List<object>();

            if (context.Request.HttpMethod == "GET" || context.Request.HttpMethod == "POST")
            {
                // Извлечение параметров из QueryString (GET) или тела запроса (POST)
                foreach (var parameter in parameters)
                {
                    if (context.Request.HttpMethod == "GET")
                    {
                        result.Add(Convert.ChangeType(context.Request.QueryString[parameter.Name],
                            parameter.ParameterType));
                    }
                    else if (context.Request.HttpMethod == "POST")
                    {
                        using var reader = new StreamReader(context.Request.InputStream);
                        string body = reader.ReadToEnd();
                        var data = HttpUtility.ParseQueryString(body);
                        result.Add(Convert.ChangeType(data[parameter.Name], parameter.ParameterType));
                    }
                }
            }
            else
            {
                // Дополнительная обработка для сегментов URL
                var urlSegments = context.Request.Url.Segments
                    .Skip(2) // Пропуск первых двух сегментов
                    .Select(s => s.Replace("/", ""))
                    .ToArray();

                for (int i = 0; i < parameters.Length; i++)
                {
                    result.Add(Convert.ChangeType(urlSegments[i], parameters[i].ParameterType));
                }
            }

            return result.ToArray();
        }

    }
}
