using UnityEngine;
using System.Collections;

public class UI_MoneyDisplay : MonoBehaviour {
	public UnityEngine.UI.Text text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetMoney(int money) {
		text.text = "$" + money;
	}
}
