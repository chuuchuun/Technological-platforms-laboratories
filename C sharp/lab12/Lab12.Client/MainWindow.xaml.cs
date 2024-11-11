using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab12.Client
{
    public partial class MainWindow : Window
    {
        private TcpClient? _client = null;
        private bool _stop = false;
        private Task? _checkConnectionTask = null;
        private int _clientId;
        private int _messageNumber = 0;
        private Timer? _sendTimer = null;

        public MainWindow()
        {
            InitializeComponent();
            _checkConnectionTask = Task.Run(async () =>
            {
                while (!_stop)
                {
                    if (_client is not null && !_client.Connected)
                    {
                        _client = null;
                        UpdateUIForDisconnectedState();
                        ShowDisconnectionMessage();
                    }

                    await Task.Delay(1000);
                }
            });
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            _client?.Close();

            try
            {
                _client = new TcpClient(IPAddress.Loopback.ToString(), 12345);
                var stream = _client.GetStream();
                var reader = new StreamReader(stream);

                var clientIdString = await reader.ReadLineAsync();
                _clientId = JsonSerializer.Deserialize<int>(clientIdString!);
                ClientIdTextBox.Text = _clientId.ToString();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Failed to connect to server: {ex.Message}");
                Status.Text = "Failed to connect!";
                MessageBox.Show($"Failed to connect to server: {ex.Message}");
                return;
            }

            Status.Text = "Connected";
            SendButton.IsEnabled = true;
            DisconnectButton.IsEnabled = true;
            ConnectButton.IsEnabled = false;
        }

        private void SendButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_client is null)
            {
                MessageBox.Show("Connect to the server first");
                return;
            }

            _sendTimer = new Timer(SendMessage, null, 0, 5000);
        }

        private async void SendMessage(object? state)
        {
            var message = new ClientMessage
            {
                ClientId = _clientId,
                MessageNumber = ++_messageNumber,
                Timestamp = DateTime.Now
            };

            Dispatcher.Invoke(() =>
            {
                MessageNumberTextBox.Text = message.MessageNumber.ToString();
                TimestampTextBox.Text = message.Timestamp.ToString("O");
            });

            try
            {
                var result = await Task.Run(async () =>
                {
                    var stream = _client.GetStream();
                    var streamWriter = new StreamWriter(stream) { AutoFlush = true };
                    await streamWriter.WriteLineAsync(JsonSerializer.Serialize(message));

                    var streamReader = new StreamReader(stream);
                    var newData = JsonSerializer.Deserialize<ClientMessage>((await streamReader.ReadLineAsync())!);
                    return newData;
                });

                Dispatcher.Invoke(() =>
                {
                    ResultTextBox.Text = $"ID: {result.ClientId}, Number: {result.MessageNumber}, Timestamp: {result.Timestamp}";
                    Status.Text = "Done";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    Status.Text = $"Error: {ex.Message}";
                });
            }
        }

        private void DisconnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            _client?.Close();
            _client = null;
            _sendTimer?.Dispose();
            UpdateUIForDisconnectedState();
        }

        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            _stop = true;
            _checkConnectionTask?.Wait();
            _client?.Close();
            _sendTimer?.Dispose();
        }

        private void UpdateUIForDisconnectedState()
        {
            Status.Dispatcher.Invoke(() =>
            {
                Status.Text = "Disconnected";
            });
            DisconnectButton.Dispatcher.Invoke(() =>
            {
                DisconnectButton.IsEnabled = false;
            });
            ConnectButton.Dispatcher.Invoke(() =>
            {
                ConnectButton.IsEnabled = true;
            });
            SendButton.Dispatcher.Invoke(() =>
            {
                SendButton.IsEnabled = false;
            });
            ClientIdTextBox.Dispatcher.Invoke(() =>
            {
                ClientIdTextBox.Text = string.Empty;
            });
        }

        private void ShowDisconnectionMessage()
        {
            MessageBox.Show("You have been disconnected from the server.");
        }
    }

    public class ClientMessage
    {
        public int ClientId { get; set; }
        public int MessageNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}