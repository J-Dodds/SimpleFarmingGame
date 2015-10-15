using UnityEngine;
using System.Collections;

public class UI_NewGameButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnPressed() {
		GameController.StartNewGame();
	}
}
