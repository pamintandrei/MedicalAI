import numpy as np
import tensorflow as tf
from tensorflow.keras.callbacks import TensorBoard
lista = np.array([[]])
lista.resize((4341,26))
rezultate = np.array([])
rezultate.resize((4341))
Date=open("allhyper.data","r")
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
def repetare(lista,linie,rezultate):
    for i in range(0,20):
            linie=linie+1
            lista[linie]=lista[linie-1]
            rezultate[linie]=1


def crearebazadate():

    linie=-1
    for fiecarelinie in Date:
        linie=linie+1
        dparcurs=0
        fiecarelinie = fiecarelinie.split(',')    

        

        
        

        datesauintrebare(fiecarelinie[0],linie,dparcurs)   
        dparcurs=dparcurs+1
        
        if(fiecarelinie[1]=="M"):
            lista[linie][1]=1
        if(fiecarelinie[1]=="F"):
            lista[linie][1]=-1
        if(fiecarelinie[1]=="?"):
            lista[linie][1]=0
        dparcurs=dparcurs+1
    

        for i in range(2,16):
            truf(fiecarelinie[i],linie,dparcurs)  
            dparcurs=dparcurs+1

        ultlin=16
        for i in range(1,7):
            if(i!=4):
                truf(fiecarelinie[ultlin],linie,dparcurs)
                dparcurs=dparcurs+1
                ultlin=ultlin+1
                datesauintrebare(fiecarelinie[ultlin],linie,dparcurs)
                dparcurs=dparcurs+1
                ultlin=ultlin+1
            else:
                ultlin=ultlin+2
  

        

        if(fiecarelinie[29]!="negative."):
            rezultate[linie]=1
            repetare(lista,linie,rezultate)
            linie=linie+20

        else:
            rezultate[linie]=0




crearebazadate()
tfmodel = tf.keras.models.Sequential()
tfmodel.add(tf.keras.layers.Dense(26,input_dim=26))
tfmodel.add(tf.keras.layers.Dense(16)) 
tfmodel.add(tf.keras.layers.Activation('hard_sigmoid'))

tfmodel.add(tf.keras.layers.Dense(2,activation='softmax'))


tfmodel.compile(optimizer='Nadam',loss='sparse_categorical_crossentropy',metrics=['accuracy'])
tfmodel.fit(lista,rezultate,epochs=7,validation_split=0.01)

val_loss, val_acc = tfmodel.evaluate(lista,rezultate)
print(val_loss)
print(val_acc)
precitions = tfmodel.predict(lista)
print(precitions)
tfmodel.save('hyper.h5')

model=tf.keras.models.load_model('hyper.h5')
val_loss, val_acc = model.evaluate(lista,rezultate)
print(val_loss)
print(val_acc)
input()

