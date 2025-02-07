using System.Net.Sockets;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        string host = "127.0.0.1";
        int port = 8888;
        TcpClient client = new TcpClient();
        StreamReader? Reader = null;
        StreamWriter? Writer = null;
        string? userName;

        public Form1()
        {
            InitializeComponent();

            // Подключение к серверу
            client.Connect(host, port);
            Reader = new StreamReader(client.GetStream());
            Writer = new StreamWriter(client.GetStream());

            // Запрос имени пользователя
            userName = Microsoft.VisualBasic.Interaction.InputBox("Введите ваше имя:", "Имя пользователя", "User");
            if (string.IsNullOrEmpty(userName)) userName = "User";

            // Отправка имени на сервер
            Writer.WriteLine(userName);
            Writer.Flush();

            // Запуск потока для получения сообщений
            Task.Run(() => ReceiveMessageAsync(Reader));
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            string message = textBox1.Text;
            await SendMessageAsync(message);
            textBox1.Clear();
        }

        // Отправка сообщений
        async Task SendMessageAsync(string message)
        {
            if (Writer != null)
            {
                await Writer.WriteLineAsync(message);
                await Writer.FlushAsync();
            }
        }

        // Получение сообщений
        async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    string? message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message)) continue;

                    // Вывод сообщения в ListBox
                    Print(message);
                }
                catch
                {
                    break;
                }
            }
        }

        // Вывод сообщения в ListBox
        void Print(string message)
        {
            if (listBox1.InvokeRequired)
            {
                listBox1.Invoke(new Action<string>(Print), message);
            }
            else
            {
                listBox1.Items.Add(message);
            }
        }

        // Обработка закрытия формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Writer != null)
            {
                Writer.WriteLine($"{userName} вышел из чата");
                Writer.Flush();
            }
            Writer?.Close();
            Reader?.Close();
            client.Close();
        }
    }

}
