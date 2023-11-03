using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Unity engine classes
using System.IO; // For working with files


// Data class definition
//public struct Data
//{
//    public float model_position_with_respect_to_qr_code_X;
//    public float model_position_with_respect_to_qr_code_Y;
//    public float model_position_with_respect_to_qr_code_Z;
//    public bool models_interactable_individually;
//    public string model1_url;
//    public string model1_name;
//    //public string model1_material_url;
//    public string model1_material_color;
//    public string model2_url;
//    public string model2_name;
//    //public string model2_material_url;
//    public string model2_material_color;
//    public string model3_url;
//    public string model3_name;
//    //public string model3_material_url;
//    public string model3_material_color;
//    public string model4_url;
//    public string model4_name;
//    //public string model4_material_url;
//    public string model4_material_color;
//    public string model5_url;
//    public string model5_name;
//    //public string model5_material_url;
//    public string model5_material_color;
//    public string model6_url;
//    public string model6_name;
//    //public string model6_material_url;
//    public string model6_material_color;
//    public string model7_url;
//    public string model7_name;
//    //public string model7_material_url;
//    public string model7_material_color;
//    public string model8_url;
//    public string model8_name;
//    //public string model8_material_url;
//    public string model8_material_color;
//    // new variables that store initial rotation and position so they can be used to restore this properties in other scripts
//    public Vector3 init_scale;
//    public Quaternion init_rotation;
//}



public class ResetPosition_and_size : MonoBehaviour
{
    private config_file_loader configLoader;
    private GameObject QRCodeVisualizer;
    private GameObject child;

    private bool isDataFull = false;

    public Data data;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        QRCodeVisualizer = GameObject.Find("QRVisualizer");
        child = GameObject.Find("ModelsHolder");
        Debug.Log("QRVisualizer: " + QRCodeVisualizer);


        //// Read Json file only once
        //if (isDataFull == false)
        //{
        //    string fileName = "ConfigurationFile";
        //    string jsonDirectory = Path.Combine(Application.persistentDataPath, "cache");
        //    string jsonPath = Path.Combine(jsonDirectory, fileName);

        //    if (File.Exists(jsonPath))
        //    {
        //        string jsonData = File.ReadAllText(jsonPath);
        //        data = JsonUtility.FromJson<Data>(jsonData);
        //        // Convert 'data' back to JSON string for debugging
        //        string jsonDataDebug = JsonUtility.ToJson(data);
        //        isDataFull = true;
        //        Debug.Log("[button] what is inside the variable data?: (reading json from folder) " + jsonDataDebug);
        //    }
        //    else
        //    {
        //        Debug.LogError("[button] Cached file not found: " + fileName);
        //    }
        //}
    }

    public void OnClickRestartSizeAndPosition()
    {
        configLoader = GetComponent<config_file_loader>();
        Data data = configLoader.GetData();


        //QRCodeVisualizer = child;

        Debug.Log("Clicked on Restart Size and Position button");

        if (QRCodeVisualizer != null)
        {
            // Set the position
            QRCodeVisualizer.transform.position = new Vector3(0, 0, 0.25f);
            //QRCodeVisualizer.transform.position = new Vector3(data.model_position_with_respect_to_qr_code_X, data.model_position_with_respect_to_qr_code_Y, data.model_position_with_respect_to_qr_code_Z);
            Debug.Log("Using: " + (data.model_position_with_respect_to_qr_code_X, data.model_position_with_respect_to_qr_code_Y, data.model_position_with_respect_to_qr_code_Z) + "Set position to: " + QRCodeVisualizer.transform.position);

            // Set the local scale
            //QRCodeVisualizer.transform.localScale = data.init_scale;
            QRCodeVisualizer.transform.localScale = new Vector3 (1, 1, 1); // currently hardcoding it 
            Debug.Log("Set local scale to: " + data.init_scale.ToString("F6"));

            // Set the rotation
            //QRCodeVisualizer.transform.rotation = data.init_rotation;
            QRCodeVisualizer.transform.rotation = new Quaternion (0, 0, 0, 1); // currently hardcoding it
            Debug.Log("Set rotation to: " + data.init_rotation);
        }
        else
        {
            Debug.LogError("QRCodeVisualizer not found");
        }

    }
}
