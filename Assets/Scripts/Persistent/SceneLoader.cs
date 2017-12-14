using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private string lastClosedScene;

	void Start() {
//        LoadScene("Background");
		StartCoroutine( LoadSceneAndSetActive ("MainMenu") );
	}

	public IEnumerator LoadSceneAndSetActive (string sceneName) {
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (newlyLoadedScene);
	}

	public void LoadScene(string sceneName) {
		SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
	}


	public void UnloadScene(string sceneName) {
		if (SceneManager.GetSceneByName (sceneName).isLoaded) {
			SceneManager.UnloadSceneAsync (sceneName);
			lastClosedScene = sceneName;
		}
	}

	public void SetActiveScene(string sceneName) {
		Scene scene = SceneManager.GetSceneByName (sceneName);
		SceneManager.SetActiveScene (scene);
	}

    public string getLastClosedScene() {return lastClosedScene;}
}
