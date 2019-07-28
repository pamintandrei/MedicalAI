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
import time
from shutil import copyfile
from numba import cuda
base_dir = os.path.dirname(os.path.realpath(__file__))
config_file = open(base_dir + '/server_config.json')
AI_core_file = base_dir + '\\AI_core'

config = json.load(config_file)
global graph

coada=0
graph=tf.get_default_graph()


datedeintrare=np.array([[]])
datedeintrare.resize((2,23))

datedeintrare2 = np.array([[]])
datedeintrare2.resize((2,26))



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




def connectGmail():
    global server
    server = smtplib.SMTP('smtp.gmail.com', 587)
    server.connect('smtp.gmail.com', 587)
    server.ehlo()
    server.starttls()
    server.ehlo()
    usrn=config['username_gmail']
    pas=config['password_gmail']
    server.login(str(usrn),str(pas))


def sendEmail(title, email, message):
    try:
        server.sendmail(title,email,message)
    except:
        connectGmail()
        sendEmail(title,email,message)





def pusinbaza(utilizator,cur,conn):

    cur.executemany("INSERT INTO baza VALUES(?,?,?,?,?,?,?,?,?,?)",utilizator)
    conn.commit()
    
def gasitinbaza(column,fromuser,cur):
    t = (fromuser, )
    cur.execute("SELECT * FROM baza WHERE "+column+"=?", t)
    columns = cur.fetchall()
    if(columns):
        return 1
    else:
        return 0
        
def found_in_photo_results(photo,disease,cur):
    t = (photo, disease, )
    cur.execute("SELECT * FROM photo_results WHERE photo = ? AND disease = ? LIMIT 1",t)
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
            
            
            
def datesauintrebare2(fiecarelinie,dparcurs):
        if(fiecarelinie!="?" and fiecarelinie!=None):
            datedeintrare2[0][dparcurs]=fiecarelinie
            datedeintrare2[1][dparcurs]=fiecarelinie
        else:
            datedeintrare2[0][dparcurs]=0
            datedeintrare2[1][dparcurs]=0
            
def truf(fiecarelinie,dparcurs):
        if(fiecarelinie=="f" or fiecarelinie==None):
            datedeintrare[0][dparcurs]=-1
            datedeintrare[1][dparcurs]=-1
        else:
            datedeintrare[0][dparcurs]=1
            datedeintrare[1][dparcurs]=1

def truf2(fiecarelinie,dparcurs):
        if(fiecarelinie=="f" or fiecarelinie==None):
            datedeintrare2[0][dparcurs]=-1
            datedeintrare2[1][dparcurs]=-1
        else:
            datedeintrare2[0][dparcurs]=1
            datedeintrare2[1][dparcurs]=1


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
    cur.execute("SELECT * FROM baza WHERE cookie=?",data)
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
            
                if(parolabaza[0][9] == 1 and parolabaza[0][8] == 0):
                    data['error'] = '3'
                    data['errormessage'] = 'Nu aveti inca aprobarea administratorului!'
                else:
                    data['error'] = '0'
                    data['cookie'] = parolabaza[0][4]
                    data['errormessage'] = "Logare cu succes!"
                    if(recvdata['username'] == 'admin'):
                        data['is_admin'] = True
                        data['is_medic'] = False
                        data['is_patient'] = False
                    elif(parolabaza[0][9] == 1):
                        data['is_medic'] = True
                        data['is_patient'] = False
                        data['is_admin'] = False
                    else:
                        data['is_patient'] = True
                        data['is_medic'] = False
                        data['is_admin'] = False
                
            elif(config['register_verification']):
                data['error'] = '2'
                data['errormessage'] = "Cont neverificat!"
            else:
                if(parolabaza[0][9] == 1 and parolabaza[0][8] == 0):
                    data['error'] = '3'
                    data['errormessage'] = 'Nu aveti inca aprobarea administratorului!'
                else:
                    data['error'] = '0'
                    data['cookie'] = parolabaza[0][4]
                    data['errormessage'] = "Logare cu succes!"
                    if(recvdata['username'] == 'admin'):
                        data['is_admin'] = True
                        data['is_medic'] = False
                        data['is_patient'] = False
                    elif(parolabaza[0][9] == 1):
                        data['is_medic'] = True
                        data['is_patient'] = False
                        data['is_admin'] = False
                    else:
                        data['is_patient'] = True
                        data['is_medic'] = False
                        data['is_admin'] = False           
        else:
            data['error'] = '1'
            data['errormessage'] = "Nume de utilizator sau parola incorecta!"
    else:
        data['error'] = '1'
        data['errormessage'] = "Nume de utilizator sau parola incorecta!"
		
    data['username'] = recvdata['username']
    json_data = json.dumps(data)
    print(json_data)
    
    return json_data

	
