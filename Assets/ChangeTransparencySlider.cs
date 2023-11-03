// Author: GAA
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class ChangeTransparencySlider : MonoBehaviour
{
    [SerializeField]
    private Renderer targetRenderer = null; 
    private Material targetMaterial; 

    private void Start()
    {
        targetMaterial = targetRenderer.material;
        Debug.Log(targetMaterial);
    }

    public void OnSliderUpdated(SliderEventData eventData)
    {
        if (targetRenderer != null)
        {
            // Change the alpha value 
            Color color = targetMaterial.color;
            Debug.Log(color);

            color.a = eventData.NewValue;
            Debug.Log("color cambiando" + color);
            targetMaterial.color = color;
            Debug.Log(targetMaterial);
        }
    }
}