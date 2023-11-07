/// GAA: modified to make it usable for the standardized app

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;



using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Threading.Tasks;

using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;


using Microsoft.MixedReality.SampleQRCodes;

using Dummiesman;

using Newtonsoft.Json; //json manager


// Jason element data
public struct Data
{
    public float model_position_with_respect_to_qr_code_X;
    public float model_position_with_respect_to_qr_code_Y;
    public float model_position_with_respect_to_qr_code_Z;
    public bool models_interactable_individually;
    public string model1_url;
    public string model1_name;
    public string model1_material_url;
    public string model1_material_color;
    public string model2_url;
    public string model2_name;
    public string model2_material_url;
    public string model2_material_color;
    public string model3_url;
    public string model3_name;
    public string model3_material_url;
    public string model3_material_color;
    public string model4_url;
    public string model4_name;
    public string model4_material_url;
    public string model4_material_color;
    public string model5_url;
    public string model5_name;
    public string model5_material_url;
    public string model5_material_color;
    public string model6_url;
    public string model6_name;
    public string model6_material_url;
    public string model6_material_color;
    public string model7_url;
    public string model7_name;
    public string model7_material_url;
    public string model7_material_color;
    public string model8_url;
    public string model8_name;
    public string model8_material_url;
    public string model8_material_color;
    public Vector3 init_scale;
    public Quaternion init_rotation;
}


public class config_file_loader : MonoBehaviour
{
    public string jsonURL; //= "https://drive.google.com/uc?export=download&id=1eTwD9mSXcWtRif_oqg8isDA7iUkRhVht";    // "https://drive.google.com/uc?export=download&id=1qqV2SmNk_PcoolE1VYi4MeU64_pDy6yg";


    bool ComponentsAdded_model1 = false;
    bool ComponentsAdded_model2 = false;
    bool ComponentsAdded_model3 = false;
    bool ComponentsAdded_model4 = false;
    bool ComponentsAdded_model5 = false;
    bool ComponentsAdded_model6 = false;
    bool ComponentsAdded_model7 = false;
    bool ComponentsAdded_model8 = false;
    bool ComponentsAdded_parent = false;
    bool QRCodeTrackable_parent = false;
    bool interactabilityButtonNotConfigured = true;
    GameObject ParentModel = null;
    bool loadModelsButtonNotConfigured = true;

    private GameObject currentModel;
    private ToggleModelVisibility currentModelToggleVisibility;
    private bool[] modelNeedsConfiguration = new bool[9]; // Array size of 9 to accommodate models 1 to 8
    private ChangeTransparencySlider currentModelTransparencySlider;


    // material management
    internal Dictionary<string, Material> Materials;

    public Data data;


    // Start is called before the first frame update
    void Start()
    {
        TextAsset ConfigFileURLtxt = Resources.Load<TextAsset>("Configuration file url");
        Debug.Log("archivo de text:" + ConfigFileURLtxt);

        if (ConfigFileURLtxt != null)
        {
            jsonURL = ConfigFileURLtxt.text;
            Debug.Log("json url: " + jsonURL);
        }

        // Initialize all elements to true initially, indicating all models need configuration
        for (int i = 1; i <= 8; i++)
        {
            modelNeedsConfiguration[i] = true;
        }
    }

    public void OnLoadDataFromDriveClick()
    {
        //Run the coroutine upon pressing a button
        Debug.Log("Button pressed: starting coroutine");
        StartCoroutine(GetData(jsonURL));

    }

    void Update()
    {
        if (data.models_interactable_individually)
        {
            CallForInteractability(data);
        }
        if (interactabilityButtonNotConfigured & ParentModel != null)
        {
            Debug.Log("Calling the configuration of the interactability button from the Update method!");
            interactabilityButtonNotConfigured = false;
            configureInteractabilityButton(ParentModel); // Parent models is modelsHolder
        }
        if (loadModelsButtonNotConfigured && ParentModel != null) // need second constraint to make sure the models have already been loaded
        {
            loadModelsButtonNotConfigured = false;
            configureModelSelectionButtons();
        }

        for (int i = 1; i <= 8; i++)
        {
            string buttonName = "Model" + i + "_button";
            Interactable buttonInteractable = GameObject.Find(buttonName).GetComponent<Interactable>();

            string modelName = GetModelName(data, i);
            GameObject model = GameObject.Find(modelName);

            // Debug information
            if (buttonInteractable == null)
            {
                Debug.Log("Button Interactable is null: " + buttonName);
            }

            if (model == null)
            {
                Debug.Log("Model not found: " + modelName);
            }

            PinchSlider pinchSlider = GameObject.Find("TransparencySlider").GetComponent<PinchSlider>();
            Interactable visibilityToggle_interactable = GameObject.Find("VisibilityToggle").GetComponent<Interactable>();


            if (buttonInteractable != null && buttonInteractable.IsToggled && modelNeedsConfiguration[i])// && model != null)
            {
                // Visibility Toggle configuration
                configureVisibilityToggle(model, buttonInteractable);
                SetVisibilityToggleToVisibilityState(model);

                // Transparency slider configuration
                configureTransparencySlider(model);
                SetTransparencySliderToModelTransparency(model);

                modelNeedsConfiguration[i] = false;
                // Set all other elements to true
                for (int j = 1; j <= 8; j++)
                {
                    if (j != i)
                    {
                        modelNeedsConfiguration[j] = true;
                    }
                }

                //// [Debug]: Print the values of modelNeedsConfiguration array
                //for (int j = 1; j <= 8; j++)
                //{
                //    Debug.Log("[1111] Model " + j + " needs configuration: " + modelNeedsConfiguration[j]);
                //}

            }
        }
    }

