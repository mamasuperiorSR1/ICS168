using UnityEngine;
using System.Collections;

/**
 *	Rapidly sets a light on/off.
 *	
 *	(c) 2015, Jean Moreno
**/

[RequireComponent(typeof(Light))]
public class WFX_LightFlicker : MonoBehaviour
{
	public float time = 0.05f;
	
	private float timer;

	private string inputAxis;
	
	void Start ()
	{
		//Debug.Log(this.transform.parent.parent.parent.parent.parent.name);
		timer = time;

		//Local
		if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.LOCAL)
        {
			if (this.transform.parent.parent.parent.parent.parent.name.Contains("(joystick)"))
			{
				inputAxis = "JoystickFire";
			}
			else
			{
				inputAxis = "KeyboardFire";
			}
		}

		//Multiplayer
		if (GameStateManager.GetMultiplayState() == GameStateManager.MULTIPLAY.ONLINE)
        {
			inputAxis = "KeyboardFire";
		}

			//Debug.Log(inputAxis);

			StartCoroutine("Flicker");

	}
	
	IEnumerator Flicker()
	{
		if (Input.GetButton(inputAxis)) ;
		{
			GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
			
			do
			{
				timer -= Time.deltaTime;
				yield return null;
			}
			while(timer > 0);
			timer = time;
		}
	}
}
