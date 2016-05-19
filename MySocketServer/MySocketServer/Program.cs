using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom;
namespace MySocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            IPHostEntry ipHost = Dns.Resolve("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 12345);

            Socket sListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                sListener.Bind(ipEndPoint);

                sListener.Listen(10);

                while(true)
                {
                    Console.WriteLine("Waiting for a connection on port 12345: {0}", ipEndPoint);
                    Socket h = sListener.Accept();
                    if(h.Connected)
                        Console.WriteLine("Connect");

                    string receive = ""; 
                    while (true)
                    {
                        
                        byte[] bytes = new byte[1024];
                        int length = h.Receive(bytes);

                        receive = Encoding.ASCII.GetString(bytes, 0, length);
                        
                        
                        Console.Write(receive);

                        
                        if (receive.IndexOf("Exit") > -1)
                        {
                            break;
                        }
                        else if (receive.IndexOf("\r\n") > -1)
                        {
                            string csFile = "code.cs";
                            using (FileStream fs = new FileStream(csFile, FileMode.OpenOrCreate))
                            {
                                fs.Write(bytes, 0, length);
                                fs.Flush();

                                
                            }
                            Compile(csFile);
                            receive = "";
                            byte[] send;
                            send = Encoding.ASCII.GetBytes((String.Format("{0}\n", DateTime.Now.ToShortTimeString().Trim())));
                            h.Send(send);
                        }
                    }
                    Console.WriteLine(receive);

                    byte[] returning = Encoding.ASCII.GetBytes("accept");
                    h.Send(returning);
                    h.Shutdown(SocketShutdown.Both);
                    h.Close();
                }

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static string Compile(string csFilePath)
        {
            
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("CompilerVersion", "v3.5");
            CSharpCodeProvider csCodeProvider = new CSharpCodeProvider(options);
            TextReader codeStream = new StreamReader(csFilePath);
            CodeCompileUnit codeCompile = csCodeProvider.Parse(File.OpenText(csFilePath));
            Console.WriteLine(codeCompile.ToString());
            return "";
        }
    }
}
