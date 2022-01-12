using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace AbhishekAssignment
{
    class AbhishekClient
    {
        private const string Hostname = "en8fm6n2p76as.x.pipedream.net";
        private const int PortNumber = 443;

        public AbhishekClient()
        {
             Run(Hostname, PortNumber);
        }

        private Dictionary<string, string> getDefaultHeaders(string host)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Host", host);
            headers.Add("Connection", "keep-alive");
            headers.Add("User-Agent", "Abhishek Client 2.0");
            headers.Add("Accept", "*/*");
            headers.Add("Origin", "https://requestbin.com");
            headers.Add("Sec-Fetch-Site", "cross-site");
            headers.Add("Sec-Fetch-Mode", "cors");
            headers.Add("Sec-Fetch-Dest", "empty");
            headers.Add("Referer", "https://requestbin.com/");
            headers.Add("Accept-Encoding", "gzip, deflate, br");
            headers.Add("Accept-Language","en-US,en;q=0.9");
            return headers;
        }

        private string getRequestPayload(Dictionary<string, string> headers,string method,string host,string protocol,string path)
        {
            string message = $"{method} {protocol}://{host}/{path} HTTP/1.1\r\n";
            foreach ( (string headerName, string headerValue) in headers)
            {
                message += $"{headerName}: {headerValue}\r\n";
            }
            message += "\r\n";
            return message;
        }

        private void RunGETRequest(string host, StreamReader reader, StreamWriter writer)
        {
            Console.WriteLine("=====================================  GET REQUEST ==================================================");
            string requestPayload = getRequestPayload(getDefaultHeaders(host),"GET",host,protocol:"https",path:"");
            writer.Write(requestPayload);
            writer.Flush();
            PrintResponse(reader);
            Console.WriteLine("=====================================  GET REQUEST END ==================================================");
        }

        private void RunPOSTRequest(string host, StreamReader reader, StreamWriter writer)
        {
            Console.WriteLine("=====================================  POST REQUEST ==================================================");
            string sampleJSON = "{\"message\":\"Hello, I am Abhishek Client\"}";
            Dictionary<string, string> baseHeaders = getDefaultHeaders(host);
            baseHeaders.Add("content-type","application/json");
            baseHeaders.Add("Content-Length",sampleJSON.Length.ToString());
            string requestPayloadBase = getRequestPayload(baseHeaders,"POST",host,protocol:"https",path:"");
            string fullPostPayload = requestPayloadBase + sampleJSON;
            writer.Write(fullPostPayload);
            writer.Flush();
            PrintResponse(reader);
            Console.WriteLine("=====================================  POST REQUEST END ==================================================");
        }
        
        private void Run(string host, int port)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(host, port);
                SslStream sslStream = new SslStream(client.GetStream(),true);
                sslStream.AuthenticateAsClient(host);
                sslStream.ReadTimeout = 2000;
                StreamWriter writer = new StreamWriter(sslStream);
                writer.AutoFlush = true;
                StreamReader reader = new StreamReader(sslStream);
                RunPOSTRequest(host,reader,writer);
                RunGETRequest(host,reader,writer);
                reader.Close();
                writer.Close();
                sslStream.Close();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void PrintResponse(StreamReader reader)
        {
            char[] responseBuffer = new char[1024];
            reader.Read(responseBuffer,0,responseBuffer.Length);
            for (int characterIndex = 0; characterIndex < responseBuffer.Length; characterIndex++)
            {
                Console.Write(responseBuffer[characterIndex]);
            } 
            Console.WriteLine("");
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