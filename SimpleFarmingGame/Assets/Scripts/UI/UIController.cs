using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	public static UIController Instance {get; private set;}

	public UI_MoneyDisplay moneyDisplay;
	public UI_ControlsPanel controlsPanel;
	public UI_SystemStatusIndicator cropStatusIndicator;
	public UI_SystemStatusIndicator gameStateStatusIndicator;
		
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static void SetSelectedMenuItem(string menuItemId) {
		Instance.controlsPanel.SetSelectedMenuItem(menuItemId);
	}
	
	public static void SetMoney(int money) {
		Instance.moneyDisplay.SetMoney(money);
	}
	
	public static void SetCropStatus(bool isHappy) {
		Instance.cropStatusIndicator.SetStatus(isHappy);
	}
	
	public static void SetGameStateStatus(bool isHappy) {
		Instance.gameStateStatusIndicator.SetStatus(isHappy);
	}
}
