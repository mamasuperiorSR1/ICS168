using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Benedict
public class Menu : MonoBehaviour
{
    [SerializeField] private string menuName;
    [SerializeField] private bool open;

    //Getters and setters
    public string MenuName { get => menuName; set => menuName = value; }
    public bool Open1 { get => open; set => open = value; }

    //This will open the menu
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);
    }

    //This will close the menu
    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}
