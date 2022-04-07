using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Created by Ed Slee
/// <summary>
/// Disables the UI for the other player, so client can't see it
/// </summary>
public class OnlineUIDisabler : MonoBehaviourPunCallbacks
{

    private void Awake()
    {
        if (!GetComponentInParent<PhotonView>().IsMine)
        {

            for(int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

}