'''
Functia de jos trebuie editata pentru utilizatorii logati
In caz de exista cookie in recvdata se va cauta ID-ul dupa care se va trece intr-o
tabela de inregistrari rezultatele analizelor

Alt lucru de implementat ar mai fi sa primim un nume pentru pacient
'''
def getHyper(recvdata):
    neural_net=tf.keras.models.load_model(AI_core_file + '\\hyper.h5')


    print(1)
        
    datesauintrebare2(recvdata['Age'],0)   
        
    if(recvdata['Sex']=="M"):
        datedeintrare2[0][1]=1
        datedeintrare2[1][1]=1
    if(recvdata['Sex']=="F"):
        datedeintrare2[0][1]=-1
        datedeintrare2[1][1]=-1
    if(recvdata['Sex']=="?" or recvdata['Sex']==None):
        datedeintrare2[0][1]=0
        datedeintrare2[1][1]=0


    

    truf2(recvdata['on_thyroxine'],2)  
    truf2(recvdata['query_on_thyroxine'],3)  
    truf2(recvdata['on_antithyroid_medication'],4)  
    truf2(recvdata['sick'],5)  
    truf2(recvdata['pregnant'],6)  
    truf2(recvdata['thyroid_surgery'],7)
    truf2(recvdata['Il3l_treatment'],8)
    truf2(recvdata['query_hypothyroid'],9)  
    truf2(recvdata['query_hyperthyroid'],10)  
    truf2(recvdata['lithium'],11)    
    truf2(recvdata['goitre'],12)  
    truf2(recvdata['tumor'],13)  
    truf2(recvdata['hypopituitary'],14)
    truf2(recvdata['psych'],15)
    truf2(recvdata['TSH_measured'],16)  
    datesauintrebare2(recvdata['TSH'],17)
    truf2(recvdata['T3_measured'],18)  
    datesauintrebare2(recvdata['T3'],19)
    truf2(recvdata['TT4_measured'],20)  
    datesauintrebare2(recvdata['TT4'],21)
    truf2(recvdata['FTI_measured'],22)  
    datesauintrebare2(recvdata['FTI'],23)
    truf2(recvdata['TBG_measured'],24)  
    datesauintrebare2(recvdata['TBG'],25)

    rezultat = neural_net.predict(datedeintrare2)
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


def get_medic_id(username):
    t = (username, )
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    cur.execute("SELECT * FROM baza WHERE username = ? AND medic = 1 AND admin_confirmation = 1 AND username != \"admin\"", t)
    medic = cur.fetchall()
    if(medic):
        return medic[0][0]
    else:
        return -1
    
def get_patient_id(username):
    t = (username, )
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    cur.execute("SELECT * FROM baza WHERE username = ? AND medic = 0 AND username != \"admin\"", t)
    patient = cur.fetchall()
    if(patient):
        return patient[0][0]
    else:
        return -1

