using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Written by Benedict; watched a tutorial on how Quaternions work with weapon swaying
public class WeaponSway : MonoBehaviourPunCallbacks
{
    [Header("Sway Settings")]
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;
    private PhotonView view;

    private void Awake()
    {
        view = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
            //get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            //calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            //You can just multiply them since they are Quaternions
            Quaternion targetRotation = rotationX * rotationY;

            //rotate
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else if(GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE && view.IsMine)
        {
            //get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

            //calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            //You can just multiply them since they are Quaternions
            Quaternion targetRotation = rotationX * rotationY;

            //rotate
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
    }
}
