namespace MyHtttpServer.Service
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IMailService
    {
        Task SendAsync(string login, string password, string messageBody);
    }
}
