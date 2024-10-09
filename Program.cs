using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

/*class SimpleTCPClient
{
    static void Main(string[] args)
    {
        try
        {
            TcpClient clientSocket = new TcpClient("127.0.0.1", 14000);
            Console.WriteLine("Client is ready");

            Stream ns = clientSocket.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };

            while (true)
            {
                Console.WriteLine("Write your message here (type 'exit' to quit): ");
                string message = Console.ReadLine();

                if (message.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                sw.WriteLine(message);
                string serverAnswer = sr.ReadLine();
                Console.WriteLine("Server answer: " + serverAnswer);
            }

            ns.Close();
            clientSocket.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
// Could do funny startAGAIN here but no :3
*/
internal class JsonTcpClient
{
    public void Start()
    {
        try
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 14000))
            using (NetworkStream ns = client.GetStream())
            using (StreamReader sr = new StreamReader(ns))
            using (StreamWriter sw = new StreamWriter(ns) { AutoFlush = true })
            {
                Console.WriteLine("Enter command (Random, Add, Subtract):");
                string method = Console.ReadLine();

                if (string.IsNullOrEmpty(method))
                {
                    Console.WriteLine("Method cannot be empty.");
                    return;
                }

                Console.WriteLine("Enter first number:");
                if (!int.TryParse(Console.ReadLine(), out int tal1))
                {
                    Console.WriteLine("Invalid input for first number.");
                    return;
                }

                Console.WriteLine("Enter second number:");
                if (!int.TryParse(Console.ReadLine(), out int tal2))
                {
                    Console.WriteLine("Invalid input for second number.");
                    return;
                }

                var request = new
                {
                    Method = method.ToLower(),
                    Tal1 = tal1,
                    Tal2 = tal2
                };

                string jsonRequest = JsonSerializer.Serialize(request);
                Console.WriteLine($"Sending: {jsonRequest}");
                sw.WriteLine(jsonRequest);

                string jsonResponse = sr.ReadLine();

                if (string.IsNullOrEmpty(jsonResponse))
                {
                    Console.WriteLine("Empty response from server.");
                    return;
                }

                Console.WriteLine($"Received: {jsonResponse}");
                var response = JsonSerializer.Deserialize<Response>(jsonResponse);

                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.Error))
                    {
                        Console.WriteLine($"Error: {response.Error}");
                    }
                    else
                    {
                        Console.WriteLine($"Result: {response.Result}");
                    }
                }
                else
                {
                    Console.WriteLine("Deserialization error.");
                }
            }
        }
        catch (SocketException se)
        {
            Console.WriteLine("Socket error: " + se.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    private class Response
    {
        public string Result { get; set; }
        public string Error { get; set; }
    }

    public static void Main(string[] args)
    {
        var client = new JsonTcpClient();
        client.Start();
    }
}





