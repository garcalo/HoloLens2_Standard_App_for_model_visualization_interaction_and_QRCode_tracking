using System.Collections;
using System.Collections.Generic;
using UnityEngine; // Unity engine classes
using System.IO; // For working with files


public class Restart_scale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // currently hardcoded
    public void OnClickResetScale ()
    {
        GameObject QRVisualizer = GameObject.Find("QRVisualizer");

        // Set local scale

        QRVisualizer.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("Scale reset");
    }
}
