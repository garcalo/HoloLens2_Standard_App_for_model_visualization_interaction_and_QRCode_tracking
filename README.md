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
- __modelX_url__ : the user is requiered to provide here the link to the specific 3D model placed in the Google drive folder so that the app can download it. This link cannot be the regular share link directly obtained from Google Drive, it has to be a direct download link. See the *"Direct download links from Google Drive"* section for more information on this. The ideal file format for the 3D models is .obj
- __modelX_name__ : the user is required to provide the name of each of the models, which will be used in the automatic configuration of the control panel and will appear in the model selection button
- __modelX_material__url__ : the user is required to provide here the link to the specific material. In the same way as before, this link cannot be the regular share link directly obtained from Google Drive, it has to be a direct download link. See the *"Direct download links from Google Drive"* section for more information on this. The material file format must be .mtl. In this way the app can take, for instance, materials generated using the 3D Slicer software (https://www.slicer.org/)


<img width="751" alt="configuration file example" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/b641233a-8074-4243-a839-6aacfd14dc1c">

## Direct download links from Google Drive

To download the 3D models and the materials from the Google Drive folder connected to the app, the links provided by the user need to be urls that directly trigger the download of the files and not the regular "share" url obtained in the Google Drive webpage. To obtain the needed url the steps are the following:

1. Once the file is present in the Google Drive folder, the user must select it and click on the "share" icon:
   
<img width="287" alt="select share for the file" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/03678333-bf60-49aa-a50c-b81d296baa78">

2. Then the user must copy the share url, making sure the visibility option selected is "Anyone with the link":

![share publicly](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/e4efd6e0-b401-4197-abdc-33cbc7ada538)

3. This link must then be pasted into the webpage *"Google Drive Direct Link Generator"* (https://sites.google.com/site/gdocs2direct/) to obtain the direct link to the file:

![google drive direct link generator](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/7fa78d25-5fd0-4b37-a1dc-3e4745d42380)
<img width="600" alt="url check id" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/bc0c9c01-2890-4910-8bd4-9fee4f60d4a7">


## App integration with Google Drive folder
