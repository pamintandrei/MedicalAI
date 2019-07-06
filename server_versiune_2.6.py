import socket
import threading
import json
import keras
import tensorflow as tf
import numpy as np
import sqlite3
from datetime import datetime
import secrets
import smtplib
import ssl
import os,sys
import base64
import hashlib
import imghdr
from shutil import copyfile

base_dir = os.path.dirname(os.path.realpath(__file__))
config_file = open(base_dir + '/server_config.json')
AI_core_file = base_dir + '/AI_core'

config = json.load(config_file)



datedeintrare=np.array([[]])
datedeintrare.resize((2,23))





context = ssl.SSLContext(ssl.PROTOCOL_TLS_SERVER)
context.load_cert_chain(base_dir + '/medicalai.cert', base_dir + '/medicalai.key')
server = smtplib.SMTP('smtp.gmail.com', 587)
server.connect('smtp.gmail.com', 587)
server.ehlo()
server.starttls()
server.ehlo()
usrn=config['username_gmail']
pas=config['password_gmail']
server.login(str(usrn),str(pas))


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
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    data=([cookie])
    cur.execute("SELECT * FROM baza where cookie=?",data)
    toateinformatiile= cur.fetchall()
    try:
        return toateinformatiile[0][0]
    except:
        return -1
		

		
def inset_medical_tests(medic_id, data, rezultat):
	conn = sqlite3.connect('bazadedate.db')
	cur = conn.cursor()
	now=datetime.now()
	timestamp=datetime.timestamp(now)
	inser_data = ([None,data['patient_name'], data['Sex'], data['Age'], data['on_thyroxine'], data['query_on_thyroxine'], data['on_antithyroid_medication'], data['thyroid_surgery'], data['query_hypothyroid'], data['query_hyperthyroid'], data['pregnant'], data['sick'], data['tumor'], data['lithium'], data['goitre'], data['TSH_measured'], data['TSH'], data['T3_measured'], data['T3'], data['TT4_measured'], data['TT4'], data['FTI_measured'], data['FTI'], data['TBG_measured'], data['TBG'], medic_id, timestamp, rezultat[0][0],rezultat[1][1] ])
	cur.execute("INSERT INTO analyzes_patient VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", inser_data)
	conn.commit()
	
	
'''
print(id_cookie(''))
'''
def login(recvdata):
    data = {}
    data['action'] = "loginresponse"
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
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
def getHyper(recvdata):
    neural_net=tf.keras.models.load_model('hyper.h5')


    print(1)
        
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
    truf(recvdata['sick'],5)  
    truf(recvdata['pregnant'],6)  
    truf(recvdata['thyroid_surgery'],7)
    truf(recvdata['Il3l_treatment'],8)
    truf(recvdata['query_hypothyroid'],9)  
    truf(recvdata['query_hyperthyroid'],10)  
    truf(recvdata['lithium'],11)    
    truf(recvdata['goitre'],12)  
    truf(recvdata['tumor'],13)  
    truf(recvdata['hypopituitary'],14)
    truf(recvdata['psych'],15)
    truf(recvdata['TSH_measured'],16)  
    datesauintrebare(recvdata['TSH'],17)
    truf(recvdata['T3_measured'],18)  
    datesauintrebare(recvdata['T3'],19)
    truf(recvdata['TT4_measured'],20)  
    datesauintrebare(recvdata['TT4'],21)
    truf(recvdata['FTI_measured'],22)  
    datesauintrebare(recvdata['FTI'],23)
    truf(recvdata['TBG_measured'],24)  
    datesauintrebare(recvdata['TBG'],25)

    rezultat = neural_net.predict(datedeintrare)
    data = {}
    data['action']='response'
    rezultat=rezultat.tolist()
    data['rezultat'] = rezultat
    json_data = json.dumps(data)
    tf.keras.backend.clear_session()
    data['patient_name']=recvdata['patient_name']
	
    id_medic = id_cookie(recvdata['cookie'])
    if id_medic != -1:
        inset_medical_tests(id_medic, recvdata, rezultat)
        print('ceva')	
    print(id_medic)
    print(rezultat)
    print(2)
    return json_data


def pamantfunction(recvdata):

    neural_net=tf.keras.models.load_model(AI_core_file + '\hypothyroid.h5')


    print(1)
        
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
    data['patient_name']=recvdata['patient_name']
	
    id_medic = id_cookie(recvdata['cookie'])
    if id_medic != -1:
        inset_medical_tests(id_medic, recvdata, rezultat)
        print('ceva')	
    print(id_medic)
    print(rezultat)
    print(2)
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
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
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


