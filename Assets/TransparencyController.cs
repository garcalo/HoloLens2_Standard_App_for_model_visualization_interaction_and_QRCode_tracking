// Author: GAA

using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class TransparencyController : MonoBehaviour
{
    private PinchSlider slider;

    public void SetSlider(PinchSlider slider)
    {
        this.slider = slider;
        Debug.Log("slider set");
    }

    public void SetTransparency(GameObject model)
    {
        float transparency = slider.SliderValue;
        SetModelTransparency(model, transparency);
        Debug.Log("transparency set");
    }

    private void SetModelTransparency(GameObject model, float transparency)
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();

        //Debug.Log("Number of renderers found: " + renderers.Length);

        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;

            //Debug.Log("Material found on renderer " + renderer.name);

            // Update transparency property based on material type
            if (material.HasProperty("_Color"))
            {
                // If material has a "_Color" property, modify its alpha component
                Color color = material.color;
                color.a = transparency;
                material.color = color;
                assignMaterial(model, material);
                //Debug.Log("color part " + color);
                //Debug.Log("material renderer: " + renderer.material);
                //Debug.Log("assigned material renderer color " + renderer.material.color);
            }
            else
            {
                Debug.LogWarning("Transparency property not found on material: " + material.name);
            }

            //Debug.Log("Transparency set to: " + transparency + " for material: " + material.name);
        }
    }

    void assignMaterial(GameObject model, Material material)
    {
        Transform child = model.transform.GetChild(0); // Assuming the child object with the Renderer is the first child
        Renderer renderer = child.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
        else
        {
            Debug.LogError("Failed to find Renderer component on child object.");
        }
    }


}
