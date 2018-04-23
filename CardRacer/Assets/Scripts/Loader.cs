using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadGame(){
		SceneManager.LoadScene ("_GAMESCENE_");
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void LoadMenu() {
		SceneManager.LoadScene ("_MainMenu_");
	}
}
