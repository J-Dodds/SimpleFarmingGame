using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct CropMenuItemInfo {
	public string UniqueId;
	public string Name;
	public int Cost;
}

public class UI_CropsPanel : MonoBehaviour {
	private List<CropMenuItemInfo> menuItemInfos;
	private List<UI_CropsMenuItem> menuItems = new List<UI_CropsMenuItem>();
	
	public GameObject MenuItemPrefab;
	public int LeftMargin = 5;
	public int BottomMargin = 5;
	public int TopMargin = 5;
	
	// Use this for initialization
	void Start () {
		// Retrive the menu item infos
		menuItemInfos = GameController.Instance.GetCropMenuItemInfos();
		
		// Spawn all of the menu items
		RectTransform menuItemRect = MenuItemPrefab.GetComponent<RectTransform>();
		float currentX = (2 * LeftMargin) + menuItemRect.rect.width;
		foreach(CropMenuItemInfo itemInfo in menuItemInfos) {
			// Spawn the menu item
			GameObject menuItem = GameObject.Instantiate(MenuItemPrefab) as GameObject;
			
			// Set the parent to us
			menuItem.transform.SetParent(this.gameObject.transform);
			menuItem.transform.position = new Vector3(currentX, TopMargin, 0);
			
			// Retrieve the crop UI component
			UI_CropsMenuItem cropUI = menuItem.GetComponent<UI_CropsMenuItem>();
			if (cropUI) {
				cropUI.MenuItemId = itemInfo.UniqueId;
				cropUI.MenuItemText = itemInfo.Name + " ($" + itemInfo.Cost + ")";
				menuItems.Add(cropUI);
			}
			else {
				Debug.LogError("The Crop Menu Item prefab does not have a UI_CropsMenuItem script attached. It will not function.");
			}
			
			// Update the X position
			currentX += menuItemRect.rect.width + LeftMargin;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetSelectedMenuItem(string menuItemId) {
		// Update the selected state of all of the menu items
		foreach(UI_CropsMenuItem menuItem in menuItems) {
			menuItem.Selected = menuItem.MenuItemId == menuItemId;
		}
	}
}