    public IEnumerator GetData(string url)
    {
        Debug.Log("inside GetData");
        ObtainConfigurationFile(url);
        string jsonDirectory = Path.Combine(Application.persistentDataPath, "cache");
        string jsonPath = Path.Combine(jsonDirectory, "ConfigurationFile");

        while (!File.Exists(jsonPath))
        {
            yield return new WaitForSeconds(0.001f);
            Debug.Log("waiting for 0.001 seconds for configuration file to appear in folder");
        }

        // start coroutine for all 7 models
        // First create parent for all 
        ParentModel = new GameObject("ModelsHolder");
        //ParentModel.transform.position = new Vector3(0.643418f, 1.023573f, -0.1277779f);
        //ParentModel.transform.eulerAngles = new Vector3(49.173f, -61.233f, 77.955f);
        //ParentModel.transform.position = new Vector3(data.model_position_with_respect_to_qr_code_X, data.model_position_with_respect_to_qr_code_Y, data.model_position_with_respect_to_qr_code_Z);
        Debug.Log("[config_script] modelsHolder local scale: " + (data.model_position_with_respect_to_qr_code_X, data.model_position_with_respect_to_qr_code_Y, data.model_position_with_respect_to_qr_code_Z)); //Debug
        makeInteractable(ParentModel, ref ComponentsAdded_parent);


        // Model 1
        if (data.model1_url != "")
        {
            Debug.Log("starting coroutine for model 1!");
            StartCoroutine(GetModel(data.model1_url, data.model1_material_url, data.model1_name + ".obj", ParentModel));
            Debug.Log("GetModel done for model 1");
        }
        else
        {
            Debug.Log("model 1 not present for coroutine");
        }

        // Model 2
        if (data.model2_url != "")
        {
            Debug.Log("starting coroutine for model 2!");
            StartCoroutine(GetModel(data.model2_url, data.model2_material_url, data.model2_name + ".obj", ParentModel));
            Debug.Log("GetModel done for model 2");
        }
        else
        {
            Debug.Log("model 2 not present for coroutine");
        }

        // Model 3
        if (data.model3_url != "")
        {
            Debug.Log("starting coroutine for model 3!");
            StartCoroutine(GetModel(data.model3_url, data.model3_material_url, data.model3_name + ".obj", ParentModel));
            Debug.Log("Input material for model 3 is a color");
        }
        else
        {
            Debug.Log("model 3 not present for coroutine");
        }

        // Model 4
        if (data.model4_url != "")
        {
            Debug.Log("starting coroutine for model 4!");
            StartCoroutine(GetModel(data.model4_url, data.model4_material_url, data.model4_name + ".obj", ParentModel));
            Debug.Log("Input material for model 4 is a color");
        }
        else
        {
            Debug.Log("model 4 not present for coroutine");
        }

        // Model 5
        if (data.model5_url != "")
        {
            Debug.Log("starting coroutine for model 5!");
            StartCoroutine(GetModel(data.model5_url, data.model5_material_url, data.model5_name + ".obj", ParentModel));
            Debug.Log("Input material for model 5 is a color");
        }
        else
        {
            Debug.Log("model 5 not present for coroutine");
        }

        // Model 6
        if (data.model6_url != "")
        {
            Debug.Log("starting coroutine for model 6!");
            StartCoroutine(GetModel(data.model6_url, data.model6_material_url, data.model6_name + ".obj", ParentModel));
            Debug.Log("Input material for model 6 is a color");
        }
        else
        {
            Debug.Log("model 6 not present for coroutine");
        }

        // Model 7
        if (data.model7_url != "")
        {
            Debug.Log("starting coroutine for model 7!");
            StartCoroutine(GetModel(data.model7_url, data.model7_material_url, data.model7_name + ".obj", ParentModel));
            Debug.Log("Input material for model 7 is a color");
        }
        else
        {
            Debug.Log("model 7 not present for coroutine");
        }

        // Model 8
        if (data.model8_url != "")
        {
            Debug.Log("starting coroutine for model 8!");
            StartCoroutine(GetModel(data.model8_url, data.model8_material_url, data.model8_name + ".obj", ParentModel));
            Debug.Log("Input material for model 8 is a color");
        }
        else
        {
            Debug.Log("model 8 not present for coroutine");
        }

        // create parent for QR code tracking
        GameObject QRVisualizer = new GameObject("QRVisualizer");
        ParentModel.transform.SetParent(QRVisualizer.transform);
        makeQRCodeTrackable(QRVisualizer, ref QRCodeTrackable_parent);

        // Add it to the trackermanager
        GameObject TrackerManager = GameObject.Find("TrackerManager");
        QRCodesVisualizer qrCodesVisualizer = TrackerManager.GetComponent<QRCodesVisualizer>();
        qrCodesVisualizer.qrCodePrefab = QRVisualizer;

        yield return null;
    }

