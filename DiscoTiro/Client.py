import socket
import event
import threading
import json
import response
import errno
import time
import ssl

class TcpClient:
    def __init__(self, ip, port, buffersize = 10000):
        self.on_connected = event.Event()
        self.on_connection_lost = event.Event()
        self.on_response = event.Event()
        self.ip = ip
        self.port = port
        self.buffersize = buffersize
        self.sslcontext = ssl.SSLContext(cert_reqs=ssl.CERT_OPTIONAL)
        self.mainsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.connected = False
		

    def DoConnectionUntilConnected(self):
        self.mainsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        while True:
            try:
                self.mainsocket.connect((self.ip, self.port))
                self.encryptionconn = self.sslcontext.wrap_socket(self.mainsocket, server_hostname="MedicalAI")
                self.connected = True
                self.on_connected(self)
                break
            except socket.error as error:
                print("Error while connecting to the AI server. " + str(error))

            time.sleep(6)




    def get_service_status(self):
        return self.connected

    async def SendData(self, data):
        try:
            data += "<EOF>"
            self.encryptionconn.send(data.encode())
            recvdata = await self.RecvData()
            recvdata = recvdata.decode('UTF-8')
            if not recvdata:
                self.connected = False
                return 1
			
			
            recvdata = recvdata[:-5]
            jsonobj = json.loads(recvdata)
            if jsonobj['action'] == 'response':
                return response.response(jsonobj["rezultat"][0][0], jsonobj["rezultat"][1][1])

        except socket.error as e:
            print("Error in sending data. " + str(e))
            self.connected = False
            self.mainsocket.close()
            self.on_connection_lost(self)
            return 1
			
			
    async def RecvData(self):
        data = b''
        while True:
            data += self.encryptionconn.recv(4096)
            if data.decode('UTF-8')[-5:] == "<EOF>":
                return data

"""
    def Receiver(self):
        while True:
            data = self.mainsocket.recv(buffersize = self.buffersize)
            if not data:
                self.on_connection_lost(self)
                break

            jsonstring = data.decode('utf-8')
            jsonobj =  json.loads(jsonstring)
            action = jsonobj['action']
            if action == 'response':
                self.on_response(self)
            else:
                print("Error from server side")
"""

