import socket
import event
import threading
import json
import response
import errno
import time

class TcpClient:
    def __init__(self, ip, port, buffersize = 10000):
        self.on_connected = event.Event()
        self.on_connection_lost = event.Event()
        self.on_response = event.Event()
        self.ip = ip
        self.port = port
        self.buffersize = buffersize
        self.mainsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.connected = False

    def DoConnectionUntilConnected(self):
        self.mainsocket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        while True:
            try:
                self.mainsocket.connect((self.ip, self.port))
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
            self.mainsocket.send(data.encode())
            recvdata = self.mainsocket.recv(self.buffersize)

            if not recvdata:
                self.connected = False
                return 1

            jsonobj = json.loads(recvdata.decode('utf-8'))
            if jsonobj['action'] == 'response':
                return response.response(jsonobj["rezultat"][0][0], jsonobj["rezultat"][1][1])

        except socket.error as e:
            print("Error in sending data. " + str(e))
            self.connected = False
            self.mainsocket.close()
            self.on_connection_lost(self)
            return 1

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

