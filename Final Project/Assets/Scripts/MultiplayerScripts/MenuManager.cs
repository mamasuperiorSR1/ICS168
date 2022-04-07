using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Benedict
public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManagerInstance;

    [SerializeField] private Menu[] menus;

    private void Awake()
    {
        menuManagerInstance = this;    
    }

    public void OpenMenu(string menuName)
    {
        //Loop through to find the right menu that has the same string name
        for(int i = 0; i < menus.Length; i++)
        {
            if(menus[i].MenuName == menuName)
            {
                OpenMenu(menus[i]);
            }
            else if(menus[i].Open1)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    //Open the menu that we want
    public void OpenMenu(Menu menu)
    {
        //Close any open menu
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].Open1)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
        Debug.Log("Opening " + menu.MenuName);
    }

    //Close the menu we want
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
