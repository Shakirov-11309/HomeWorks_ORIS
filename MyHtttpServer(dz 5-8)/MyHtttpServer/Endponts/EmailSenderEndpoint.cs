using HttpServerLibrary;
using System.Data.SqlClient;

namespace MyHtttpServer.Endponts
{
    internal class EmailSenderEndpoint : BaseEndpoint
    {
        // lol GET
        [Get("lol")]
        public IHttpResponseResult LOLPage()
        {
            Console.WriteLine("HAHAHAHAHAH");
            return Html("<h1>ILHAM</h1>");
        }

        private static async Task ReadDataAsync()
        {
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";

            string sqlExpression = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        object age = reader.GetValue(2);
                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }
                reader.Close();
            }
        }

        // request GET
        [Get("request")]
        public void RequestPage()
        {
            string responseText = "request";
            
        }

        // homework GET
        [Get("homework")]
        public void HomeworkPage()
        {
            string responseText = "homework";
            
        }


        // lol POST
        [Post("lol")]
        public void SendEmailToLOL(string email, string password)
        {

        }

        // request POST
        [Post("request")]
        public void SendEmailToRequest()
        {

        }

        // homework POST
        [Post("homework")]
        public void SendEmailToHomework()
        {

        }
    }
}