def appointment(recvdata):
    data = {}
    medic_username = recvdata['medic_username']
    id_medic = get_medic_id(medic_username)
    patient_id = id_cookie(recvdata['cookie'])
    
    data['action'] = 'appointment_response'
    
    if(id_medic >= 1 and patient_id >= 1):
        error = programare(id_medic, patient_id, recvdata['timestamp'])
        
        if(error == -1):
            data['errcode'] = -2
            data['errmessage'] = 'Aveti deja o programare pentru aceasta ora'
        elif(error == -2):
            data['errcode'] = -3
            data['errmessage'] = 'Un alt pacient a fost programat'
        else:
            data['errcode'] = 0
            data['errmessage'] = 'Programarea a fost facuta'
    else:
        data['errcode'] = -1
        data['errmessage'] = 'Wrong cookie'
        
    json_data = json.dumps(data)
    return json_data
    
    

def programare(id_medic, patient_id, timestamp):
    t = (id_medic, )
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    
    cur.execute("SELECT * FROM programari WHERE medic_id = ?",t)
    programari=cur.fetchall()
    
    for i in programari:
    
        if(i[3]==timestamp and i[2] == patient_id):
            return -2
        if(i[3]==timestamp):
            return -1
        ''' -1 inseamna ca doctorul are deja programare atunci'''

        '''-2 inseamna ca pacientul deja are pusa o programare atunci'''
    date=(None,id_medic,patient_id,timestamp,0, "",0,)
    cur.execute("INSERT INTO programari VALUES(?,?,?,?,?,?,?)",date)
    conn.commit()
    return 0
    


def get_email_by_username(username):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    t = (username, )
    cur.execute("SELECT * FROM baza WHERE username = ? LIMIT 1",t)
    info = cur.fetchall()
    if(info):
        return info[0][6]
    else:
        return -1
        
'''

-1 - Confirmat deja
0 - Confirmat
-2 - Respins cu mesaj trimis

'''

def appointment_confirm(recvdata):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'appointment_confirm_response'
    if gasitinbaza('cookie', recvdata['cookie'], cur) and checkMedicCookie(cur, recvdata['cookie']):
        medic_id = id_cookie(recvdata['cookie'])
        patient_id = get_patient_id(recvdata['username'])
        time = recvdata['time']
        confirmed = recvdata['confirmed']
        message = recvdata['message']
        error = 0
        email = get_email_by_username(recvdata['username'])
        if(confirmed == -1):
            error = confirm_programare(medic_id, patient_id, time, confirmed, message, email)
        else:
            error = confirm_programare(medic_id, patient_id, time, confirmed, email = email)
        if(error == -1):
            data['errcode'] = -1
            data['errmessage'] = 'Programare deja facuta'
        elif(error == -2 or error == 0):
            data['errcode'] = 0
            data['errmessage'] = 'Operatiune reusita'
            
    json_response = json.dumps(data)
    return json_response
        

def confirm_programare(medic_id, patient_id, time, confirmed, message = None, email = None):
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    t = (medic_id,time,patient_id,1, )
    cur.execute("SELECT * FROM programari WHERE medic_id = ? AND Date_time = ? AND patient_id = ? AND confirmed = ?",t)
    programari=cur.fetchall()
    if(programari):
        return -1
    if(confirmed==1):
        converted_time = datetime.utcfromtimestamp(time).strftime("%Y-%m-%d %H:%M")
        programare=(medic_id, time,)
        cur.execute("UPDATE programari SET confirmed = 1 WHERE medic_id = ? AND Date_time = ?",programare)
        conn.commit()   
        msg="Programare din data " +converted_time+" a fost confirmata."
        
        return 0
    else:
        programare=(message, medic_id, time, )
        cur.execute("UPDATE programari SET confirmed = -1, message = ? WHERE medic_id = ? AND Date_time = ?",programare)
        conn.commit() 
        sendEmail("infoeducatietiroida@gmail",email,message)
        
        return -2
        
