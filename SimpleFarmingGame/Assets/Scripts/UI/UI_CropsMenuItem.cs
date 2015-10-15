using UnityEngine;
using System.Collections;

public enum UIAction {
	ClearCrop,
	PlantCrop
}

public class UI_CropsMenuItem : MonoBehaviour {
	public UIAction action = UIAction.ClearCrop;
	public string MenuItemId;
	public string MenuItemText;
	
	private CanvasRenderer canvasRenderer;
	private UnityEngine.UI.Text menuItemText;
	
	private bool isSelected = false;

	// Use this for initialization
	void Start () {
		// Retrieve the canvas and make the button disabled to begin
		canvasRenderer = gameObject.GetComponent<CanvasRenderer>();
		canvasRenderer.SetAlpha(0.25f);
		
		// Retrieve the menu item's text
		menuItemText = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();
		menuItemText.text = MenuItemText;
		
		// Override the menu item id for the clear crop
		if (action == UIAction.ClearCrop) {
			MenuItemId = UIAction.ClearCrop.ToString();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnPressed() {
		GameController.SelectedCropMenuItem(MenuItemId);
	}
	
	public bool Selected {
		get {
			return isSelected;
		}
		set {
			canvasRenderer.SetAlpha(value ? 1.0f : 0.25f);
			isSelected = value;
		}
	}
}
