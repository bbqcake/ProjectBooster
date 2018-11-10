using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour {	

	[SerializeField] float levelLoadDelay = 2f;
		

	public void LoadNextLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
		{
			nextSceneIndex = 0;
		}		
			SceneManager.LoadScene(nextSceneIndex);	
	}

	public void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}	
}
