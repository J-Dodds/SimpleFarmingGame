using UnityEngine;
using System.Collections;

public class CropController : MonoBehaviour {
	public GameObject CropMesh;
	private bool deathApplied = false;
	private bool matureApplied = false;
	
	// Use this for initialization
	void Start () {
		SetCropState(0.0f, false, false);	
		SetMaterialColour(Color.gray);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetCropState(float maturityPercentage, bool isMature, bool isDead) {
		// Scale the crop based on it's maturity percentage
		if (isMature) {
			gameObject.transform.localScale = Vector3.one;
			
			// has the mature tint been applied?
			if (!matureApplied) {
				matureApplied = true;
				SetMaterialColour(Color.white);
			}
		}
		else {
			gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, Mathf.Lerp(0.5f, 0.9f, maturityPercentage));
		}
		
		// Is the crop dead?
		if (isDead && !deathApplied) {
			deathApplied = true;

			// Change all of the materials to black
			SetMaterialColour(Color.black);	
 		}
	}
	
	private void SetMaterialColour(Color colour) {
		MeshRenderer mr = CropMesh.GetComponent<MeshRenderer>();
		foreach(Material material in mr.materials) {
			material.color = colour;
		}
	}
}
