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
    keras_data=keras.preprocessing.image.ImageDataGenerator()
    path1="D:\\tiroida\cancer_san\\train"
    date1 = keras_data.flow_from_directory(path1, target_size = (650, 650),batch_size=37, classes = ["normal","cancerigen"], class_mode = "binary")
    path2="D:\\tiroida\\cancer_san\\test"
    date2 = keras_data.flow_from_directory(path2, target_size = (650, 650),batch_size=20, classes = ["normal","cancerigen"], class_mode = "binary")
    tfmodel=keras.models.Sequential()
    tfmodel.add(Conv2D(filters=4,kernel_size=(3,3), padding='same',activation="relu",input_shape=(650,650,3)))    
     
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
    tfmodel.compile(optimizer='rmsprop',loss="binary_crossentropy", metrics=["accuracy"])
    checkpoint = keras.callbacks.ModelCheckpoint(filepath='test.h5', save_best_only=True,monitor='val_acc')
    tfmodel.fit_generator(date1,validation_data=date2,epochs=1,steps_per_epoch=30,validation_steps=4,callbacks=[checkpoint])
    model=keras.models.load_model('cancersan.h5')
    print(model.evaluate_generator(date2,steps=21))
        

    
input()