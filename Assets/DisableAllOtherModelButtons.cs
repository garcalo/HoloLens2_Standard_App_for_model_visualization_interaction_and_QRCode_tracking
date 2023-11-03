using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


using Microsoft;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class DisableAllOtherModelButtons : MonoBehaviour
{
    public 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ifEnabledOnClick()
    {
        // First get the number from the GameObject's name
        int ButtonNumber = ExtractNumberFromName(gameObject.name);
        Debug.Log("Button number: " + ButtonNumber);

        DissableButtonBut(ButtonNumber);

    }

    private int ExtractNumberFromName(string name)
    {
        // Use regular expression to match the number in the name
        Match match = Regex.Match(name, @"\d+");

        // Check if a match was found
        if (match.Success)
        {
            // Parse the matched number string to an integer
            int number;
            if (int.TryParse(match.Value, out number))
            {
                return number;
            }
        }

        // Return a default value if no number was found or parsing failed
        return -1;
    }

    private void DissableButtonBut(int currentButtonNumber)
    {
        for (int i = 1; i<=8; i++)
        {
            if (i != currentButtonNumber)
            {
                // Generate button number
                string buttonName = "Model" + i.ToString() + "_button";

                // Dissable button
                Interactable button = GameObject.Find(buttonName).GetComponent<Interactable>();
                button.IsToggled = false;
            }

        }

    }
}