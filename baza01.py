import numpy as np
import tensorflow as tf

lista = np.array([[]])
lista.resize((3164,23))
rezultate = np.array([])
rezultate.resize((3164))
Date=open("hypothyroid.data","r")
dparcurs=0



def datesauintrebare(fiecarelinie,linie,dparcurs):
        if(fiecarelinie!="?"):
            lista[linie][dparcurs]=fiecarelinie
        else:
            lista[linie][dparcurs]=0
            


def truf(fiecarelinie,linie,dparcurs):
        if(fiecarelinie=="f"):
            lista[linie][dparcurs]=-1
        else:
            lista[linie][dparcurs]=1

            
def yon(fiecarelinie,linie,dparcurs):
           
        if(fiecarelinie=="n"):
            lista[linie][dparcurs]=-1
        if(fiecarelinie=="y"):
            lista[linie][dparcurs]=1
        if(fiecarelinie=="?"):
            lista[linie][dparcurs]=0  
            
def crearebazadate():

    linie=-1
    for fiecarelinie in Date:
        linie=linie+1
        dparcurs=0
        fiecarelinie = fiecarelinie.split(',')    
        if(fiecarelinie[0]=="hypothyroid"):
            rezultate[linie]=1
        else:
            rezultate[linie]=0
        

        
        

        datesauintrebare(fiecarelinie[1],linie,dparcurs)   
        dparcurs=dparcurs+1
        
        if(fiecarelinie[2]=="M"):
            lista[linie][1]=1
        if(fiecarelinie[2]=="F"):
            lista[linie][1]=-1
        if(fiecarelinie[2]=="?"):
            lista[linie][1]=0
        dparcurs=dparcurs+1
    

        for i in range(3,14):
            truf(fiecarelinie[i],linie,dparcurs)  
            dparcurs=dparcurs+1

        ultlin=14
        for i in range(1,6):
            if(i!=4):
                yon(fiecarelinie[ultlin],linie,dparcurs)
                dparcurs=dparcurs+1
                ultlin=ultlin+1
                datesauintrebare(fiecarelinie[ultlin],linie,dparcurs)
                dparcurs=dparcurs+1
                ultlin=ultlin+1
            else:
                ultlin=ultlin+2
  

        
        if(fiecarelinie[25]!="?\n"):
            lista[linie][22]=fiecarelinie[25]
        else:
            lista[linie][22]=0
        






crearebazadate()
tfmodel = tf.keras.models.Sequential()
tfmodel.add(tf.keras.layers.Dense(23,input_dim=23))
tfmodel.add(tf.keras.layers.Dense(512)) 
tfmodel.add(tf.keras.layers.Activation('hard_sigmoid'))
tfmodel.add(tf.keras.layers.Dense(512))
tfmodel.add(tf.keras.layers.Activation('hard_sigmoid'))
tfmodel.add(tf.keras.layers.Dense(2,activation=tf.nn.softmax))


tfmodel.compile(optimizer='Nadam',loss='sparse_categorical_crossentropy',metrics=['accuracy'])
tfmodel.fit(lista,rezultate,epochs=1)

val_loss, val_acc = tfmodel.evaluate(lista,rezultate)
print(val_loss)
print(val_acc)
precitions = tfmodel.predict(lista)
print(precitions)
tfmodel.save('tester4.h5')