def takemadic(recvdata):
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    username=([recvdata['username']])
    cur.execute("UPDATE baza SET admin_confirmation = 0 WHERE username = ?",username)
    conn.commit()
    return 0

def makemedic(recvdata):
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    username=([recvdata['username']])
    cur.execute("UPDATE baza SET admin_confirmation = 1 WHERE username = ?",username)
    conn.commit()
    return 0
    
def check_medic_confirmation(ID):
    conn = sqlite3.connect(base_dir + '/bazadedate.db')
    cur = conn.cursor()
    id_row = ([ID])
    cur.execute("SELECT * FROM baza WHERE ID = ?", id_row)
    
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
    elif(recvdata['medic'] == True and config['medic_registration'] == False):
        data['errorcode'] = 3
        data['errormessage'] = "Cererile pentru medici sunt inchise!"
    else:
        data['errorcode'] = 0
        data['errormessage'] = "Inregistrare cu succes!"
        now=datetime.now()
        timestamp=datetime.timestamp(now)
        cookies= secrets.token_hex(16)
        secret_code = secrets.token_hex(5)
        pusinbaza([(None,recvdata['username'],recvdata['password'],timestamp,cookies,0,recvdata['email'], secret_code,0,recvdata['medic'])],cur,conn)
        msg="Codul de confirmare este = "+secret_code
        sendEmail("infoeducatietiroida@gmail", recvdata['email'], msg)
        
	
    json_data = json.dumps(data)
	
    
    print(json_data)
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
    
    print(jsons)
    return jsons



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

    if(os.path.exists(return_folder)):
        return return_folder + '\\',file_name.hexdigest(), -1
    else:
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
        return return_folder + '\\', file_name.hexdigest(), 0
        
        

'''

returneaza la sfarsit rezultatul in format json


'''

def save_result_database(photo_name, result, disease, medic_id = -1):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    inser_data = ([None,medic_id,result,photo_name,disease])
    cur.execute("INSERT INTO photo_results VALUES(?,?,?,?,?)", inser_data)
    conn.commit()


def get_photo_reuslt(photo_name, disease):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    t = (photo_name, disease, )
    if(found_in_photo_results(photo_name,disease,cur)):
        cur.execute("SELECT * FROM photo_results WHERE photo=? AND disease=? LIMIT 1",t)
        info = cur.fetchall()
        return info[0][2]
    else:
        return -1
    

'''
Functia trebuie modificata pentru 
a salva medicul
'''
def getImage(recvdata,boala,rezolutie,optiune=None):
    global coada
    while coada==1:
        time.sleep(5)
        pass
    coada=1
    response_data = {}
    path_to_photo, photo_name, error = save_photo_frombase64(recvdata['imageContent'])
    tf.keras.backend.clear_session()
    print(boala)
    fetch_data = get_photo_reuslt(photo_name, boala)
    if(error == -1 and fetch_data != -1):
        print("am ajuns boss")
        rezultat=np.array([[]])
        rezultat.resize((1,1))
        rezultat[0][0]=fetch_data
        response_data['rezultat']=rezultat.tolist()
        response_data['action']='photoresult'
        
        json_data=json.dumps(response_data)
        coada = 0
        return json_data
    
    response_data['action'] = 'photoresult'
    keras_data=keras.preprocessing.image.ImageDataGenerator()
    
    if(optiune=='grayscale-pneumo'):
        keras_data=keras.preprocessing.image.ImageDataGenerator(rescale=1./255)
        verificat=keras_data.flow_from_directory(path_to_photo,color_mode='grayscale',target_size = (rezolutie, rezolutie),batch_size=1)
    elif(optiune=='grayscale'):
        keras_data=keras.preprocessing.image.ImageDataGenerator()
        verificat=keras_data.flow_from_directory(path_to_photo,color_mode='grayscale',target_size = (rezolutie, rezolutie),batch_size=1)
    else:
        keras_data=keras.preprocessing.image.ImageDataGenerator()
        verificat=keras_data.flow_from_directory(path_to_photo, target_size = (rezolutie, rezolutie),batch_size=1)
    with graph.as_default():
        tfmodel=keras.models.load_model(AI_core_file + '\\' +boala+'.h5')
        predict=tfmodel.predict_generator(verificat,steps=1)
        predict=predict.tolist()
    response_data['rezultat'] = predict
    json_data = json.dumps(response_data)
    tf.keras.backend.clear_session()
    del tfmodel
    cuda.select_device(0)
    cuda.close()
    save_result_database(photo_name, predict[0][0],boala)
    coada=0
    print(json_data)
    return json_data
    
    
    
