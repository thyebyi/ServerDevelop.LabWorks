using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using TcpPrac;

namespace TcpServer
{
    class BasicHttpHandler : TcpHandler
    {
        public BasicHttpHandler(TcpClient client) : base(client)
        { }

        public string GETResponse(HttpStatusCode code, string contentType, int length)
        {
            StringBuilder headers = new StringBuilder().
                AppendLine("HTTP/1.1 " + ((Int16)code).ToString() + " " + code.ToString()).
                AppendLine("Content-type: " + contentType).
                AppendLine("Content-Length: " + length.ToString()).
                AppendLine();
            return headers.ToString();
        }

        public string GETRequest()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = client.GetStream().Read(data, 0, data.Length);
                builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
            }
            while (client.GetStream().DataAvailable);
            string message = builder.ToString();
            string lines = message.Split('\n')[0];
            string[] headers = lines.Split(' ');
            foreach (var header in headers)
                header.Trim();
            if (headers[0] != "GET")
                throw new HttpListenerException(400);
            return headers[1].Substring(1);
            /*
             * string[] headerOnlyPathMas = headers[1].Split('/');
            string headerOnlyPath = "";
            for (int i = 0; i < headerOnlyPathMas.Length - 1; i++)
                headerOnlyPath += headerOnlyPathMas[i] + '/';
            headerOnlyPath = "." + headerOnlyPath;
            //string textMessage = $"<script src=\"js/jquery-3.3.1.min.js\"></script>";
            string[] parts = message.Split(' ');
            string toReturn = "";
            string sep = "src=\"";
            for (int i = 0; i < parts.Length; i++)
            {
                sep = "src=\"";
                int srcPos = parts[i].IndexOf(sep);
                if (srcPos == -1)
                {
                    sep = "href=\"";
                    srcPos = parts[i].IndexOf(sep);
                    if (srcPos != -1)
                    {
                        if (parts[i][srcPos + sep.Length] == '#')
                            continue;
                        if (parts[i - 1].IndexOf("link") != -1)
                            continue;
                    }
                    else
                    {
                        sep = "url('";
                        srcPos = parts[i].IndexOf(sep);
                        if (srcPos == -1)
                            continue;
                    }
                    //if (srcPos != 0)
                    //    if (parts[i].Substring(srcPos - 4, 8) == "linkhref")
                    //        continue;
                }
                parts[i] = parts[i].Insert(srcPos + sep.Length, headerOnlyPath);
                int j = 0;
            }
            foreach (var p in parts)
                toReturn += p + " ";
            return toReturn;
            */
        }

        public override void Process()
        {
            try
            {
                NetworkStream output = client.GetStream();
                string body3 = GETRequest();

                //string body1 = "<!DOCTYPE html><html><body><p>";
                //string body2 = "</p></body></html>";
                string body = body3;
                string response = GETResponse(HttpStatusCode.OK, "text/html", body.Length);
                Console.WriteLine(response);
                byte[] buffer = Encoding.ASCII.GetBytes(response + body);
                output.Write(buffer, 0, buffer.Length);

                //buffer = Encoding.ASCII.GetBytes(body);
                //output.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
