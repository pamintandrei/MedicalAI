import socket
import threading
import json
import tensorflow as tf
import numpy as np
import sqlite3
from datetime import datetime
import secrets
import smtplib
datedeintrare=np.array([[]])
datedeintrare.resize((2,23))


server = smtplib.SMTP('smtp.gmail.com', 587)
server.connect('smtp.gmail.com', 587)
server.ehlo()
server.starttls()
server.ehlo()
server.login('infoeducatietiroida','cartofel')


socklistener = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

socklistener.bind(('0.0.0.0', 5554))
intrari=0
socklistener.listen(1)


allconnection = []


def pusinbaza(utilizator,cur,conn):

    cur.executemany("INSERT INTO baza VALUES(?,?,?,?,?,?,?,?)",utilizator)
    conn.commit()
    
def gasitinbaza(column,fromuser,cur):
    t = (fromuser, )
    cur.execute("SELECT * FROM baza WHERE "+column+"=?", t)
    columns = cur.fetchall()
    if(columns):
        return 1
    else:
        return 0
		

def datesauintrebare(fiecarelinie,dparcurs):
        if(fiecarelinie!="?" and fiecarelinie!=None):
            datedeintrare[0][dparcurs]=fiecarelinie
            datedeintrare[1][dparcurs]=fiecarelinie
        else:
            datedeintrare[0][dparcurs]=0
            datedeintrare[1][dparcurs]=0
def truf(fiecarelinie,dparcurs):
        if(fiecarelinie=="f" or fiecarelinie==None):
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
        if(fiecarelinie=="?" or fiecarelinie=="None"):
            datedeintrare[0][dparcurs]=0  
            datedeintrare[1][dparcurs]=0

			
'''
ADAUGAT: cod 2 pentru utilizatorii neverificati la email
'''


def id_cookie(cookie):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data=([cookie])
    cur.execute("SELECT * FROM baza where cookie=?",data)
    toateinformatiile= cur.fetchall()
    try:
        return toateinformatiile[0][0]
    except:
        return -1
		

		
def inset_medical_tests(medic_id,data):
	conn = sqlite3.connect('bazadedate.db')
	cur = conn.cursor()
	now=datetime.now()
	timestamp=datetime.timestamp(now)
	inser_data = ([None,data['patinet_name'], data['Sex'], data['Age'], data['on_thyroxine'], data['query_on_thyroxine'], data['on_antithyroid_medication'], data['thyroid_surgery'], data['query_hypothyroid'], data['query_hyperthyroid'], data['pregnant'], data['sick'], data['tumor'], data['lithium'], data['goitre'], data['TSH_measured'], data['TSH'], data['T3_measured'], data['T3'], data['TT4_measured'], data['TT4'], data['FTI_measured'], data['FTI'], data['TBG_measured'], data['TBG'], medic_id, timestamp ])
	cur.execute("INSERT INTO analyzes_patient VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", inser_data)
	conn.commit()
	
	
'''
print(id_cookie(''))
'''
def login(recvdata):
    data = {}
    data['action'] = "loginresponse"
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    caut=gasitinbaza('username', recvdata['username'],cur)
    usr = " "
    if(caut):
        usr=([recvdata['username']])
        cur.execute("SELECT * FROM baza WHERE username=?", usr)
        parolabaza=cur.fetchall()
        if(parolabaza[0][2]==recvdata['password']):
            '''
			de editat:
			- trebuie editata functia in acest if pentru codul de verificare
			- in cazul in care nu este verificat error va fi 2
			'''
            confirm = confirmation(recvdata)
			
			
            if(confirm == 2):
                data['error'] = '0'
                data['cookie'] = parolabaza[0][4]
                data['errormessage'] = "Logare cu succes!"
            else:
                data['error'] = '2'
                data['errormessage'] = "Cont neverificat!"
        else:
            data['error'] = '1'
            data['errormessage'] = "Nume de utilizator sau parola incorecta!"
    else:
        data['error'] = '1'
        data['errormessage'] = "Nume de utilizator sau parola incorecta!"
		
    data['username'] = recvdata['username']
    json_data = json.dumps(data)
    print(data)
    
    return json_data

	