def changePassword(currentPassword, newPassword, cookie):
    response = {}
    response['action'] = 'changepasswordresult'
    
    user_id = id_cookie(cookie)
    if user_id != -1:
        conn = sqlite3.connect('bazadedate.db')
        cur = conn.cursor()
        t = (currentPassword, user_id, )
        cur.execute("SELECT * FROM baza WHERE password = ? AND ID = ?", t)
        columns = cur.fetchall()
        print(columns)
        if columns:
            newcookies= secrets.token_hex(16)
            t2 = (newPassword,newcookies,user_id, )
            cur.execute("UPDATE baza SET password = ?, cookie = ? WHERE ID = ?", t2)
            conn.commit()
            response['errcode'] = 0
            response['errmessage'] = "Parola a fost schimbata cu succes!"
            response['cookie'] = newcookies
        else:
            response['errcode'] = -1
            response['errmessage'] = "Parola curenta introdusa gresit"
        
    else:
        response['errcode'] = -2
        response['errmessage'] = "Not a valid user"
        
    stringjson = json.dumps(response)
    return stringjson
    
    
    
def getappointment(recvdata):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'getappointments_response'
    if gasitinbaza('cookie', recvdata['cookie'], cur) and checkMedicCookie(cur, recvdata['cookie']):
        medic_id = id_cookie(recvdata['cookie'])
        t = (medic_id, )
        cur.execute("SELECT baza.username AS username, programari.Date_time AS date FROM programari LEFT JOIN baza ON baza.ID=programari.patient_id WHERE programari.confirmed = 0 AND medic_id = ? ORDER BY programari.ID", t)
        
        
        result = cur.fetchall()
        formatedresult = []
        tempresult = {}
        typecount = 0
        
        for row in result:
            tempresult = {}
            for type in row:
                if(typecount == 0):
                    tempresult['username'] = type
                    typecount = 1
                else:
                    tempresult['time'] = type
                    typecount = 0
                    formatedresult.append(tempresult)
                    
        
        data['appointments'] = formatedresult
        
        json_data = json.dumps(data)
        
        
        print(json_data)
        return json_data
        
    
    
def checkAdminUserDataBase():
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    if gasitinbaza('username', 'admin', cur) == 0:
        now=datetime.now()
        timestamp=datetime.timestamp(now)
        cookies= secrets.token_hex(16)
        secret_code = secrets.token_hex(5)
        pusinbaza([(None,'admin','admin',timestamp,cookies,1,config["username_gmail"], secret_code,1,1)],cur,conn)


def checkMedicCookie(cur,cookie):
    t = (cookie, )
    cur.execute("SELECT * FROM baza WHERE cookie = ? LIMIT 1", t)
    columns = cur.fetchall()
    if(columns[0][8] == 1 and columns[0][9]):
        return True
    else:
        return False


def checkAdminCookie(cur,cookie):
    t = (cookie, )
    cur.execute("SELECT * FROM baza WHERE cookie = ? LIMIT 1", t)
    columns = cur.fetchall()
    if columns[0][1] == "admin":
        return True
    else:
        return False


