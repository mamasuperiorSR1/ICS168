using UnityEngine;

//Written by Benedict, any joystick code is Joshua's
public class WeaponSwap : MonoBehaviour
{
    [SerializeField] private int m_SelectedWeapon = 0;

    private bool joystick;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();

        //Debug.Log(this.transform.parent.parent.name);
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            if (this.transform.parent.parent.name.Contains("(joystick)"))
            {
                joystick = true;
            }
            else
            {
                joystick = false;
            }
        }

        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
            joystick = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStateManager.GetState() == GameStateManager.GAMESTATE.PLAYING)
        {
            int previousSelectedWeapon = m_SelectedWeapon;
            if (joystick)
            {
                if (Input.GetButtonDown("JoystickWeaponSwap"))
                {
                    if (m_SelectedWeapon >= transform.childCount - 1)
                    {
                        m_SelectedWeapon = 0;
                    }
                    else
                    {
                        m_SelectedWeapon++;
                    }
                }
            }
            else
            {

                //Weapon swap with scroll wheel
                //Scrolling up
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    if (m_SelectedWeapon >= transform.childCount - 1)
                    {
                        m_SelectedWeapon = 0;
                    }
                    else
                    {
                        m_SelectedWeapon++;
                    }
                }
                //Scrolling Down
                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    if (m_SelectedWeapon <= 0)
                    {
                        m_SelectedWeapon = transform.childCount - 1;
                    }
                    else
                    {
                        m_SelectedWeapon--;
                    }
                }

                //Weapon swap with numbers
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    m_SelectedWeapon = 0;
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
                {
                    m_SelectedWeapon = 1;
                }
                if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
                {
                    m_SelectedWeapon = 2;
                }
            }
            

            //If there is a swap, swap the weapon
            if (previousSelectedWeapon != m_SelectedWeapon)
            {
                SelectWeapon();
            }
        }
        



    }

    //Check which weapon is currently selected
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == m_SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