'''
Functia de jos trebuie editata pentru utilizatorii logati
In caz de exista cookie in recvdata se va cauta ID-ul dupa care se va trece intr-o
tabela de inregistrari rezultatele analizelor

Alt lucru de implementat ar mai fi sa primim un nume pentru pacient
'''
def pamantfunction(recvdata):
    global intrari
    neural_net=tf.keras.models.load_model('tester_versiune2.0.h5')
    intrari=1


        
    datesauintrebare(recvdata['Age'],0)   
        
    if(recvdata['Sex']=="M"):
        datedeintrare[0][1]=1
        datedeintrare[1][1]=1
    if(recvdata['Sex']=="F"):
        datedeintrare[0][1]=-1
        datedeintrare[1][1]=-1
    if(recvdata['Sex']=="?" or recvdata['Sex']==None):
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

    rezultat = neural_net.predict(datedeintrare)
    data = {}
    data['action']='response'
    rezultat=rezultat.tolist()
    data['rezultat'] = rezultat
    json_data = json.dumps(data)
    tf.keras.backend.clear_session()
    data['patinet_name']=recvdata['patinet_name']
	
    id_medic = id_cookie(recvdata['cookie'])
    if id_medic != -1:
        inset_medical_tests(id_medic, recvdata)
        print('ceva')	
    print(id_medic)
    print(rezultat)
    return json_data
        
    '''
    aici pamant isi va face de cap
    ps: vezi sa dai return la final de cod cu raspunsul in format json
    '''

	
	
'''
functia unde vom verifica datele trimise si trimitem un cod de eroare
0 - totul ok
1 - username existent
2 - email existent

Inserare cu parola criptata. 

return in format json cu actiunea de regresponse( register repsonse )
'''

'''
Modificat pentru a putea folosi functia in 2 situatii:
- pentru a verifica daca utilizatorul este confirmat
- pentru a confirma
'''
def confirmation(recvdata):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    username=([recvdata['username']])
    try:
        secret_code=recvdata['secret_code']
    except:
        print()
    cur.execute("SELECT * FROM baza WHERE username = ?", username)
    utilizator=cur.fetchall()
    if(utilizator[0][5]==1):
        return 2
    if(utilizator[0][7]!=secret_code):
        return 1
    if(utilizator[0][7]==secret_code):
        cur.execute("UPDATE baza SET confirmation = 1 WHERE username = ?",username)
        conn.commit()
        return 0
		
	
def registerfunction(recvdata):
    data = {}
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data['action'] = 'regresponse'
	    
    if(gasitinbaza('username', recvdata['username'],cur)):
        data['errorcode'] = 1
        data['errormessage'] = "Nume de utilizator existent!"
	
    elif(gasitinbaza('email', recvdata['email'],cur)):
        data['errorcode'] = 2
        data['errormessage'] = "Email-ul a fost deja folosit!"
    else:
        data['errorcode'] = 0
        data['errormessage'] = "Inregistrare cu succes!"
        now=datetime.now()
        timestamp=datetime.timestamp(now)
        cookies= secrets.token_hex(16)
        secret_code = secrets.token_hex(5)
        pusinbaza([(None,recvdata['username'],recvdata['password'],timestamp,cookies,0,recvdata['email'], secret_code)],cur,conn)
        msg="Codul de confirmare este = "+secret_code
        server.sendmail("infoeducatietiroida@gmail.com",recvdata['email'],msg)
	
    json_data = json.dumps(data)
	
    return json_data

	
	
	
def confirm_code(recvdata):
    data = {}
    data['action'] = "code_verify_response"

    errorcode = confirmation(recvdata)
    data['errorcode'] = errorcode
	
    if(errorcode == 0):
        data['errormessage'] = "Contul dumneavostra a fost activat!"
        
    if(errorcode == 1):
        data['errormessage'] = "Codul de confirmare nu coincide! Va rugam verificati codul!"
    if(errorcode == 2):
        data['errormessage'] = "Contul a fost deja verificat!"
	
    json_data = json.dumps(data)
	
    return json_data

def handler(c, a):


    while True:
        data = c.recv(4096)
        if not data:
            break

        stringjson = data.decode('utf-8')

        loadedjson = json.loads(stringjson)
		
		
        response = "no action"
        if(loadedjson['action'] == "analize"):
            response = pamantfunction(loadedjson)
        if(loadedjson['action'] == "register"):
            response = registerfunction(loadedjson)
        if(loadedjson['action'] == "login"):
            response = login(loadedjson)
        if(loadedjson['action'] == "code_verify"):
            response = confirm_code(loadedjson)
		
        c.send(response.encode())

    
    print("Lost connection: " + str(a))

print("am online")
while True:
    c, a = socklistener.accept()
    allconnection.append(c)

    newthread = threading.Thread(target=handler, args=(c, a))
    newthread.daemon = True
    newthread.start()

    print("New connection available: " + str(a))