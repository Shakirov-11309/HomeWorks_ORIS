using HttpServerLibrary;

namespace MyHtttpServer.Endponts
{
    internal class SparkEndpoint : BaseEndpoint
    {
        [Get("spark")]
        public IHttpResponseResult Test(string qwerty) 
        {
            Console.WriteLine(qwerty);
            return Html("<h1>Hello World!</h1>");
        }

        [Get("spark2")]
        public IHttpResponseResult Test2(object data)
        {
            Console.WriteLine(data);
            return Html("<h1>Hello World!</h1>");
        }
    }
}
