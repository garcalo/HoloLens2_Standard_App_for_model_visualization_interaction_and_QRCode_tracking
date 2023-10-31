# HoloLens2 Standard App for model visualization interaction and QRCode tracking
This repository contains an app designed to be used on Microsoft HoloLens 2. The app provides a userfriendly solution for users to visualize and interact any 3D models desired, these models can also be registered to a tracked QR code, all of this can be configured in the app's control panel.
## App workflow overview

The overall idea of the workflow is illustrated by the figure below and is the following
1. First the user is required to upload the models and materials to a Google Drive folder that is linked to the app
2. In the same Google Drive folder, the user is requiered to add and complete a Configuration file (provided in this repository) with all the information relative to the models, materials and user configuration
3. Upon launching the app, a control panel will appear in front of the user
4. By clicking the "Load Models" button, the user will trigger the download from Google Drive or simply the load of the models from the device's storage (if they were already present). At the same time the control panel will be         automatically configured, as well as the QR code tracking system to work with these specific 3D models.

![app workflow overview](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/f4dfb4e5-fcba-44de-bee5-6f2f3ac3b1a8)
 
## App user interface
The app has a moveable control panel with various buttons. Here's what each button does:

- __Load Models__: Downloads files and loads 3D models into the app.
- __Restart Size & Position__: Resets the models to their original position and size.
- __Interactable__: Lets you move, rotate, or scale the models with your hands. QR code tracking stops when you toggle this on.
- __Clear Memory__: Frees up storage by deleting files.
- __Restart Scale__: Only resets the models to their original size.

![button lineup (1)](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/4ac95100-6b73-4fa0-b28a-b43a4d0290a1)


The panel can show up to 8 model selection buttons, but it adapts to fewer models too. If there are fewer than 8 models, extra buttons get hidden.

Lastly, there's a toggle switch and a slider. The toggle switch lets you hide or show the selected model. The slider changes the model's transparency.

The complete control panel can be seen below:

<img width="300" alt="unconfigured controlpanel_v3" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/72b73308-4c98-4258-a964-6aa6a06b876c">

## App integration of QR code tracking system
The app integrates the QR code tracking architecture presented in https://github.com/garcalo/QR-code-tracking-architecture-for-HoloLens-2. The architecture is integrated in the app in such way that when the user clicks on the "Load Models" buttons all configuration needed for the tracking to waork is done automatically.
The scripts that belong to this tracking archicture are contained within the assets folder of the app, in a folder named "QR code tracking scripts".
## App configuration file

The application requieres a configuration file that needs to be present in the Google Drive folder linked to the app. This file can be seen below and contains the following information:

- __models_interactable_individually__ : this option allows the user to select if they want to be able to interact with the models one by one, that is, be able to grab them and move them around one by one, or, on the opposite, do this with all models together as a whole. This last option, enabled when the user sets this field as *"false"*,  will mean that the models cannot be separated from one to another
- __model__X____url__ : the user is requiered to provide here the link to the specific model placed in the Google drive folder so that the app can download it. This link cannot be the regular share link directly obtained from Google Drive, it has to be a direct download link. See the *"Direct download links"* section for more information on this
- __model__X___name__ : the user is required to provide the name of each of the models, which will be used in the automatic configuration of the control panel and will appear in the model selection button
- __model__X___material__url__ : the user is required to provide here the link to the specific material 


<img width="751" alt="configuration file example" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/b641233a-8074-4243-a839-6aacfd14dc1c">


## App integration with Google Drive folder