def getMedics():
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'medicsresult'

    cur.execute("SELECT username FROM baza WHERE admin_confirmation = 1 AND medic = 1 AND username != \"admin\" ")
    informatii=cur.fetchall()
    date=[]

    for liste in informatii:
        for elemente in liste:
            date.append(elemente)
        
    data['errcode'] = 0
    data['medics'] = date
    print(data)

        
        
    json_data = json.dumps(data)
    return json_data
    
def getpatientappointment(recvdata):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = "patient_appointment_response"
    patient_id = id_cookie(recvdata['cookie'])
    t = (patient_id, )
    cur.execute("SELECT baza.username AS username, programari.Date_time AS date, programari.confirmed AS confirmed FROM programari LEFT JOIN baza ON baza.ID=programari.medic_id WHERE programari.patient_id = ? AND programari.deleted != 1 ORDER BY programari.ID", t)
    
    
    
    result = cur.fetchall()
    formatedresult = []
    tempresult = {}
    typecount = 0
        
    for row in result:
        tempresult = {}
        for type in row:
            if(typecount == 0):
                tempresult['username'] = type
                typecount = 1
            elif(typecount == 1):
                tempresult['time'] = type
                typecount = 2
            else:
                tempresult['confirmed'] = type
                typecount = 0
                formatedresult.append(tempresult)
                
                    
        
    data['appointments'] = formatedresult
    
    
    json_data = json.dumps(data)
    print(json_data)
    
    return json_data
    
    
    
    
def getNonMedics(cookie):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'nonmedicsresult'
    if checkAdminCookie(cur, cookie):
        cur.execute("SELECT username FROM baza WHERE admin_confirmation = 0 AND medic = 1")
        informatii=cur.fetchall()
        date=[]
    
        for liste in informatii:
            for elemente in liste:
                date.append(elemente)
            
        data['errcode'] = 0
        data['medics'] = date
        print(data)
    else:
        data['errcode'] = 1
        
        
    json_data = json.dumps(data)
    return json_data
    
    
def getConfig(cookie):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'configresult'
    if checkAdminCookie(cur, cookie):
        data['register_verification'] = config['register_verification']
        data['medic_registration'] = config['medic_registration']
        data['errcode'] = 0
    else:
        data['errcode'] = 1
        
    json_data = json.dumps(data)
    return json_data
    
    
def setConfig(cookie, register_verification, medic_registration):
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    data = {}
    data['action'] = 'setconfigresult'
    if checkAdminCookie(cur, cookie):
        config['register_verification'] = register_verification
        config['medic_registration'] = medic_registration
        config_data = json.dumps(config)
        f = open("server_config.json", "w")
        f.write(config_data)
        f.close()
        data['errcode'] = 0
    else:
        data['errcode'] = 1
        
    json_data = json.dumps(data)
    return json_data
    
    
def addmedicrequest(recvdata):
    data = {}
    data['action'] = 'addmedicresponse'
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    if checkAdminCookie(cur, recvdata['cookie']):
        data['errcode'] = 0
        data['errmessage'] = "Medicul a fost adaugat cu succes!"
        makemedic(recvdata)
    else:
        data['errcode'] = 1
        data['errmessage'] = "Invalid cookie"
          
    json_data = json.dumps(data)
    return json_data
        
        
def removemedicrequest(recvdata):
    data = {}
    data['action'] = 'removemedicresponse'
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    if checkAdminCookie(cur, recvdata['cookie']):
        data['errcode'] = 0
        data['errmessage'] = "Medicul a fost sters cu succes"
        takemadic(recvdata)
    else:
        data['errcode'] = 1
        data['errmessage'] = "Invalid cookie"
        
    json_data = json.dumps(data)
    return json_data
    
    