    public IEnumerator GetModel(string modelUrl, string materialUrl, string modelName, GameObject ParentObject)
    {
        Debug.Log("inside GetModel");
        // Download or only load models from cache folder
        CheckCacheAndDownloadModels(modelUrl, materialUrl, modelName, ParentObject);

        yield return null;

    }

    static void EnableMaterialTransparency(Material mtl)
    {
        Debug.Log("inside EnableMaterialTransparency");
        mtl.SetFloat("_Mode", 3f);
        mtl.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mtl.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mtl.SetInt("_ZWrite", 0);
        mtl.DisableKeyword("_ALPHATEST_ON");
        mtl.EnableKeyword("_ALPHABLEND_ON");
        mtl.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mtl.renderQueue = 3000;
    }



    // Methods to manage downloads 

    public void CheckCacheAndDownloadModels(string url, string matUrl, string modelname, GameObject ParentObject)
    {
        Debug.Log("inside CheckCacheAndDownloadModels");
        // First check if the model has previously been downloaded
        string modelkey = "model_id_" + url;
        string materialKey = "material_id" + matUrl;
        string modeldirectory = Path.Combine(Application.persistentDataPath, "cache");
        string modelpath = Path.Combine(modeldirectory, modelname);
        string materialpath = modelpath.Replace(".obj", ".mtl");

        // Debug
        Debug.Log("Model: " + modelname + "key previously present: " + PlayerPrefs.HasKey(modelkey));
        Debug.Log("Material: " + modelname + "key previously present: " + PlayerPrefs.HasKey(materialKey));

        if (!PlayerPrefs.HasKey(materialKey))
        {
            // Material has not been previously downloaded

            if (!Directory.Exists(modeldirectory))
            {
                Debug.Log("Directory does not exist, creating new one at: " + modeldirectory);
                Directory.CreateDirectory(modeldirectory);
            }

            UnityWebRequest request_material = UnityWebRequest.Get(matUrl);
            request_material.downloadHandler = new DownloadHandlerBuffer();


            StartCoroutine(DownloadMaterialInfo(request_material, materialpath, materialKey, modelname));

            if (File.Exists(modelpath))
            {
                Debug.Log("[CheckCache] material " + modelname + "exists in folder " + materialpath);
            }
            else
            {
                Debug.Log("[CheckCache] material " + modelname + "does not exist in folder " + materialpath);
            }
        }


        if (!PlayerPrefs.HasKey(modelkey))
        {
            // Model has not been previously downloaded
            Debug.Log("Model has not been previously downloaded: " + modelname);

            if (!Directory.Exists(modeldirectory))
            {
                Debug.Log("Directory does not exist, creating new one at: " + modeldirectory);
                Directory.CreateDirectory(modeldirectory);
            }

            UnityWebRequest request_model = UnityWebRequest.Get(url);
            request_model.SetRequestHeader("Accept", "*/*");
            request_model.downloadHandler = new DownloadHandlerBuffer();
            


            // Use coroutine to download model
            StartCoroutine(DownloadObject(request_model, modelpath, modelkey, modelname, url, modelname, ParentObject));

            if (File.Exists(modelpath))
            {
                Debug.Log("[CheckCache] model " + modelname + "exists in folder " + modelpath);
            }
            else
            {
                Debug.Log("[CheckCache] model " + modelname + "does not exist in folder " + modelpath);
            }
        }
        else
        {
            // Model already downloaded
            Debug.Log("model file already downloaded: " + modelname);

            // Load the model from the local directory
            GameObject model = new OBJLoader().Load(modelpath);
            model.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            //model.transform.position = new Vector3(0, 0, 0);
            model.transform.position = new Vector3(0.5f, 0.775f, -0.1f);
            model.transform.eulerAngles = new Vector3(49.173f, -61.233f, 77.955f);

            // store this values to be able to use them to restore size and position
            Debug.Log("reading scale as: " + model.transform.localScale);
            Debug.Log("reading rotation as: " + model.transform.rotation);

            data.init_scale = new Vector3(0.001f, 0.001f, 0.001f); // bc of the number of decimal the code reads
            data.init_rotation = model.transform.rotation;
            Debug.Log("init stored as: scale: " + data.init_scale.ToString("F6") + " and rotation: " + data.init_rotation);

            model.transform.SetParent(ParentObject.transform);

            // Material management:

            // Load materials
            Debug.Log("material path:" + materialpath);
            MTLLoader mtlLoader = new MTLLoader();
            Dictionary<string, Material> materialDict = mtlLoader.Load(materialpath);

            // Apply the loaded materials to the loaded model's submeshes
            MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();

            Debug.Log("Available Material Names: " + string.Join(", ", materialDict.Keys));
            Debug.Log("Mesh Renderer Names: " + string.Join(", ", meshRenderers.Select(mr => mr.name)));

            // Check if materialDict is populated
            if (materialDict.Count == 0)
            {
                Debug.LogError("materialDict is empty. MTLLoader may have failed to load materials.");
            }
            else
            {
                Debug.Log("Material names in materialDict: " + string.Join(", ", materialDict.Keys));
            }

            // Check if meshRenderers array is populated
            if (meshRenderers.Length == 0)
            {
                Debug.LogError("No MeshRenderers found in model's children.");
            }
            else
            {
                Debug.Log("MeshRenderer names: " + string.Join(", ", meshRenderers.Select(mr => mr.name)));
            }

            foreach (MeshRenderer mr in meshRenderers)
            {
                if (materialDict.TryGetValue("mtl1", out Material material)) //hardcoding material name
                {
                    if (material != null)
                    {
                        // Additional material properties for HoloLens here
                        material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
                        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.SetInt("_ZWrite", 0);
                        material.DisableKeyword("_ALPHATEST_ON");
                        material.EnableKeyword("_ALPHABLEND_ON");
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                        Transform child = model.transform.GetChild(0); // Assuming the child object with the Renderer is the first child
                        Renderer renderer = child.GetComponent<Renderer>();
                        if (renderer != null)
                        {
                            renderer.material = material;
                            Debug.Log("Material created is: " + material);
                        }
                        else
                        {
                            Debug.LogError("Failed to find Renderer component on child object.");
                        }
                    }
                }
                else
                {
                    Debug.Log("Material not found for mesh: " + mr.name);
                }
            }
        }


    }

