# HoloLens2 Standard App for model visualization interaction and QRCode tracking
This repository contains an app designed to be used on Microsoft HoloLens 2. The app provides a userfriendly solution for users to visualize and interact any 3D models desired, these models can also be registered to a tracked QR code, all of this can be configured in the app's control panel.
### App workflow overview

The overall idea of the workflow is illustrated by the figure below and is the following
1. First the user is required to upload the models and materials to a Google Drive folder that is linked to the app
2. In the same Google Drive folder, the user is requiered to add and complete a Configuration file (provided in this repository) with all the information relative to the models, materials and user configuration
3. Upon launching the app, a control panel will appear in front of the user
4. By clicking the "Load Models" button, the user will trigger the download from Google Drive or simply the load of the models from the device's storage (if they were already present). At the same time the control panel will be         automatically configured, as well as the QR code tracking system to work with these specific 3D models.

![app workflow overview](https://github.com/garcalo/HoloLens2_Standard_App_for_model_visualization_interaction_and_QRCode_tracking/assets/133862204/f4dfb4e5-fcba-44de-bee5-6f2f3ac3b1a8)
 
