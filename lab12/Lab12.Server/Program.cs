using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab12.Server
{
    class Program
    {
        private static int _clientCounter = 0;

        static async Task Main(string[] args)
        {
            var listener = new TcpListener(IPAddress.Any, 12345);
            listener.Start();
            Console.WriteLine("Server started. Waiting for clients...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                _ = Task.Run(() => HandleClient(client, ++_clientCounter));
            }
        }

        private static async Task HandleClient(TcpClient client, int clientId)
        {
            try
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream) { AutoFlush = true };

                await writer.WriteLineAsync(JsonSerializer.Serialize(clientId));
                Console.WriteLine($"Assigned ID {clientId} to client.");

                while (true)
                {
                    var data = await reader.ReadLineAsync();
                    if (data == null) break;

                    var message = JsonSerializer.Deserialize<ClientMessage>(data);
                    message.Timestamp = message.Timestamp.AddYears(message.MessageNumber);

                    await writer.WriteLineAsync(JsonSerializer.Serialize(message));
                    Console.WriteLine($"Processed message number {message.MessageNumber} from client {message.ClientId} with timestamp {message.Timestamp}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client {clientId} disconnected: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine($"Client {clientId} has been disconnected.");
            }
        }
    }

    public class ClientMessage
    {
        public int ClientId { get; set; }
        public int MessageNumber { get; set; }
        public DateTime Timestamp { get; set; }
    }
}