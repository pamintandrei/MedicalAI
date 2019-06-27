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
    path1="D:\\tiroida\\head_ct\\train"
    date1 = keras_data.flow_from_directory(path1, target_size = (200, 200),batch_size=10,color_mode='grayscale', classes = ["normal","hemoragie"], class_mode = "binary")
    path2="D:\\tiroida\\head_ct\\test"
    date2 = keras_data.flow_from_directory(path2, target_size = (200, 200),batch_size=10,color_mode='grayscale', classes = ["normal","hemoragie"], class_mode = "binary")
    tfmodel=keras.models.Sequential()
    tfmodel.add(Conv2D(filters=4, kernel_size=(3,3), activation="relu",padding='same',input_shape=(200,200,1)))
    
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))       
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2))) 
    tfmodel.add(Conv2D(filters=128, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=64, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2))) 
    tfmodel.add(Flatten())
    tfmodel.add(Dense(64, activation="relu"))
    tfmodel.add(Dense(1, activation="sigmoid"))
    tfmodel.compile(optimizer='Adam',loss="binary_crossentropy", metrics=["accuracy"])
    checkpoint = keras.callbacks.ModelCheckpoint(filepath='best_hemoragie.h5', save_best_only=True)
    tfmodel.fit_generator(date1,validation_data=date2,epochs=100,steps_per_epoch=18,validation_steps=2,callbacks=[checkpoint])

        

    
input()