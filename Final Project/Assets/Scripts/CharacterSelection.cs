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
    public int selected = 0;
    public int selectedMeshP1 = -1;
    public int selectedMeshP2 = -1;
    public GameObject Text;
    [SerializeField]
    private Mesh[] Meshes;
    private bool gun = false;
    public GameObject guns;
    public GameObject mesh;
    public string SceneToLoad;
    //private ArrayList[] selects = new ArrayList[2];

    private void Awake()
    {
        guns.SetActive(false);
        Text.GetComponent<Text>().text = "Select P1's Skin";
    }

    public void NextCharacter()
    {
        if (gun)
        {
            characters[selected].SetActive(false);
            selected = (selected + 1) % characters.Length;
            characters[selected].SetActive(true);
        }
        else
        {
            selected = (selected + 1) % Meshes.Length;
            mesh.GetComponent<MeshFilter>().sharedMesh = Meshes[selected];
        }
        
    }
    
    public void PreviousCharacter()
    {
        if (gun)
        {
            characters[selected].SetActive(false);
            selected--;
            if (selected < 0)
            {
                selected += characters.Length;
            }
            characters[selected].SetActive(true);
        }
        else
        {
            selected--;
            if (selected < 0)
            {
                selected += Meshes.Length;
            }
            mesh.GetComponent<MeshFilter>().sharedMesh = Meshes[selected];
        }
        
    }

    public void Confirm()
    {
        if (gun)
        {
            if (selectedCharacterP1 == -1)
            {
                selectedCharacterP1 = selected;
                PlayerPrefs.SetInt("selectedCharacterP1", selectedCharacterP1);
                Debug.Log("Player1 pref set");
                //if (PlayerPrefs.GetInt("AI") == 1)
                //{
                //    SceneManager.LoadScene(sceneName: "AIvPlayer");
                //}
                //characters[selected].SetActive(false);
                selected = 0;
                characters[selected].SetActive(true);
                guns.SetActive(false);
                gun = false;
                Text.GetComponent<Text>().text = "Select P2's Skin";
                mesh.SetActive(true);
                mesh.GetComponent<MeshFilter>().sharedMesh = Meshes[selected];
            }
            else
            {
                selectedCharacterP2 = selected;
                PlayerPrefs.SetInt("selectedCharacterP2", selectedCharacterP2);
                Debug.Log("Player2 pref set");
                SceneManager.LoadScene(sceneName: SceneToLoad);
            }
        }
        else
        {
            if (selectedMeshP1 == -1)
            {
                selectedMeshP1 = selected;
                PlayerPrefs.SetInt("selectedMeshP1", selectedMeshP1);
            }
            else
            {
                selectedMeshP2 = selected;
                PlayerPrefs.SetInt("selectedMeshP2", selectedMeshP2);
            }
            selected = 0;
            mesh.SetActive(false);
            gun = true;
            guns.SetActive(true);
            if (selectedCharacterP1 == -1)
            {
                Text.GetComponent<Text>().text = "Select P1's Gun";
            }
            else
            {
                Text.GetComponent<Text>().text = "Select P2's Gun";
            }
            
        }
    }
}
