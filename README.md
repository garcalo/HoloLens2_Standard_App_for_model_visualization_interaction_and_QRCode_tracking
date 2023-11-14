# HoloLens2 Standard App for model visualization interaction and QRCode tracking
This repository contains an app designed to be used on Microsoft HoloLens 2. The app provides a userfriendly solution for users to visualize and interact any 3D models desired, these models can also be registered to a tracked QR code, all of this can be configured in the app's control panel.

## Instructions for using the app
The app has been developed in Unity 2021.3.9f1. The app has been designed to be used through the Holographic Remoting player, but can also be deployed using Microsoft Visual Studio.
When the repository is cloned, follow these instructions:
1. Open the project using Unity 2021.3.9f1
2. Using the MixedRealityFeatureTool (https://www.microsoft.com/en-us/download/details.aspx?id=102778) make sure you have installed the following packages:
   - Mixed Reality Toolkit Foundation - version 2.8.3
   - Mixed Reality Toolkit Standard Assets - version 2.8.3
   - Mixed Reality Toolkit Tools - version 2.8.3
   - Mixed Reality OpenXR Plugin - version 1.9.0
   
<img width="686" alt="Captura de pantalla 2023-10-17 122327" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/d440fc8a-59cb-4aee-8c40-1c29a81122a5">
<img width="688" alt="platform support" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/7763d579-a737-4d36-a0a2-8ec67982a2d5">

  
3. Install NuGet for Unity following the repository instructions: https://github.com/GlitchEnzo/NuGetForUnity. I recommend the install via .unitypackage file
    - Inside Unity, use NuGet to install the Microsoft.MixedReality.QR. For this, click on the NuGet tab, then on Manage NuGet Packages. This way you will obtain a window where you can search for the package. This can be seen in the image below:
    
      <img width="334" alt="install microsoft mixed reality qr from nuget" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/81e9cf1d-26ac-49d2-a54e-d5e1c3bbf52e">


 Once this is done, both the app and the QR code tracking architecture integrated in it can be used either using the Holographic Remoting tab in Unity to connect the device using an ip address and run the app in the computer and screencast the functionality into the HoloLens 2 device or building the project in Unity and deploying via Visual Studio.



## App information
### App workflow overview

The overall idea of the workflow is illustrated by the figure below and is the following
1. First the user is required to upload the models and materials to a Google Drive folder that is linked to the app
2. In the same Google Drive folder, the user is requiered to add and complete a Configuration file (provided in this repository) with all the information relative to the models, materials and user configuration
3. Upon launching the app, a control panel will appear in front of the user
4. By clicking the "Load Models" button, the user will trigger the download from Google Drive or simply the load of the models from the device's storage (if they were already present). At the same time the control panel will be         automatically configured, as well as the QR code tracking system to work with these specific 3D models.

![app workflow overview](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/a3da7808-e70d-475a-a005-76a98d868cbf)
 
### App user interface
The app has a moveable control panel with various buttons. Here's what each button does:

- __Load Models__: Downloads files and loads 3D models into the app.
- __Restart Size & Position__: Resets the models to their original position and size.
- __Interactable__: Lets you move, rotate, or scale the models with your hands. QR code tracking stops when you toggle this on.
- __Clear Memory__: Frees up storage by deleting files.
- __Restart Scale__: Only resets the models to their original size.


![button lineup (1)](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/62253d7c-2cf6-4e49-962c-be0c6fdd7cf9)


The panel can show up to 8 model selection buttons, but it adapts to fewer models too. If there are fewer than 8 models, extra buttons get hidden.

Lastly, there's a toggle switch and a slider. The toggle switch lets you hide or show the selected model. The slider changes the model's transparency.

The complete control panel can be seen below:

<img width="300" alt="unconfigured controlpanel_v3" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/23348d5d-c249-41d0-816f-5d1ac72e10e9">

### App integration of QR code tracking system
The app integrates the QR code tracking architecture presented in https://github.com/garcalo/QR-code-tracking-architecture-for-HoloLens-2. The architecture is integrated in the app in such way that when the user clicks on the "Load Models" buttons all configuration needed for the tracking to waork is done automatically.
The scripts that belong to this tracking archicture are contained within the assets folder of the app, in a folder named "QR code tracking scripts".


### App configuration file

The application requieres a configuration file that needs to be present in the Google Drive folder linked to the app. This file can be seen below and contains the following information:

- __models_interactable_individually__ : this option allows the user to select if they want to be able to interact with the models one by one, that is, be able to grab them and move them around one by one, or, on the opposite, do this with all models together as a whole. This last option, enabled when the user sets this field as *"false"*,  will mean that the models cannot be separated from one to another
- __modelX_url__ : the user is requiered to provide here the link to the specific 3D model placed in the Google drive folder so that the app can download it. This link cannot be the regular share link directly obtained from Google Drive, it has to be a direct download link. See the *"Direct download links from Google Drive"* section for more information on this. The ideal file format for the 3D models is .obj
- __modelX_name__ : the user is required to provide the name of each of the models, which will be used in the automatic configuration of the control panel and will appear in the model selection button
- __modelX_material__url__ : the user is required to provide here the link to the specific material. In the same way as before, this link cannot be the regular share link directly obtained from Google Drive, it has to be a direct download link. See the *"Direct download links from Google Drive"* section for more information on this. The material file format must be .mtl. In this way the app can take, for instance, materials generated using the 3D Slicer software (https://www.slicer.org/)

<img width="751" alt="configuration file example" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/f7f21376-4fdc-476c-b36c-246b26ce0bfe">

### Direct download links from Google Drive

To download the 3D models and the materials from the Google Drive folder connected to the app, the links provided by the user need to be urls that directly trigger the download of the files and not the regular "share" url obtained in the Google Drive webpage. To obtain the needed url the steps are the following:

1. Once the file is present in the Google Drive folder, the user must select it and click on the "share" icon:
   
<img width="287" alt="select share for the file" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/5dd3c1b0-d022-415f-a709-8523ef3ee130">

2. Then the user must copy the share url, making sure the visibility option selected is "Anyone with the link":

![share publicly](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/e4efd6e0-b401-4197-abdc-33cbc7ada538)

3. This link must then be pasted into the webpage *"Google Drive Direct Link Generator"* (https://sites.google.com/site/gdocs2direct/) to obtain the direct link to the file:

<img width="923" alt="google drive direct link generator" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/c0bf3910-872d-4064-a539-5969fea4968a">

<img width="919" alt="url check id" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/aef49afe-2faa-4313-8e65-aa636794ddbf">


### App integration with Google Drive folder
To connect the app with the Google Drive folder, the user must upload the filled configuration file to the Google Drive folder. Then, obtain the direct link to the file by following the instructions in the section *"Direct download links from Google Drive"* previously explained. Then this link must be pasted in the text file present in the app data, in the location Assets/Resources/Configuration file url.txt. Just like it can be seen in the image below:

<img width="587" alt="configuration file example opened in text editor" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/c5959d36-d218-42e6-99dc-9a87fde3d870">

### 3D models and materials download
When the app is initialized, the control panel appears in front of the user, then the next step for the user is to click on the *"Load Models"* button. What this does is first checking if the 3D models are already present in the device's internal storage. If they are, the app just loads them into scene as well as configuring the user interace and the QR code tracking system. If they were not present, then the download is triggered, the app will first download the configuration file and then use its information to download the models and the materials into the device's internal storage, then load everything into scene and configure the user interface and QR code tracking system. To reset this and be able to download the configuration file again, which you'll want to do every time you modify its contents, you should click on the *"Clear Memory"* button just before clicking on the *"Load Models"* button, this will erase all contents of the local folder of the device and you'll be able to download new 3D models and materials. 

### Changing the 3D models

To change the 3D models to visualize in the app, the user is not requiered to create a new configuration file, then upload it to Google Drive, then generate the Google Drive direct link and then paste this link in the .txt file that contains it. Instead, the user can go to the configuration file already present in the Google Drive folder and edit it online using the online text editor provided by Google Drive as it can be seen in the image below. In this way, when the app downloads the configuration file again, it will obtain all the new information and the user will avoid having to go through the tedious process of creating a new configuration file from scratch. 

<img width="523" alt="text editor example" src="https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/cc92cd0c-777e-4b75-a803-89d77859af48">