def appointment_response(recvdata):
    data = {}
    data['action'] = 'appointment_patient_response'
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    
    patient_id = id_cookie(recvdata['cookie'])
    if(patient_id != -1):
        medic_id = get_medic_id(recvdata['medic_username'])
        t = (medic_id, patient_id, recvdata['date'],)
        cur.execute("SELECT message FROM programari WHERE medic_id = ? AND patient_id = ? AND Date_time = ? AND deleted != 1", t)
        info = cur.fetchall()
        data['response'] = info[0][0]
        
    json_data = json.dumps(data)
    print(json_data)
    return json_data
    
    
    
def delete_appointment(recvdata):
    data = {}
    data['action'] = 'appointment_delete'
    conn = sqlite3.connect('bazadedate.db')
    cur = conn.cursor()
    
    patient_id = id_cookie(recvdata['cookie'])
    if(patient_id != -1):
        medic_id = get_medic_id(recvdata['medic_username'])
        t=(medic_id,patient_id,recvdata['date'],)
        cur.execute("UPDATE programari SET deleted = 1 WHERE medic_id = ? AND patient_id = ? AND Date_time = ? AND deleted = 0", t)
        conn.commit()
        data['errcode'] = 0
        data['errmessage'] = 'Mesajul a fost sters'
        
    json_data = json.dumps(data)
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
		
        print(loadedjson['action'])
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
            response = getImage(loadedjson,'pneumonia',512,'grayscale-pneumo')
        if(loadedjson['action']=="tuberculoza"):
            response = getImage(loadedjson,'tuberculoza',512,'grayscale')
        if(loadedjson['action']=="hemoragie"):
            response = getImage(loadedjson,'hemoragie',200,'grayscale')
        if(loadedjson['action']=="hyper"):
            response = getHyper(loadedjson)
        if(loadedjson['action']=="cancersan"):
            response = getImage(loadedjson,'cancersan',50)
        if(loadedjson['action']=="leucemie"):
            response = getImage(loadedjson,'leucemie',450)
        if(loadedjson['action']=="malarie"):
            response = getImage(loadedjson,'malarie',50)
        if(loadedjson['action']=="cancerpiele"):
            response = getImage(loadedjson,'cancerpiele',64)
        if(loadedjson['action']=="parkinson"):
            response = getImage(loadedjson,'parkinson',224)
        if(loadedjson['action'] == "changepassword"):
            response = changePassword(loadedjson["currentpassword"], loadedjson["newpassword"], loadedjson["cookie"])
        if(loadedjson['action'] == "getconfig"):
            response = getConfig(loadedjson['cookie'])
        if(loadedjson['action'] == "setconfig"):
            response = setConfig(loadedjson['cookie'], loadedjson['register_verification'], loadedjson['medic_registration'])
        if(loadedjson['action'] == "getmedics"):
            response = getMedics()
        if(loadedjson['action'] == "getnonmedics"):
            response = getNonMedics(loadedjson['cookie'])
        if(loadedjson['action'] == "addmedic"):
            response = addmedicrequest(loadedjson)
        if(loadedjson['action'] == "removemedic"):
            response = removemedicrequest(loadedjson)
        if(loadedjson['action'] == 'appointment'):
            response = appointment(loadedjson)
        if(loadedjson['action'] == 'getappointment'):
            response = getappointment(loadedjson)
        if(loadedjson['action'] == 'appointment_confirm'):
            response = appointment_confirm(loadedjson)
        if(loadedjson['action'] == 'getpatientappointment'):
            response = getpatientappointment(loadedjson)
        if(loadedjson['action'] == 'appointment_response'):
            response = appointment_response(loadedjson)
        if(loadedjson['action'] == 'delete_appointment'):
            response = delete_appointment(loadedjson)
         
        response += "<EOF>"
	
	
        c.send(response.encode())

    
    print("Lost connection: " + str(a))


	
def recvall(sock):
	data = b''
	while True:
		data += sock.recv(102400)
		if data.decode('UTF-8')[-5:] == "<EOF>":
			return data
	


checkAdminUserDataBase()
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
		
