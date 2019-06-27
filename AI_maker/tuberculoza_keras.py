import numpy as np
from tensorflow.keras.callbacks import TensorBoard
import cv2
import sys
import threading
import keras
from keras.layers import Conv2D,Dense,MaxPooling2D,Flatten,BatchNormalization
from IPython.display import display 
from PIL import Image
import tensorflow as tf
np.random.seed(1)
with tf.device('/gpu:0'):
    keras_data=keras.preprocessing.image.ImageDataGenerator()
    path1="D:\\tiroida\\tuberculoza\\train"
    date1 = keras_data.flow_from_directory(path1, target_size = (512, 512),color_mode='grayscale', classes = ["normal","tuberculoza"], class_mode = "binary")
    path2="D:\\tiroida\\tuberculoza\\test"
    date2 = keras_data.flow_from_directory(path2, target_size = (512, 512),color_mode='grayscale', classes = ["normal","tuberculoza"], class_mode = "binary")
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
    tfmodel.fit_generator(date1,validation_data=date2,epochs=20,steps_per_epoch=20,validation_steps=1)

        
    tfmodel.save('tuberculoza.h5')
    
input()