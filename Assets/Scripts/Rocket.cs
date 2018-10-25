using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

	// TODO fix lighting big
	[SerializeField] float rcsThrust = 125f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] AudioClip MainEngineSFX;
	[SerializeField] AudioClip DeathSFX;
	[SerializeField] AudioClip SuccessSFX;


	Rigidbody rigidBody;
	AudioSource audioSource;

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


	 void OnCollisionEnter(Collision collision)	
	 {

		 if (state != State.Alive)
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
		Invoke("LoadNextLevel", 1f); // load after one second
	 }

	 private void StartDeathSequence()
	 {
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(DeathSFX);			
		Invoke("LoadFirstLevel", 1f);		
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
		}	
	}

	private void ApplyThrust()
	{
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
			audioSource.mute = false;

			if (!audioSource.isPlaying)
			{
				audioSource.PlayOneShot(MainEngineSFX);
			} 
	}

}