    void ObtainConfigurationFile(string url)
    {
        // Check if the file has been previously downloaded
        string jsonKey = "ModelDownloaded_" + url;
        string fileName = "ConfigurationFile";

        string jsonDirectory = Path.Combine(Application.persistentDataPath, "cache");
        string jsonPath = Path.Combine(jsonDirectory, fileName);

        if (!PlayerPrefs.HasKey(jsonKey))
        {
            // File not previously downloaded
            Debug.Log("Json file has been updated and is to be downloaded: " + fileName);

            if (!Directory.Exists(jsonDirectory))
            {
                Debug.Log("Creating directory: " + jsonDirectory);
                Directory.CreateDirectory(jsonDirectory);
            }

            Debug.Log("json url prev to donwload:" + url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Use Coroutine to download the file
            StartCoroutine(DownloadFile(request, jsonPath, jsonKey, fileName));
        }
        else
        {
            // File already downloaded, load from cache
            Debug.Log("Json file already downloaded: " + fileName);

            if (File.Exists(jsonPath))
            {
                string jsonData = File.ReadAllText(jsonPath);
                data = JsonUtility.FromJson<Data>(jsonData);
                // Convert 'data' back to JSON string for debugging
                string jsonDataDebug = JsonUtility.ToJson(data);

                Debug.Log("what is inside the variable data?: (reading json from folder) " + jsonDataDebug);
            }
            else
            {
                Debug.LogError("Cached file not found: " + fileName);
            }
        }
    }

    IEnumerator DownloadFile(UnityWebRequest request, string jsonPath, string jsonKey, string fileName)
    {
        Debug.Log("inside DownloadFile");
        Debug.Log("inside DownloadFile, with json path:" + jsonPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(jsonPath, request.downloadHandler.data);

            // Update the model name in PlayerPrefs
            PlayerPrefs.SetString(jsonKey, fileName);
            PlayerPrefs.Save();

            Debug.Log("Json file downloaded successfully");
            Debug.Log("JSON data: " + System.Text.Encoding.Default.GetString(request.downloadHandler.data));

            // Parse the downloaded JSON data
            data = JsonUtility.FromJson<Data>(System.Text.Encoding.Default.GetString(request.downloadHandler.data));

        }
        else
        {
            Debug.LogError("Failed to download the JSON file. Error: " + request.error);
        }
    }

    IEnumerator DownloadMaterialInfo(UnityWebRequest request, string materialPath, string materialKey, string fileName)
    {
        Debug.Log("inside DownloadMaterial info");
        Debug.Log("Before yield return request.SendWebRequest()");
        yield return request.SendWebRequest();
        Debug.Log("After yield return request.SendWebRequest()");

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Log the raw bytes in downloadHandler.data
            string rawDataString = System.Text.Encoding.Default.GetString(request.downloadHandler.data);
            Debug.Log("Raw data in downloadHandler: " + rawDataString);

            File.WriteAllBytes(materialPath, request.downloadHandler.data);

            Debug.Log("Material information file downloaded successfully");

            // Update the model name in PlayerPrefs
            PlayerPrefs.SetString(materialKey, fileName);
            PlayerPrefs.Save();

            // Check the file size
            Debug.Log("Downloaded File Size: " + request.downloadHandler.data.Length);

            // Check encoding
            Debug.Log("Downloaded File Encoding: " + System.Text.Encoding.Default.WebName);


            Debug.Log("Material information file downloaded successfully");
            Debug.Log("Material data: " + System.Text.Encoding.Default.GetString(request.downloadHandler.data));

        }
        else
        {
            Debug.LogError("Failed to download the Material information file. Error: " + request.error);
        }
    }

