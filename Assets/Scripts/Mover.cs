using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent] // wont allow more copies of the script working at once on the same object
public class Mover : MonoBehaviour 
{
	
	[SerializeField] Vector3 movementVector = new Vector3(10f,10f,10f);
	[SerializeField] float period = 2f;
	Vector3 startingPos; // stored for absolute movement

	// todo remove from inspector later
	[Range(0,1)][SerializeField] float movementFactor; // 0 for not moving and 1 for full move

	

	// Use this for initialization
	void Start ()
	{
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()	
	{
		// protect against period is zero
		if (period <= Mathf.Epsilon) { return; } //instead of 0
		float cycles = Time.time / period; // grows from 0

		const float tau = Mathf.PI * 2f; // about 6.28
		float rawSineWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

		movementFactor = rawSineWave / 2f + 0.5f;
		Vector3 offset = movementVector * movementFactor;
		transform.position = startingPos + offset;
		
	}
}