def doctoranalize(medic_id):

    medic_id=([medic_id])
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    conn.row_factory=sqlite3.Row
    cur=conn.cursor()
    cur.execute("SELECT * FROM analyzes_patient WHERE medic_id=?",medic_id)
    data = {}
    data['results'] = [dict(row) for row in cur.fetchall()]
    data['action'] = 'results'
    data['error'] = 0
    jsons=json.dumps(data)
    return jsons
print(doctoranalize(1))


def getanalyze(recvdata):
	
	medic_id = id_cookie(recvdata['cookie'])
	if medic_id != -1:
		results = doctoranalize(medic_id)
	else:
		results['action'] = 'results'
		results['error'] = 1
		results['errormsg'] = 'Nu sunteti logat'
		results = json.dumps(results)
		
	return results
		
		
		
	
	

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

'''

save file from base64 and return the path
return:
-1 in case of invalid photo
path to image in case of valid photo


'''
def save_photo_frombase64(base64content):
    file_name = hashlib.md5(base64content.encode())
    
    
    return_folder = base_dir + '\\savephoto\\' + file_name.hexdigest()
    print(return_folder)
    os.mkdir(return_folder)
    
    return_folder2 = return_folder + '\\' + file_name.hexdigest()
    os.mkdir(return_folder2)
    
    
    save_path = return_folder2 + '\\' + file_name.hexdigest()
    
    
    decoded = base64.b64decode(base64content)
    f = open(save_path, "wb")
    f.write(decoded)
    f.close()
    
    extension = imghdr.what(save_path)
    
    
    
    
    save_path_ext = save_path + '.' + extension
    copyfile(save_path,save_path_ext)
    os.remove(save_path)
    return return_folder + '\\'
    
    

'''

returneaza la sfarsit rezultatul in format json


'''


def getImage(recvdata,boala,rezolutie):
    response_data = {}
    path_to_photo = save_photo_frombase64(recvdata['imageContent'])
    response_data['action'] = boala+'result'
    keras_data=keras.preprocessing.image.ImageDataGenerator()
    tfmodel=keras.models.load_model(AI_core_file + '\ ' +boala+'.h5')
    verificat=keras_data.flow_from_directory(path_to_photo, target_size = (rezolutie, rezolutie),batch_size=1)
    predict=tfmodel.predict_generator(verificat,steps=1)
    predict=predict.tolist()
    response_data['rezultat'] = predict
    json_data = json.dumps(response_data)
    tf.keras.backend.clear_session()
    print(json_data)
    return json_data

def handler(c, a):
    while True:
        data = recvall(c)
        if not data:
            break
			
        stringjsonEOF = data.decode('UTF-8')
        stringjson  = stringjsonEOF[:-5]

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
        if(loadedjson['action'] == "getresult"):
            response = getanalyze(loadedjson)
        if(loadedjson['action'] == "pneumonia"):
            response = getImage(loadedjson,'pneumonia',512)
        if(loadedjson['action']=="tuberculoza"):
            response = getImage(loadedjson,'tuberculoza',512)
        if(loadedjson['action']=="hemoragie"):
            response = getImage(loadedjson,'hemoragie',200)
        if(loadedjson['action']=="hyper"):
            response = getHyper(loadedjson)
        if(loadedjson['action']=="cancersan"):
            response = getImage(loadedjson,'cancersan',50)
        if(loadedjson['action']=="leucemie"):
            response = getImage(loadedjson,'leucemie',400)
        if(loadedjson['action']=="malarie"):
            response = getImage(loadedjson,'malarie',50)
        response += "<EOF>"
	
	
        c.send(response.encode())

    
    print("Lost connection: " + str(a))


	
def recvall(sock):
	data = b''
	while True:
		data += sock.recv(51200)
		if data.decode('UTF-8')[-5:] == "<EOF>":
			return data
	



with socket.socket(socket.AF_INET, socket.SOCK_STREAM, 0) as sock:
	sock.bind(('0.0.0.0', 5554))
	sock.listen(5)
	print("===Server Online===")
	while True:
		c, a = sock.accept()
		encryptionconn = context.wrap_socket(c, server_side = True)
		newthread = threading.Thread(target=handler, args=(encryptionconn, a))
		newthread.daemon = True
		newthread.start()
		allconnection.append(encryptionconn)
		print("New connection available: " + str(a))
		
