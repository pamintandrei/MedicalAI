using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Tiroida
{
    class ClientTCP
    {
        public static int CONECTIONSUCCESS = 1;
        public static int CONNECTIONFAIL = 0;

        public static int MESSAGESENT = 1;
        public static int UNKNOWNERROR = 0;

        private string ServerIP { get; set; }
        private int ServerPort { get; set; }
        private Socket ClientSocket { get; set; }
        private int buffersize { get; set; }

        public string Username;
        public string Cookie;

        public event EventHandler<OnReceiveMessageClientEventArgs> OnResponse;
        public event EventHandler<OnReceiveRegisterMessageArgs> OnRegisterResponse;
        public event EventHandler<OnReceiveLoginMessageArgs> OnLoginResponse;
        public event EventHandler<OnCodeVerifyResponseArgs> OnCodeVerifyResponse;
        public event EventHandler OnConnectionLost;

        public ClientTCP(string ServerIP, int ServerPort)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = 50000;
            
        }
        public ClientTCP(string ServerIP, int ServerPort, int buffersize)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = buffersize;
            
        }

        public int DoConnection()
        {
            try
            {
                this.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(this.ServerIP), this.ServerPort);
                Console.WriteLine(this.ServerIP);
                this.ClientSocket.Connect(endpoint);

                Thread receive = new Thread(Response);
                receive.Start();

                return CONECTIONSUCCESS;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return CONNECTIONFAIL;
            }

        }

        public int SendContent(string stringmessage)
        {
            try
            {
                byte[] bytemessage = Encoding.Default.GetBytes(stringmessage);
                this.ClientSocket.Send(bytemessage, 0, bytemessage.Length, 0);
                return MESSAGESENT;
            }
            catch (Exception ex)
            {
                return UNKNOWNERROR;
            }
        }

        public void Response()
        {
            while (true)
            {
                try
                {
                    byte[] receivebuffer = new byte[this.buffersize];
                    this.ClientSocket.Receive(receivebuffer, 0, receivebuffer.Length, 0);
                    string response = Encoding.ASCII.GetString(receivebuffer);
                    Console.WriteLine(response);
                    JObject obj = JObject.Parse(response);
                    if (obj["action"] == null)
                    {
                        /*
                         *  Error shit
                         *  To do:
                         *  Write a file with the logs
                         *  Creating the MedicalProblems object
                         * 
                         * 
                         */
                        Console.WriteLine("No action has been sent!");

                        return;
                    }
                    string action = (string)obj["action"];
                    switch (action)
                    {
                        case "response":
                            MedicalProblems problem = new MedicalProblems();
                            Console.WriteLine("yes");
                            Console.WriteLine(obj["rezultat"][0][0].GetType());
                            Console.WriteLine(obj["rezultat"][1][1]);
                            problem.Chanse_To_Have = obj["rezultat"][0][0].ToObject<double>();
                            problem.Chanse_To_Have_Nothing = obj["rezultat"][1][1].ToObject<double>();

                            Console.WriteLine("Problem with invoke shit");
                            OnResponse?.Invoke(this, new OnReceiveMessageClientEventArgs { Medical = problem });

                            break;

                        case "regresponse":
                            Console.WriteLine("Receiving register response");
                            OnRegisterResponse?.Invoke(this, new OnReceiveRegisterMessageArgs { errorcode = (int)obj["errorcode"], errormessage = (string)obj["errormessage"] });
                            
                            break;

                        case "loginresponse":
                            //Console.WriteLine("Receiving login response");
                            Console.WriteLine("Code..... \n");
                            string errorcodeestring = (string)obj["error"];
                            Console.WriteLine(errorcodeestring);
                            int errorcodee = Int32.Parse(errorcodeestring);
                            Console.WriteLine("Cod de eroare: " + errorcodee);
                            OnLoginResponse?.Invoke(this, new OnReceiveLoginMessageArgs { errorcode = errorcodee, errormessage = (string)obj["errormessage"], username = (string)obj["username"] });

                            if (errorcodee == 0)
                            {
                                this.Cookie = (string)obj["cookie"];
                            }


                            break;

                        case "code_verify_response":
                            Console.WriteLine("Receiving code_verify response");
                            OnCodeVerifyResponse?.Invoke(this, new OnCodeVerifyResponseArgs { errorcode = (int)obj["errorcode"], errormessage = (string)obj["errormessage"]});


                            break;
                        default:
                            /*
                             * Some error comming from server
                             * or maybe from client?
                             * IDK
                             * 
                             * */
                            break;
                    }
                }
                catch (Exception ex)
                {
                    OnConnectionLost?.Invoke(this, EventArgs.Empty);
                    Console.WriteLine("Wrong format " + ex.Message);
                    break;
                }
            }
        }


    }
}
