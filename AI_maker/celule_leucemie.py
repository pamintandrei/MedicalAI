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
    path1="D:\\tiroida\\celule\\leucemie_train"
    date1 = keras_data.flow_from_directory(path1, target_size = (450, 450),batch_size=32, classes = ["normal","leucemie"], class_mode = "binary")
    path2="D:\\tiroida\\celule\\leucemie_test"
    date2 = keras_data.flow_from_directory(path2, target_size = (450, 450),batch_size=10, classes = ["normal","leucemie"], class_mode = "binary")
    tfmodel=keras.models.Sequential()
    tfmodel.add(Conv2D(filters=4,kernel_size=(3,3), padding='same',activation="relu",input_shape=(450,450,3)))    
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=8, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=8, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=16, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=16, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(BatchNormalization())
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Flatten())
    tfmodel.add(Dense(16, activation="relu"))
    tfmodel.add(Dense(1, activation="sigmoid"))
    tfmodel.compile(optimizer='Adam',loss="binary_crossentropy", metrics=["accuracy"])
    checkpoint = keras.callbacks.ModelCheckpoint(filepath='leucemie.h5', save_best_only=True,monitor='val_acc')
    tfmodel.fit_generator(date1,validation_data=date2,epochs=10,steps_per_epoch=100,validation_steps=1,callbacks=[checkpoint])
    
    model=keras.models.load_model('leucemie.h5')
    print(model.evaluate_generator(date2,steps=1))
        

    
input()