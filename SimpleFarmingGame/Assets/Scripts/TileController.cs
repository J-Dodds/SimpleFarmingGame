using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {
	private GameObject currentCrop;
	private CropController cropController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CropPlanted(GameObject cropPrefab) {
		// Spawn the crop and set it to have the tile as it's parent
		currentCrop = GameObject.Instantiate(cropPrefab);
		currentCrop.transform.SetParent(gameObject.transform);
		
		// Set the location of the crop
		currentCrop.transform.localPosition = Vector3.zero;
		
		// Cache the crop controller
		cropController = currentCrop.GetComponent<CropController>();
	}
	
	public void RemoveCrop() {
		GameObject.Destroy(currentCrop);
		
		currentCrop = null;
		cropController = null;
	}
	
	public void SetCropState(float maturityPercentage, bool isMature, bool isDead) {
		// Pass the state through to the crop
		if (cropController) {
			cropController.SetCropState(maturityPercentage, isMature, isDead);
		}		
	}
}
