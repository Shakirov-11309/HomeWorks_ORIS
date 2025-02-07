using HttpServerLibrary;
using MyHttttpServer.Models;
using System.Data.SqlClient;

namespace MyHtttpServer.Endponts
{
    internal class UserEndpoints : BaseEndpoint
    {
        [Get("user")]
        public IHttpResponseResult GetUser() 
        {
            string connectionString = @"Data Source=localhost;Initial Catalog=test;User ID=sa;Password=P@ssw0rd; TrustServerCertificate=true;";
            var users = new List<User>();
            string sqlExpression = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                { 

                    while (reader.Read()) // построчно считываем данные
                    {
                        var user = new User()
                        {
                            Id = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            Password = reader.GetString(2),
                        };
                        users.Add(new User());

                    }
                }
                reader.Close();
            }
            return (IHttpResponseResult)Json(users);
        }
    
    }
}
