import numpy as np
from tensorflow.keras.callbacks import TensorBoard
import cv2
import sys
import threading
import keras
from keras.layers import Conv2D,Dense,MaxPooling2D,Flatten,BatchNormalization,Dropout
from IPython.display import display 
from PIL import Image
import tensorflow as tf
np.random.seed(1)
with tf.device('/gpu:0'):
    keras_data1=keras.preprocessing.image.ImageDataGenerator(rescale=1./255)
    keras_data2=keras.preprocessing.image.ImageDataGenerator(rescale=1./255,horizontal_flip=True)

    path1="D:\\tiroida\\chest_xray\\train"
    path2="D:\\tiroida\\chest_xray\\test"
    path3="D:\\tiroida\\chest_xray\\deverificat"
    date1 = keras_data1.flow_from_directory(path1, target_size = (512, 512),color_mode='grayscale', classes = ["NORMAL","PNEUMONIA"], class_mode = "binary")
    date1_b = keras_data2.flow_from_directory(path1, target_size = (512, 512),color_mode='grayscale', classes = ["NORMAL","PNEUMONIA"], class_mode = "binary")
    date2= keras_data1.flow_from_directory(path2, target_size = (512, 512),color_mode='grayscale', classes = ["NORMAL","PNEUMONIA"], class_mode = "binary")
    verificat=keras_data1.flow_from_directory(path3, target_size = (512, 512),color_mode='grayscale',batch_size=1)
    tfmodel=keras.models.Sequential()
    tfmodel.add(Conv2D(filters=4, kernel_size=(3,3), activation="relu",padding='same',input_shape=(512,512,1)))
    
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=8, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Flatten())
    tfmodel.add(Dense(4, activation="relu"))
    tfmodel.add(Dense(1, activation="sigmoid"))
    tfmodel.compile(optimizer='Adam',loss="binary_crossentropy", metrics=["accuracy"])
    checkpoint = keras.callbacks.ModelCheckpoint(filepath='best_pneumonie.h5', save_best_only=True)
    tfmodel.fit_generator(date1,validation_data=date2,epochs=1,steps_per_epoch=68,validation_steps=30,callbacks=[checkpoint])

        
    tfmodel.save('pneumonie_2.0.h5')
    predict=tfmodel.predict_generator(verificat,steps=1)
    
    print(predict)
input()