using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
	
	[SerializeField] float rcsThrust = 125f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] float levelLoadDelay = 2f;	[SerializeField] AudioClip MainEngineSFX;
	[SerializeField] AudioClip DeathSFX;
	[SerializeField] AudioClip SuccessSFX;
	[SerializeField] ParticleSystem EngineParticle;
	[SerializeField] ParticleSystem SuccessParticle;
	[SerializeField] ParticleSystem DeathParticle;


	Rigidbody rigidBody;
	AudioSource audioSource;

	bool debugCollision = false;

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
		RespontToRotateInput();
		RespondToThrustInput();
		}
		if (Debug.isDebugBuild)
		{
			RespondToDebugKeys();
		}
		
		
	}

	private void RespontToRotateInput()
	{
		rigidBody.freezeRotation = true; //manual rotation of rotation

		float roationThisFrame = rcsThrust * Time.deltaTime;

		if (Input.GetKey(KeyCode.A))
		{			
			transform.Rotate(Vector3.forward * roationThisFrame);			
		}		
		else if (Input.GetKey(KeyCode.D))
		{			
			transform.Rotate(-Vector3.forward * roationThisFrame);			
		}

		rigidBody.freezeRotation = false; // resume physics control of rotation
	}	


	private void RespondToDebugKeys()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			LoadNextLevel();
		}

		if (Input.GetKeyDown(KeyCode.C))
		{	
			debugCollision = !debugCollision; // this is a simple toggle					
		}
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
				
				StartSuccessSequence();
				break;

			default:
				StartDeathSequence();			
				break;

		 }
	 }

	 private void StartSuccessSequence()
	 {
		state = State.Transcending;	
		audioSource.Stop();
		audioSource.PlayOneShot(SuccessSFX);
		SuccessParticle.Play();			
		Invoke("LoadNextLevel", levelLoadDelay);
	 }

	 private void StartDeathSequence()
	 {
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(DeathSFX);
		DeathParticle.Play();			
		Invoke("LoadFirstLevel", levelLoadDelay);		
	 }

	 private void LoadNextLevel()
	 {
		 //TODO allow more levels
		 SceneManager.LoadScene(1);
	 }

	 private void LoadFirstLevel()
	 {
		 SceneManager.LoadScene(0);
	 }



	private void RespondToThrustInput()
	{		
		if (Input.GetKey(KeyCode.Space))
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

}




