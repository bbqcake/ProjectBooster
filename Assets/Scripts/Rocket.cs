using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		ProcessInput();
	}

	private void ProcessInput()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			print("Space pressed");
		}
		else if (Input.GetKey(KeyCode.A))
		{
			print("rotating left");
		}
		else if (Input.GetKey(KeyCode.D))
		{
			print("rotating Right");
		}
		


	}
}
