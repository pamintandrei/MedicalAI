﻿using System;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Security.Authentication;
using System.Windows.Forms;

namespace Tiroida
{
    class ClientTCP
    {
        public static int CONECTIONSUCCESS = 1;
        public static int CONNECTIONFAIL = 0;
        public static int CONNECTIONFAILSSL = -1;
        public static string DEFAULTSERVERNAME = "MedicalAI";

        public static int MESSAGESENT = 1;
        public static int UNKNOWNERROR = 0;

        private string SERVERNAME;
        private string ServerIP { get; set; }
        private int ServerPort { get; set; }
        private Socket ClientSocket { get; set; }
        private int buffersize { get; set; }
        private Hashtable certificateErrors = new Hashtable();
        private SslStream sslStream;
        private bool rememberme;
        
        


        public string Username;
        public string Cookie;
        public bool isconnected;
        public bool isloged;
        public bool isadmin;

        public event EventHandler<OnReceiveMessageClientEventArgs> OnResponse;
        public event EventHandler<OnReceiveRegisterMessageArgs> OnRegisterResponse;
        public event EventHandler<OnReceiveLoginMessageArgs> OnLoginResponse;
        public event EventHandler<OnCodeVerifyResponseArgs> OnCodeVerifyResponse;
        public event EventHandler<OnReceiveResultArgs> OnReceiveResults;
        public event EventHandler<OnReceivePhotoResult> OnReceivePneumoniaResponse;
        public event EventHandler<OnReceiveChangePasswordResponseArgs> OnReceiveChangePasswordResponse;
        public event EventHandler<OnReceiveGetConfigResponseArgs> OnReceiveGetConfigResponse;
        public event EventHandler<OnReceiveNonMedicsArgs> OnReceiveNonMedics;
        public event EventHandler<OnReceiveNonMedicsArgs> OnReceiveMedics;
        public event EventHandler<OnReceiveSetConfigArgs> OnReceiveSetConfig;
        public event EventHandler<OnReceiveSetConfigArgs> OnReceiveAddMedic;
        public event EventHandler OnConnectionLost;

        public ClientTCP(string ServerIP, int ServerPort)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = 50000;
            this.isloged = false;
            this.SERVERNAME = DEFAULTSERVERNAME;
            this.isconnected = false;
            this.isadmin = false;
        }

