using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    //Ed
    [SerializeField] private GameObject[] TimerNumsSetter;
    private static GameObject[] TimerNums;
    private static int ChildrenCount;


    private void Awake()
    {
        //Uses a serialized field array to get the timernums static field
        TimerNums = TimerNumsSetter;

        //This will activate the last item in the array and deactivate all others
        ChildrenCount = transform.childCount;
        for (int i = ChildrenCount - 1; i >= 0; i--)
        {
            if (transform.childCount - 1 == i)
            {
                TimerNums[i].SetActive(true);
            }
            else
            {
                TimerNums[i].SetActive(false);
            }
        }
    }

    public static void SetTimerUI(int TimerIdx) 
    {
        //Since TimerIdx will never be 0, I had to create the condition for when the SWAP text shows up
        if (GameStateManager.GetState() == GameStateManager.GAMESTATE.SWAP)
        {
            //Swap will always be in the 0 place
            TimerNums[0].SetActive(true);
            TimerNums[1].SetActive(false);
        }
        //Switches between 1 - 5
        else
        {
            TimerNums[TimerIdx].SetActive(true);
            if (TimerIdx + 1 == ChildrenCount)
            {
                TimerNums[0].SetActive(false);
            }
            else
            {
                TimerNums[TimerIdx + 1].SetActive(false);
            }
        }
    }

}
