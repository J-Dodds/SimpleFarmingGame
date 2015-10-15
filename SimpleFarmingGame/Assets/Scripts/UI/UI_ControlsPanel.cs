using UnityEngine;
using System.Collections;

public class UI_ControlsPanel : MonoBehaviour {
	public UI_CropsMenuItem clearButton;
	public UI_CropsPanel cropsPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetSelectedMenuItem(string menuItemId) {
		clearButton.Selected = menuItemId == clearButton.MenuItemId;
		cropsPanel.SetSelectedMenuItem(menuItemId);
	}
}
