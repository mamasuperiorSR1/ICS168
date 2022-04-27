using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    //[SerializeField]
    public GameObject[] characters;
    public int selectedCharacterP1 = -1;
    public int selectedCharacterP2 = -1;
    public int selectedCharacter = 0;
    public GameObject Text;

    public void NextCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        characters[selectedCharacter].SetActive(true);
    }
    
    public void PreviousCharacter()
    {
        characters[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        characters[selectedCharacter].SetActive(true);
    }

    public void Confirm()
    {
        if (selectedCharacterP1 == -1)
        {
            selectedCharacterP1 = selectedCharacter;
            PlayerPrefs.SetInt("selectedCharacterP1", selectedCharacterP1);
            Debug.Log("Player1 pref set");
            characters[selectedCharacter].SetActive(false);
            selectedCharacter = 0;
            characters[selectedCharacter].SetActive(true);
            Text.GetComponent<Text>().text = "Select P2's Gun";
        }
        else
        {
            selectedCharacterP2 = selectedCharacter;
            PlayerPrefs.SetInt("selectedCharacterP2", selectedCharacterP2);
            Debug.Log("Player2 pref set");
            SceneManager.LoadScene(sceneName: "Game (Map 1 Copy)");
        }
        
        
    }
}
