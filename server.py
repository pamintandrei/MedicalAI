import socket
import threading
import json
import tensorflow as tf
import numpy as np

datedeintrare=np.array([[]])
datedeintrare.resize((2,23))


socklistener = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

socklistener.bind(('0.0.0.0', 5554))

socklistener.listen(1)


allconnection = []

def datesauintrebare(fiecarelinie,dparcurs):
        if(fiecarelinie!="?"):
            datedeintrare[0][dparcurs]=fiecarelinie
            datedeintrare[1][dparcurs]=fiecarelinie
        else:
            datedeintrare[0][dparcurs]=0
            datedeintrare[1][dparcurs]=0

def truf(fiecarelinie,dparcurs):
        if(fiecarelinie=="f"):
            datedeintrare[0][dparcurs]=-1
            datedeintrare[1][dparcurs]=-1
        else:
            datedeintrare[0][dparcurs]=1
            datedeintrare[1][dparcurs]=1

            
def yon(fiecarelinie,dparcurs):
           
        if(fiecarelinie=="n"):
            datedeintrare[0][dparcurs]=-1
            datedeintrare[1][dparcurs]=-1
        if(fiecarelinie=="y"):
            datedeintrare[0][dparcurs]=1
            datedeintrare[1][dparcurs]=1
        if(fiecarelinie=="?"):
            datedeintrare[0][dparcurs]=0  
            datedeintrare[1][dparcurs]=0

def pamantfunction(recvdata):

    neural_net=tf.keras.models.load_model('tester3.h5')


        
    datesauintrebare(recvdata['age'],0)   
        
    if(recvdata['sex']=="M"):
        datedeintrare[0][1]=1
        datedeintrare[1][1]=1
    if(recvdata['sex']=="F"):
        datedeintrare[0][1]=-1
        datedeintrare[1][1]=-1
    if(recvdata['sex']=="?"):
        datedeintrare[0][1]=0
        datedeintrare[1][1]=0

    

    truf(recvdata['on_thyroxine'],2)  
    truf(recvdata['query_on_thyroxine'],3)  
    truf(recvdata['on_antithyroid_medication'],4)  
    truf(recvdata['thyroid_surgery'],5)  
    truf(recvdata['query_hypothyroid'],6)  
    truf(recvdata['query_hyperthyroid'],7)  
    truf(recvdata['pregnant'],8)  
    truf(recvdata['sick'],9)  
    truf(recvdata['tumor'],10)  
    truf(recvdata['lithium'],11)  
    truf(recvdata['goitre'],12)  
    truf(recvdata['TSH_measured'],13)  
    datesauintrebare(recvdata['TSH'],14)
    truf(recvdata['T3_measured'],15)  
    datesauintrebare(recvdata['T3'],16)
    truf(recvdata['TT4_measured'],17)  
    datesauintrebare(recvdata['TT4'],18)
    truf(recvdata['FTI_measured'],19)  
    datesauintrebare(recvdata['FTI'],20)
    truf(recvdata['TBG_measured'],21)  
    datesauintrebare(recvdata['TBG'],22)
    
    rezultat = neural_net.predict(x)
    data = {}
    data['rezultat'] = rezultat
    json_data = json.dumps(data)
    return json_data
    
    '''
    aici pamant isi va face de cap
    ps: vezi sa dai return la final de cod cu raspunsul in format json
    '''





def handler(c, a):
    while True:
        data = c.recv(4096)
        if not data:
            break

        stringjson = data.decode('utf-8')
        response = pamantfunction(json.loads(stringjson))

        c.send(response.encode())
        print(stringjson)

    print("Lost connection: " + str(a))


while True:
    c, a = socklistener.accept()
    allconnection.append(c)

    newthread = threading.Thread(target=handler, args=(c, a))
    newthread.daemon = True
    newthread.start()

    print("New connection available: " + str(a))