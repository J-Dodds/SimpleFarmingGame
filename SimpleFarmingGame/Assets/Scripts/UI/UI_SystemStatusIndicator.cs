using UnityEngine;
using System.Collections;

public class UI_SystemStatusIndicator : MonoBehaviour {
	public UnityEngine.UI.Image image;
	public Sprite HappySprite;
	public Sprite SadSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetStatus(bool isHappy) {
		image.sprite = isHappy ? HappySprite : SadSprite;
	}
}
