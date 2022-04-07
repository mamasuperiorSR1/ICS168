using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PowerupUI : MonoBehaviour
{
    [SerializeField] private Text powerupText;

    //Set the text of the UI
    public void SetPowerUpText(string text)
    {
        powerupText.text = text;
    }
}
