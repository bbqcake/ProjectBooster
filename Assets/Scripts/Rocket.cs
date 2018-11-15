using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Rocket : MonoBehaviour {
	
	[SerializeField] float rcsThrust = 125f;
	[SerializeField] float mainThrust = 100f;	
	[SerializeField] AudioClip MainEngineSFX;
	[SerializeField] AudioClip DeathSFX;
	[SerializeField] AudioClip SuccessSFX;
	[SerializeField] ParticleSystem EngineParticle;
	[SerializeField] ParticleSystem SuccessParticle;
	[SerializeField] ParticleSystem DeathParticle;
	[SerializeField] float rayLength = 0.3f;
	[SerializeField] GameObject leftLander;
	[SerializeField] GameObject rightLander;

	bool rightLanderHit = false;
	bool leftLanderHit = false;
	


	Rigidbody rigidBody;
	AudioSource audioSource;
	bool debugCollision = false;

	LevelLoader levelLoader; // TODO delete perhaps?	

	enum State { Alive, Dying, Transcending }
		State state = State.Alive;

	// Use this for initialization
	void Start () 
	{
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();				
	}
	
	// Update is called once per frame
	void Update ()
	{		
		// TODO stop sound on death
		if (state == State.Alive)
		{		
			RespondToThrustInput();	
			RespontToRotateInput();	
		}		

		if (Debug.isDebugBuild)
		{
			RespondToDebugKeys();
		}	
		LeftRayCast();
		RightRayCast();				
	}	

	void LeftRayCast()
	{
		RaycastHit hitLeft;

		
		Debug.DrawRay(leftLander.transform.position, leftLander.transform.TransformDirection(Vector3.down) * rayLength, Color.cyan, 0.3f);		
		if (Physics.Raycast(leftLander.transform.position, leftLander.transform.TransformDirection(Vector3.down), out hitLeft, rayLength))
		{
			Debug.Log(hitLeft.transform.name);
			leftLanderHit = true;			
		}
		else
		{
			leftLanderHit = false;	
		}		
	}

	void RightRayCast()
	{
		RaycastHit hitRight;

		Debug.DrawRay(rightLander.transform.position, rightLander.transform.TransformDirection(Vector3.down) * rayLength, Color.red, 1f);		
		if (Physics.Raycast(rightLander.transform.position, rightLander.transform.TransformDirection(Vector3.down), out hitRight, rayLength))
		{
        	Debug.Log(hitRight.transform.name);	
			rightLanderHit = true;		
		}	
		else
		{
			rightLanderHit = false;	
		}	
	}

	

	private void RespontToRotateInput()
	{	
		
		float roationThisFrame = rcsThrust * Time.deltaTime;
		
	if (state == State.Alive && leftLanderHit == false && rightLanderHit == false)
	{
		if (Input.GetKey(KeyCode.A)) // TODO Crossplatform input
		{			
			transform.Rotate(Vector3.forward * roationThisFrame);			
		}		
		if (Input.GetKey(KeyCode.D)) // TODO Crossplatform input
		{			
			transform.Rotate(-Vector3.forward * roationThisFrame);
		}
		
		rigidBody.freezeRotation = false; // resume physics control of rotation	
					
	}

		
	}	
	

	private void RotateManually(float rotationThisFrame)
	{
		rigidBody.freezeRotation = true; //manual rotation of rotation		
		transform.Rotate(Vector3.forward * rotationThisFrame);	
		rigidBody.freezeRotation = false;
	}

	 void OnCollisionEnter(Collision collision)	
	 {

		 if (state != State.Alive || debugCollision == true)
		 {
			 return; // Ignores collisions when dead
		 }

		 switch (collision.gameObject.tag)
		 {
			case "Friendly":
				// do nothing
				print("safe to land here");
				break;

			case "Finish":
				
				StartCoroutine(StartSuccessSequence());
				break;

			default:
				StartCoroutine(StartDeathSequence());			
				break;

		 }
	 }

	 IEnumerator StartSuccessSequence()
	 {
		state = State.Transcending;	
		audioSource.Stop();
		audioSource.PlayOneShot(SuccessSFX);
		SuccessParticle.Play();	
		yield return new WaitForSeconds(2);	
		FindObjectOfType<LevelLoader>().LoadNextLevel();

	 }

	 IEnumerator StartDeathSequence()
	 {
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(DeathSFX);
		DeathParticle.Play();	
		yield return new WaitForSeconds(2);	
		FindObjectOfType<LevelLoader>().LoadFirstLevel();
	 } 



	private void RespondToThrustInput()
	{		
		if (CrossPlatformInputManager.GetButton("Jump"))
		{			
			ApplyThrust();
		}
		else
		{ 
			audioSource.mute = true; //TODO only mute engine
			EngineParticle.Stop();
		}	
	}

	private void ApplyThrust()
	{
		rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
			audioSource.mute = false;

			if (!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(MainEngineSFX);
			} 
			EngineParticle.Play();
	}

	private void RespondToDebugKeys()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			
		}

		if (Input.GetKeyDown(KeyCode.C))
		{	
			debugCollision = !debugCollision; // this is a simple toggle					
		}
	}

}