        public ClientTCP(string ServerIP, int ServerPort, int buffersize)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = buffersize;
            this.isloged = false;
            this.SERVERNAME = "MedicalAI";
            this.isconnected = false;
            this.isadmin = false;
        }

        public ClientTCP(string ServerIP, int ServerPort, int buffersize, bool rememberme)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = buffersize;
            this.isloged = false;
            this.SERVERNAME = "MedicalAI";
            this.rememberme = rememberme;
            this.isconnected = false;
            this.isadmin = false;
        }

        public ClientTCP(string ServerIP, int ServerPort, int buffersize, string SERVERNAME)
        {
            this.ServerIP = ServerIP;
            this.ServerPort = ServerPort;
            this.buffersize = buffersize;
            this.isloged = false;
            this.SERVERNAME = SERVERNAME;
            this.isconnected = false;
            this.isadmin = false;
        }

        public void SetIp(string ServerIP)
        {
            this.ServerIP = ServerIP;
        }

        public void SetPort(int ServerPort)
        {
            this.ServerPort = ServerPort;
        }



        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslErrors)
        {
            if (sslErrors == SslPolicyErrors.None)
                return true;

            if (chain.ChainStatus.Length == 1)
            {
                if (sslErrors == SslPolicyErrors.RemoteCertificateChainErrors || certificate.Subject == certificate.Issuer)
                {
                    if (chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot)

                    {


                        return true;

                    }
                }

            }


            Console.WriteLine("Error ssl, Invalid certificate : {0}", sslErrors);

            return false;
        }



        public int DoConnection()
        {
            try
            {
                this.ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(this.ServerIP), this.ServerPort);
                Console.WriteLine(this.ServerIP);
                this.ClientSocket.Connect(endpoint);

                NetworkStream SocketStream = new NetworkStream(this.ClientSocket,true);
                sslStream = new SslStream(SocketStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);


                Console.WriteLine("Connecting to SSL Certificate .." + this.SERVERNAME);

                try
                {
                    sslStream.AuthenticateAsClient(this.SERVERNAME);
                }
                catch (AuthenticationException e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                    }
                    Console.WriteLine("Authentication failed - closing the connection.");
                    SocketStream.Close();
                    return CONNECTIONFAILSSL;
                }

                Console.WriteLine("Connection to SSL Certificate succesfully done.." + this.SERVERNAME);
                

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
                stringmessage += "<EOF>";
                
                byte[] bytemessage = Encoding.Default.GetBytes(stringmessage);
                this.sslStream.Write(bytemessage);
                this.sslStream.Flush();
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
                    //this..Receive(receivebuffer, 0, receivebuffer.Length, 0);
                    //string response = Encoding.ASCII.GetString(receivebuffer);

                    StringBuilder messageData = new StringBuilder();
                    int bytes = -1;
                    do
                    {

                        bytes = sslStream.Read(receivebuffer, 0, receivebuffer.Length);
                        Decoder decoder = Encoding.UTF8.GetDecoder();
                        char[] messagepart = new char[decoder.GetCharCount(receivebuffer, 0, bytes)];
                        decoder.GetChars(receivebuffer, 0, bytes, messagepart, 0);
                        messageData.Append(messagepart);
                        if (messageData.ToString().IndexOf("<EOF>") != -1)
                        {
                            break;
                        }



                    } while (bytes != 0);

                    string response = messageData.ToString();
                    response = response.Substring(0,response.Length-5);




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

                            problem.Chanse_To_Have = obj["rezultat"][0][0].ToObject<double>();
                            problem.Chanse_To_Have_Nothing = obj["rezultat"][1][1].ToObject<double>();

                            //Console.WriteLine("Problem with invoke shit");
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

                            if (errorcodee == 0)
                            {
                                this.Cookie = (string)obj["cookie"];
                                this.isloged = true;
                            }

                            OnLoginResponse?.Invoke(this, new OnReceiveLoginMessageArgs { errorcode = errorcodee, errormessage = (string)obj["errormessage"], username = (string)obj["username"], is_admin = (bool)obj["is_admin"] });
                            this.isadmin = (bool)obj["is_admin"];
                            break;

                        case "code_verify_response":
                            Console.WriteLine("Receiving code_verify response");
                            OnCodeVerifyResponse?.Invoke(this, new OnCodeVerifyResponseArgs { errorcode = (int)obj["errorcode"], errormessage = (string)obj["errormessage"] });


                            break;


                        case "results":
                            int errorcode = (int)obj["error"];

                            if (errorcode == 0)
                            {

                                OnReceiveResultArgs args = JsonConvert.DeserializeObject<OnReceiveResultArgs>(response);

                                OnReceiveResults?.Invoke(this, args);
                            }
                            else
                            {

                            }




                            break;


                        case "photoresult":
                            OnReceivePhotoResult pneoargs = new OnReceivePhotoResult((string)obj["rezultat"][0][0], (string)obj["rezultat"][0][0]);
                            OnReceivePneumoniaResponse?.Invoke(this, pneoargs);
                            break;


                        case "changepasswordresult":
                            OnReceiveChangePasswordResponseArgs passchangeargs = new OnReceiveChangePasswordResponseArgs((int)obj["errcode"], (string)obj["errmessage"]);

                            if ((int)obj["errcode"] == 0)
                            {
                                this.Cookie = (string)obj["cookie"];
                            }
                            OnReceiveChangePasswordResponse?.Invoke(this, passchangeargs);
                           
                            break;

                        case "configresult":
                            if ((int)obj["errcode"] == 0)
                            {
                                OnReceiveGetConfigResponseArgs config = new OnReceiveGetConfigResponseArgs((bool)obj["register_verification"], (bool)obj["medic_registration"]);
                                OnReceiveGetConfigResponse?.Invoke(this, config);
                            }
                            else
                            {
                                MessageBox.Show("Nu sunteti conectat cu contul de admin!", "MedicalAI");
                            }
                            break;

                        case "nonmedicsresult":
                            if ((int)obj["errcode"] == 0)
                            {
                                Console.WriteLine(response);
                                OnReceiveNonMedicsArgs config = JsonConvert.DeserializeObject<OnReceiveNonMedicsArgs>(response);
                                OnReceiveNonMedics?.Invoke(this, config);

                            }

                            break;


                        case "medicsresult":
                            if ((int)obj["errcode"] == 0)
                            {
                                Console.WriteLine(response);
                                OnReceiveNonMedicsArgs config = JsonConvert.DeserializeObject<OnReceiveNonMedicsArgs>(response);
                                OnReceiveMedics?.Invoke(this, config);
                            }
                            break;

                        case "setconfigresult":
                            if ((int)obj["errcode"] == 0)
                            {
                                OnReceiveSetConfigArgs config = new OnReceiveSetConfigArgs((int)obj["errcode"], (string)obj["errmessage"]);
                                OnReceiveSetConfig?.Invoke(this, config);
                            }
                            else
                            {
                                MessageBox.Show("Invalid cookie");
                            }

                            break;

                        case "addmedicresponse":
                            if((int)obj["errcode"] == 0)
                            {

                                OnReceiveSetConfigArgs config = new OnReceiveSetConfigArgs((int)obj["errcode"], (string)obj["errmessage"]);
                                OnReceiveAddMedic?.Invoke(this,config);
                            }

                            break;
                        case "removemedicresponse":
                            if ((int)obj["errcode"] == 0)
                            {

                                OnReceiveSetConfigArgs config = new OnReceiveSetConfigArgs((int)obj["errcode"], (string)obj["errmessage"]);
                                OnReceiveAddMedic?.Invoke(this, config);
                            }

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
                    this.isloged = false;
                    break;
                }
            }
        }


    }
}
