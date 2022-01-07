using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AbhishekAssignment
{
    class AbhishekClient
    {
        private const string Hostname = "www.google.com";
        private const int PortNumber = 80;

        public AbhishekClient()
        {
             Run(Hostname, PortNumber);
        }

        private Dictionary<string, string> getDefaultHeaders(string host)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Accept", "text/html");
            headers.Add("Connection", "close");
            headers.Add("Host", host);
            headers.Add("User-Agent", "Abhishek-Client");
            return headers;
        }

        private string getRequestPayload(Dictionary<string, string> headers)
        {
            string message = "GET / HTTP/1.1\r\n";
            foreach ( (string headerName, string headerValue) in headers)
            {
                message += $"{headerName}: {headerValue}\r\n";
            }
            message += "\r\n";
            return message;
        }

        private void Run(string host, int port)
        {
            try
            {
                string requestPayload = getRequestPayload(getDefaultHeaders(host));
                TcpClient client = new TcpClient();
                client.Connect(host, 80);
                NetworkStream networkStream = client.GetStream();
                networkStream.ReadTimeout = 2000;
                StreamWriter writer = new StreamWriter(networkStream);
                StreamReader reader = new StreamReader(networkStream, Encoding.UTF8);
                byte[] bytes = Encoding.UTF8.GetBytes(requestPayload);
                networkStream.Write(bytes, 0, bytes.Length);
                string response = reader.ReadToEnd();
                Console.WriteLine(response);
                /**
                 * By using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http, I can extract body
                 * https://stackoverflow.com/questions/318506/converting-raw-http-request-into-httpwebrequest-object
                 */
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}

class Program
{
    public static void Main()
    {
        AbhishekAssignment.AbhishekClient obj = new AbhishekAssignment.AbhishekClient();
    }
}