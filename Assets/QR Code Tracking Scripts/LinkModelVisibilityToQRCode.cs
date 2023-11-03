//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class LinkModelVisibilityToQRCode : MonoBehaviour
//{
//    public GameObject model;
//    private QRCodesVisualizer qrCodesVisualizer;

//    private void Start()
//    {
//        qrCodesVisualizer = FindObjectOfType<QRCodesVisualizer>();
//    }

//    public void ToggleVisibility()
//    {
//        model.SetActive(!model.activeSelf);

//        if (model.activeSelf)
//        {
//            qrCodesVisualizer.AddQRCodeObject(model);
//        }
//        else
//        {
//            qrCodesVisualizer.RemoveQRCodeObject(model);
//        }
//    }
//}
