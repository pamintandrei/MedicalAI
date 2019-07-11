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

with tf.device('/gpu:0'):
    np.random.seed(1)
    tf.set_random_seed(1)
    keras_data=keras.preprocessing.image.ImageDataGenerator()
    path1="D:\\tiroida\\spiral\\training"
    date1 = keras_data.flow_from_directory(path1, target_size = (224, 224), classes = ["healthy","parkinson"], class_mode = "binary")
    path2="D:\\tiroida\\spiral\\testing"
    date2 = keras_data.flow_from_directory(path2, target_size = (224, 224), classes = ["healthy","parkinson"], class_mode = "binary")
    tfmodel=keras.models.Sequential()
    tfmodel.add(Conv2D(filters=8,kernel_size=(3,3), padding='same',activation="relu",input_shape=(224, 224,3)))    
    tfmodel.add(keras.layers.SpatialDropout2D(0.5))
    tfmodel.add(MaxPooling2D(pool_size=(2,2)))
    tfmodel.add(Conv2D(filters=32, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=32, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(Conv2D(filters=32, kernel_size=(3,3), activation="relu",padding='same'))
    tfmodel.add(keras.layers.SpatialDropout2D(0.5))
    tfmodel.add(BatchNormalization())

   

    tfmodel.add(Flatten())
    tfmodel.add(Dense(32, activation="relu"))
    tfmodel.add(Dropout(0.5))
    tfmodel.add(Dense(1, activation="sigmoid"))
    optimizer=keras.optimizers.RMSprop(lr=0.00001)
    tfmodel.compile(optimizer=optimizer,loss="binary_crossentropy", metrics=["accuracy"])

    checkpoint = keras.callbacks.ModelCheckpoint(filepath='parkinson_spiral.h5', save_best_only=True,monitor='val_acc',mode='max',verbose=1)
    tfmodel.fit_generator(date1,validation_data=date2,epochs=500,steps_per_epoch=2,validation_steps=1,callbacks=[checkpoint])
    model=keras.models.load_model('parkinson_spiral.h5')
    print(model.evaluate_generator(date2,steps=1))
        

    
input()