    IEnumerator DownloadObject(UnityWebRequest request_model, string DirPath, string Key, string fileName, string url, string modelname, GameObject ParentObject)   
    {
        Debug.Log("inside DownloadObject");
        yield return request_model.SendWebRequest();


        //yield return new WaitUntil(() => request.result == UnityWebRequest.Result.Success);
        Debug.Log("send request result: " + request_model.result);
        Debug.Log("send request error: " + request_model.error);
        Debug.Log("send request handler: " + request_model.downloadHandler);
        //yield return request.SendWebRequest();

        if (request_model.result == UnityWebRequest.Result.Success)
        {
            File.WriteAllBytes(DirPath, request_model.downloadHandler.data);
            //Debug.Log("calling wait until");
            //yield return new WaitUntil(() => File.Exists(Path));


            // Update the model name in PlayerPrefs
            PlayerPrefs.SetString(Key, fileName);
            PlayerPrefs.Save();
            Debug.Log("Object downloaded successfully:  " + fileName);

            if (File.Exists(DirPath))
            {
                Debug.Log("[DownloadObject] model is in folder: " + fileName);
            }

            while (!File.Exists(DirPath))
            {
                yield return new WaitForSeconds(0.001f);
                Debug.Log("waiting for 0.001 seconds for " + fileName + "to appear in folder");
            }

        }
        else
        {
            Debug.LogError("Failed to download Object. Error: " + request_model.error);
        }

        // Check cache and load material after donwload: all within the same timeline of the asynchronous method
        
        // First check if the model has previously been downloaded
        string modelkey = "model_id_" + url;
        string modeldirectory = Path.Combine(Application.persistentDataPath, "cache");
        string modelpath = Path.Combine(modeldirectory, modelname);


        // Load the model from the local directory
        GameObject model = new OBJLoader().Load(modelpath);
        model.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        model.transform.position = new Vector3(0, 0, 0);
        //return modelpath;
        //Debug.Log("[config file loader] Using: " + (data.model_position_with_respect_to_qr_code_X, data.model_position_with_respect_to_qr_code_Y, data.model_position_with_respect_to_qr_code_Z));

        // store this values to be able to use them to restore size and position
        Debug.Log("reading scale as: " + model.transform.localScale);
        Debug.Log("reading rotation as: " + model.transform.rotation);

        data.init_scale = new Vector3(0.001f, 0.001f, 0.001f); // bc of the number of decimal the code reads
        data.init_rotation = model.transform.rotation;
        Debug.Log("init stored as: scale: " + data.init_scale.ToString("F6") + " and rotation: " + data.init_rotation);

        model.transform.SetParent(ParentObject.transform);

        // Managing material:

        // Load materials
        string materialpath = modelpath.Replace(".obj", ".mtl");
        Debug.Log("material path:" + materialpath);
        MTLLoader mtlLoader = new MTLLoader();
        Dictionary<string, Material> materialDict = mtlLoader.Load(materialpath);

        // Apply the loaded materials to the loaded model's submeshes
        MeshRenderer[] meshRenderers = model.GetComponentsInChildren<MeshRenderer>();

        Debug.Log("Available Material Names: " + string.Join(", ", materialDict.Keys));
        Debug.Log("Mesh Renderer Names: " + string.Join(", ", meshRenderers.Select(mr => mr.name)));

        // Check if materialDict is populated
        if (materialDict.Count == 0)
        {
            Debug.LogError("materialDict is empty. MTLLoader may have failed to load materials.");
        }
        else
        {
            Debug.Log("Material names in materialDict: " + string.Join(", ", materialDict.Keys));
        }

        // Check if meshRenderers array is populated
        if (meshRenderers.Length == 0)
        {
            Debug.LogError("No MeshRenderers found in model's children.");
        }
        else
        {
            Debug.Log("MeshRenderer names: " + string.Join(", ", meshRenderers.Select(mr => mr.name)));
        }

        foreach (MeshRenderer mr in meshRenderers)
        {
            if (materialDict.TryGetValue("mtl1", out Material material)) //hardcoding material name
            {
                if (material != null)
                {
                    // Additional material properties for HoloLens here
                    material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    Transform child = model.transform.GetChild(0); // Assuming the child object with the Renderer is the first child
                    Renderer renderer = child.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = material;
                        Debug.Log("Material created is: " + material);
                    }
                    else
                    {
                        Debug.LogError("Failed to find Renderer component on child object.");
                    }
                }
            }
            else
            {
                Debug.Log("Material not found for mesh: " + mr.name);
            }
        }

    }


    // Configure model functionalities 

    void CallForInteractability(Data data)
    {
        // start coroutine for all 7 models
        GameObject model1 = GameObject.Find(data.model1_name);
        GameObject model2 = GameObject.Find(data.model2_name);
        GameObject model3 = GameObject.Find(data.model3_name);
        GameObject model4 = GameObject.Find(data.model4_name);
        GameObject model5 = GameObject.Find(data.model5_name);
        GameObject model6 = GameObject.Find(data.model6_name);
        GameObject model7 = GameObject.Find(data.model7_name);
        GameObject model8 = GameObject.Find(data.model8_name);

        // Model 1
        if (data.model1_url != "" && model1 != null)
        {
            Debug.Log("starting interactability coroutine for model 1! Model name: " + data.model1_name);
            makeInteractable(model1, ref ComponentsAdded_model1);
            Debug.Log("model 1 made interactable!");
        }
        else if (model1 == null)
        {
            Debug.Log("model 1 not claimed: model null");
        }
        else
        {
            Debug.Log("model 1 not present for interactability coroutine");
        }

        // Model 2
        if (data.model2_url != "" && model2 != null)
        {
            Debug.Log("starting interactability coroutine for model 2! Model name: " + data.model2_name);
            makeInteractable(model2, ref ComponentsAdded_model2);
            Debug.Log("model 2 made interactable!");
        }
        else if (model2 == null)
        {
            Debug.Log("model 2 not claimed: model null");
        }
        else
        {
            Debug.Log("model 2 not present for interactability coroutine");
        }

        // Model 3
        if (data.model3_url != "" && model3 != null)
        {
            Debug.Log("starting interactability coroutine for model 3! Model name: " + data.model3_name);
            makeInteractable(model3, ref ComponentsAdded_model3);
            Debug.Log("model 3 made interactable!");
        }
        else if (model3 == null)
        {
            Debug.Log("model 3 not claimed: model null");
        }
        else
        {
            Debug.Log("model 3 not present for interactability coroutine");
        }

        // Model 4
        if (data.model4_url != "" && model4 != null)
        {
            Debug.Log("starting interactability coroutine for model 4! Model name: " + data.model4_name);
            makeInteractable(model4, ref ComponentsAdded_model4);
            Debug.Log("model 4 made interactable!");
        }
        else if (model4 == null)
        {
            Debug.Log("model 4 not claimed: model null");
        }
        else
        {
            Debug.Log("model 4 not present for interactability coroutine");
        }

        // Model 5
        if (data.model5_url != "" & model5 != null)
        {
            Debug.Log("starting interactability coroutine for model 5! Model name: " + data.model5_name);
            makeInteractable(model5, ref ComponentsAdded_model5);
            Debug.Log("model 5 made interactable!");
        }
        else if (model5 == null)
        {
            Debug.Log("model 5 not claimed: model null");
        }
        else
        {
            Debug.Log("model 5 not present for interactability coroutine");
        }

        // Model 6
        if (data.model6_url != "" && model6 != null)
        {
            Debug.Log("starting interactability coroutine for model 6! Model name: " + data.model6_name);
            makeInteractable(model6, ref ComponentsAdded_model6);
            Debug.Log("model 6 made interactable!");
        }
        else if (model6 == null)
        {
            Debug.Log("model 6 not claimed: model null");
        }
        else
        {
            Debug.Log("model 6 not present for interactability coroutine");
        }

        // Model 7
        if (data.model7_url != "" && model7 != null)
        {
            Debug.Log("starting interactability coroutine for model 7! Model name: " + data.model7_name);
            makeInteractable(model7, ref ComponentsAdded_model7);
            Debug.Log("model 7 made interactable!");
        }
        else if (model7 == null)
        {
            Debug.Log("model 7 not claimed: model null");
        }
        else
        {
            Debug.Log("model 7 not present for interactability coroutine");
        }
        // Model 8
        if (data.model8_url != "" && model8 != null)
        {
            Debug.Log("starting interactability coroutine for model 8! Model name: " + data.model8_name);
            makeInteractable(model8, ref ComponentsAdded_model8);
            Debug.Log("model 8 made interactable!");
        }
        else if (model8 == null)
        {
            Debug.Log("model 8 not claimed: model null");
        }
        else
        {
            Debug.Log("model 8 not present for interactability coroutine");
        }
    } // used only in case we want the models to be interacable separately

    void makeInteractable(GameObject model, ref bool ComponentsAdded)
    {
        Debug.Log("Inside makeInteractable method with " + model);
        Debug.Log(model);
        Debug.Log("Components added:" + ComponentsAdded);
        while (!ComponentsAdded)
        {
            // Add components to each of the GameObjects loaded
            Debug.Log("calling AddAllNeededComponents method");
            AddAllNeededComponents(model);
            ComponentsAdded = true;
            Debug.Log("Components added value: " + ComponentsAdded);

        }
        if (ComponentsAdded) // Verify components have been added
        {
            Debug.Log("Components have been added to the GameObjects");

        }
    }

    void AddAllNeededComponents(GameObject mymodel)
    {
        Debug.Log("inside the method AddAllNeededComponents");
        mymodel.AddComponent<BoxCollider>();
        mymodel.AddComponent<NearInteractionGrabbable>();
        mymodel.AddComponent<ObjectManipulator>();

        // For the interactability button
        mymodel.AddComponent<ToggleCollider>();

        Debug.Log("model has been added box collider, near interaction grabbable and objectmanipulator");
        // Include a valid size box collider 
        // Get the Renderer component of the GameObject
        Renderer renderer = mymodel.GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.Log("Renderer component not found on the model.Adding one");
            mymodel.AddComponent<MeshRenderer>();
            renderer = mymodel.GetComponent<Renderer>();
        }

        // Get the Box Collider component
        BoxCollider boxCollider = mymodel.GetComponent<BoxCollider>();

        boxCollider.size = new Vector3(0.25f, 0.25f, 0.15f);
        boxCollider.center = new Vector3(0, 0.07f, 0);
        // We want the collider to be disabled at Start
        boxCollider.enabled = false;

        Debug.Log("All components added to the model");
    }

    void makeQRCodeTrackable(GameObject TrackerVisualizer, ref bool QRCodeTrackable)
    {
        while (!QRCodeTrackable)
        {
            // Add components to visualize the tracking
            TrackerVisualizer.AddComponent<SpatialGraphNodeTracker>();
            //TrackerVisualizer.AddComponent<QRCode>(); // not needed
            QRCodeTrackable = true;
        }
    }

    void configureInteractabilityButton(GameObject modelsHolder)
    {
        Debug.Log("inside button configurator method!");
        GameObject TrackerManager = GameObject.Find("TrackerManager");
        Debug.Log("TrackerManager found: " + TrackerManager);

        // Get a reference to the Interactable component on the button
        Interactable myButtonInteractable = GameObject.Find("Interactable&OffsetButton").GetComponent<Interactable>();

        // Add an event handler for the OnClick event
        myButtonInteractable.OnClick.AddListener(delegate { modelsHolder.GetComponent<ToggleCollider>().ToggleColliderState(); });
        myButtonInteractable.OnClick.AddListener(delegate { TrackerManager.GetComponent<QRCodesVisualizer>().changeAllowOffset(); });

        Debug.Log("Button configured!");
    }

    void configureModelSelectionButtons()
    {
        Debug.Log("Starting model selection buttons configuration!");


        // First set visibility of buttons only if the models are present
        GameObject buttonModel1 = GameObject.Find("Model1_button");

        Debug.Log("button model 1" + buttonModel1); // debug

        if (data.model1_url == "")
        {
            Debug.Log("Hiding model 1 button!");
            setGameObjectVisibility(buttonModel1, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 1 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel1.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model1_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel2 = GameObject.Find("Model2_button");
        if (data.model2_url == "")
        {
            Debug.Log("Hiding model 2 button!");
            setGameObjectVisibility(buttonModel2, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 2 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel2.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model2_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel3 = GameObject.Find("Model3_button");
        if (data.model3_url == "")
        {
            Debug.Log("Hiding model 3 button!");
            setGameObjectVisibility(buttonModel3, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 3 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel3.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model3_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel4 = GameObject.Find("Model4_button");
        if (data.model4_url == "")
        {
            Debug.Log("Hiding model 4 button!");
            setGameObjectVisibility(buttonModel4, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 4 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel4.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model4_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel5 = GameObject.Find("Model5_button");
        if (data.model5_url == "")
        {
            Debug.Log("Hiding model 5 button!");
            setGameObjectVisibility(buttonModel5, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 5 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel5.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model5_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel6 = GameObject.Find("Model6_button");
        if (data.model6_url == "")
        {
            Debug.Log("Hiding model 6 button!");
            setGameObjectVisibility(buttonModel6, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 6 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel6.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model6_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel7 = GameObject.Find("Model7_button");
        if (data.model7_url == "")
        {
            Debug.Log("Hiding model 7 button!");
            setGameObjectVisibility(buttonModel7, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 7 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel7.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model7_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }


        GameObject buttonModel8 = GameObject.Find("Model8_button");
        if (data.model8_url == "")
        {
            Debug.Log("Hiding model 8 button!");
            setGameObjectVisibility(buttonModel8, false);
        }
        else //if the model is present, configure button functionality
        {
            Debug.Log("changing model 8 button");
            ButtonConfigHelper buttonConfigHelper = buttonModel8.GetComponent<ButtonConfigHelper>();

            if (buttonConfigHelper != null)
            {
                // Set the label text through the ButtonConfigHelper
                buttonConfigHelper.MainLabelText = data.model8_name;
            }
            else
            {
                Debug.LogError("ButtonConfigHelper component not found on the button!");
            }
        }
    }


    // [UI] Visibility Toggle configuration

    void configureVisibilityToggle(GameObject model, Interactable selectionButtonInteractable)
    {
        Interactable visibilityToggle_interactable = GameObject.Find("VisibilityToggle").GetComponent<Interactable>();

        // Now add visibility toggle component to the model 
        currentModelToggleVisibility = model.GetComponent<ToggleModelVisibility>();
        if (currentModelToggleVisibility == null)
        {
            currentModelToggleVisibility = model.AddComponent<ToggleModelVisibility>(); //This is a component
        }

        visibilityToggle_interactable.OnClick.RemoveAllListeners();
        UnityAction action = delegate { currentModelToggleVisibility.ToggleVisibility(model); };
        visibilityToggle_interactable.OnClick.AddListener(action);

        Debug.Log("Listener added to the visibility toggle.");
    }

    void SetVisibilityToggleToVisibilityState(GameObject model)
    {
        GameObject visibilityToggle = GameObject.Find("VisibilityToggle");
        Interactable visibilityToggle_interactable = visibilityToggle.GetComponent<Interactable>();

        if (visibilityToggle != null)
        {
            Transform child = model.transform.GetChild(0);
            Renderer renderer = child.GetComponent<Renderer>();
            bool visibilityState = renderer.enabled; // Get the visibility state of the model
            visibilityToggle_interactable.IsToggled = visibilityState; // Set the toggle state accordingly
        }
    }


    // [UI] Transparency Slider configuration 

    void configureTransparencySlider(GameObject model)
    {

        // Find the PinchSlider component
        PinchSlider pinchSlider = GameObject.Find("TransparencySlider").GetComponent<PinchSlider>();
        //Debug.Log("pinch slider claimed: " + pinchSlider);

        // Add or update the transparency controller component
        TransparencyController transparencyController = model.GetComponent<TransparencyController>();
        if (transparencyController == null)
        {
            transparencyController = model.AddComponent<TransparencyController>();
        }
        //Debug.Log("transparency controller claimed: " + transparencyController);

        // Set the slider reference in the TransparencyController script
        transparencyController.SetSlider(pinchSlider);
        //Debug.Log("Slider set in transparency controller");

        // Remove all existing listeners from the slider
        pinchSlider.OnValueUpdated.RemoveAllListeners();
        //Debug.Log("Existing listeners removed from pinch slider");

        // Add a listener for the slider value change event
        pinchSlider.OnValueUpdated.AddListener(delegate { transparencyController.SetTransparency(model); });
        //Debug.Log("Listener added to pinch slider");

        // Set the current model
        currentModel = model;

        Debug.Log("Listener added to the transparency slider.");
    }

    void SetTransparencySliderToModelTransparency(GameObject model)
    {
        Debug.Log("getting model transparency alpha value");

        Renderer renderer = model.GetComponentInChildren<Renderer>();
        Material modelmaterial = renderer.material;
        PinchSlider pinchSlider = GameObject.Find("TransparencySlider").GetComponent<PinchSlider>();

        if (modelmaterial.HasProperty("_Color"))
        {
            Color materialcolor = modelmaterial.color;
            float transparencyValue = materialcolor.a;
            Debug.Log("transparency value found: " + transparencyValue);
            pinchSlider.SliderValue = transparencyValue;
        }
        else
        {
            Debug.LogError("material has no color property");
            pinchSlider.SliderValue = 1;
        }
    }


    // useful methods

    void clearListeners(Interactable interactable)
    {
        interactable.OnClick.RemoveAllListeners();
        Debug.Log("All listeners removed");
    }

    void clearListeners_slider(PinchSlider slider)
    {
        slider.OnValueUpdated.RemoveAllListeners();
        Debug.Log("All listeners removed");
    }


    void setGameObjectVisibility(GameObject model, bool selection)
    {
        model.SetActive(selection);
    }

    string GetModelName(Data data, int index)
    {
        switch (index)
        {
            case 1:
                return data.model1_name;
            case 2:
                return data.model2_name;
            case 3:
                return data.model3_name;
            case 4:
                return data.model4_name;
            case 5:
                return data.model5_name;
            case 6:
                return data.model6_name;
            case 7:
                return data.model7_name;
            case 8:
                return data.model8_name;
            default:
                return null;
        }
    }

    async void WaitUntilFileIsPresent(string Path)
    {
        while (!File.Exists(Path))
        {
            await Task.Delay(1);
        }
    }

    public Data GetData()
    {
        return data;
    }

    // debugging methods

    void PrintAllPlayerPrefsData()
    {
        Debug.Log("Printing all PlayerPrefs data:");

        // Get all keys stored in PlayerPrefs
        string[] keys = new string[PlayerPrefs.GetInt("PlayerPrefsKeysCount")];
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i] = PlayerPrefs.GetString("PlayerPrefsKey" + i);
        }

        // Print values for each key
        foreach (string key in keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                if (PlayerPrefs.GetString(key) != "")
                {
                    Debug.Log(key + ": " + PlayerPrefs.GetString(key));
                }
                else if (PlayerPrefs.GetInt(key) != 0)
                {
                    Debug.Log(key + ": " + PlayerPrefs.GetInt(key));
                }
                else if (PlayerPrefs.GetFloat(key) != 0f)
                {
                    Debug.Log(key + ": " + PlayerPrefs.GetFloat(key));
                }
                else if (PlayerPrefs.GetInt(key) == 0)
                {
                    Debug.Log(key + ": " + false);
                }
            }
        }
    }

}
