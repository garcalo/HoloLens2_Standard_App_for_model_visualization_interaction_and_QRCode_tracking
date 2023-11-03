// GAA

using UnityEngine;


using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;


public class ToggleModelVisibility : MonoBehaviour
{
    // Assign the model GameObject either through a method parameter or another way
    public void ToggleVisibility(GameObject model)
    {
        Interactable visibilityToggle_interactable = GameObject.Find("VisibilityToggle").GetComponent<Interactable>();
        Transform child = model.transform.GetChild(0); // The associated renderer is the first child (grp1)
        Renderer renderer = child.GetComponent<Renderer>();
        Material material = renderer.material;
        Color color = material.color;
        if (visibilityToggle_interactable.IsToggled)
        {
            renderer.enabled = true;
        }
        else if (!visibilityToggle_interactable.IsToggled)
        {
            renderer.enabled = false;
        }
        material.color = color;
        assignMaterial(model, material);

        ////Debug
        //Debug.Log("Inside toggle interactable, clicked on, what model is assigned?: " + model);
    }


    void assignMaterial(GameObject model, Material material)
    {
        //Debug.Log("inside assign material");
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