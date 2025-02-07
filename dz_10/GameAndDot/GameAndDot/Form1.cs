using GameAndDot.Packages;
using GameAndDot.Packages.Enums;
using GameAndDot.Packages.Models;
using System.Net.Sockets;
using System.Text.Json;
using System.Drawing;

namespace GameAndDot
{
    public partial class Form1 : Form
    {
        private const string host = "127.0.0.1";
        private const int port = 8888;

        private readonly Graphics _g;
        private readonly TcpClient _client = new TcpClient();
        private readonly StreamReader? _reader = null;
        private readonly StreamWriter? _writer = null;
        private Color _userColor;

        public Form1()
        {
            InitializeComponent();

            _g = panel1.CreateGraphics();
            _g.Clear(Color.White);

            _client.Connect(host, port);
            _reader = new StreamReader(_client.GetStream());
            _writer = new StreamWriter(_client.GetStream());

            Task.Run(() => ReceiveMessageAsync());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            button1.Visible = false;
            textBox1.Visible = false;

            _userColor = GenerateRandomColor();
            string colorHex = ColorTranslator.ToHtml(_userColor);

            MessageObject pkg = new MessageObject
            {
                Type = MessageType.Register,
                Data = JsonSerializer.Serialize(new { Username = textBox1.Text, Color = colorHex })
            };

            string message = JsonSerializer.Serialize(pkg);
            await _writer.WriteLineAsync(message);
            await _writer.FlushAsync();
        }

        async Task ReceiveMessageAsync()
        {
            while (true)
            {
                try
                {
                    string? str = await _reader.ReadLineAsync();
                    if (str == null) continue;

                    var message = JsonSerializer.Deserialize<MessageObject>(str);

                    switch (message.Type)
                    {
                        case MessageType.Register:
                            var users = JsonSerializer.Deserialize<string[]>(message.Data);
                            listBox1.Invoke(() =>
                            {
                                listBox1.Items.Clear();
                                foreach (var username in users)
                                {
                                    listBox1.Items.Add(username);
                                }
                            });
                            break;

                        case MessageType.Draw:
                            var drawData = JsonSerializer.Deserialize<PointDrawData>(message.Data);
                            Color receivedColor = ColorTranslator.FromHtml(drawData.Color);

                            panel1.Invoke(() =>
                            {
                                _g.FillEllipse(new SolidBrush(receivedColor), drawData.X, drawData.Y, 6, 6);
                            });
                            break;
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        private async void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            var drawData = new PointDrawData
            {
                X = e.X,
                Y = e.Y,
                Color = ColorTranslator.ToHtml(_userColor)
            };

            MessageObject pkg = new MessageObject
            {
                Type = MessageType.Draw,
                Data = JsonSerializer.Serialize(drawData)
            };

            string message = JsonSerializer.Serialize(pkg);
            await _writer.WriteLineAsync(message);
            await _writer.FlushAsync();
        }

        private Color GenerateRandomColor()
        {
            Random rand = new Random();
            return Color.FromArgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256));
        }
    }

    public class PointDrawData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; } = "#000000";
    }
}
