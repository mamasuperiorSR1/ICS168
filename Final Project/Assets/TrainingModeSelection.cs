using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingModeSelection : MonoBehaviour
{
    private void Awake()
    {
        GameStateManager.Pause();
    }

    public void Selected()
    {
        GameStateManager.Resume();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
