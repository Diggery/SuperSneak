using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour {
	
	Transform inventoryPos;
	
	[System.Serializable]
	public class ItemTypes {	
		public string name;
		public Texture texture;
		public GameObject prefab;
	}
	
	public ItemTypes[] itemTypes;
	
	public GameObject itemPrefab;
	
	public List<UIInventoryItem> currentItems;
	
	Transform[] listPositions;
	
	bool inventoryOpen;
	bool lockList;
	
	float scrollOffset = 0.0f;

	public void setUp (Transform invPos) {
		inventoryPos = invPos;
		listPositions = new Transform[5];
		listPositions[0] = invPos.Find ("Slot0");
		listPositions[1] = invPos.Find ("Slot1");
		listPositions[2] = invPos.Find ("Slot2");
		listPositions[3] = invPos.Find ("Slot3");
		listPositions[4] = invPos.Find ("Slot4");
		
		foreach (Transform listPosition in listPositions) listPosition.renderer.enabled = false;
		
		addItem("HighEX");
		addItem("HighEX");
		addItem("HighEX");
		addItem("Gas");
		addItem("Flash");
		addItem("Laser");
		addItem("Smoke");

		GameObject player = GameObject.Find ("Player");
		PlayerController playerController = player.GetComponent<PlayerController>();
		playerController.setInventory(this);
	}
	
	void Update () {
		if (!lockList) positionList();
	}
	
	void positionList() {
		for (int i = 0; i < currentItems.Count; i++) {
			float scrollAmount = scrollOffset + i;
			int fromIndex = Mathf.FloorToInt(scrollAmount);
			int toIndex = Mathf.CeilToInt(scrollAmount);
			float amount = scrollAmount - fromIndex;
			Vector3 itemPos = Vector3.Lerp(listPositions[Mathf.Clamp(fromIndex, 0, 4)].localPosition, listPositions[Mathf.Clamp(toIndex, 0, 4)].localPosition, amount);
			Vector3 itemScale = Vector3.Lerp(listPositions[Mathf.Clamp(fromIndex, 0, 4)].localScale, listPositions[Mathf.Clamp(toIndex, 0, 4)].localScale, amount);
			
			if (!inventoryOpen) {
				if (i == 0) {
					itemPos = Vector3.zero;
					itemScale = Vector3.one;
				} else {
					itemPos.x = -0.15f;
				}
			}

			currentItems[i].setPosGoal(itemPos);
			currentItems[i].setScaleGoal(itemScale);
		}
	}
	
	public void toggleInventory(UIInventoryItem selectedItem) {
		if (inventoryOpen) {
			selectItem(selectedItem);
		} else {
			openInventory();
		}
	}	
	
	public void openInventory() {
		inventoryOpen = true;
		scrollOffset = 1;
	}
	
	public void closeInventory() {
		inventoryOpen = false;
		scrollOffset = 0.0f;
	}	
	
	public void selectItem(UIInventoryItem selectedItem) {
		moveItemToSelected(selectedItem);
		closeInventory();
	}
	
	public void addItem(string newItemType) {
		//find if type is already in list
		foreach (UIInventoryItem item in currentItems) {
			if (item.getName().Equals(newItemType)) {
				item.addCount();
				return;
			}
		}
		
		// no item found, create a new entry
		foreach (ItemTypes itemType in itemTypes) {
			if (itemType.name.Equals(newItemType)) {
				GameObject newItem = Instantiate(itemPrefab, inventoryPos.position, inventoryPos.rotation) as GameObject;	
				newItem.transform.parent = inventoryPos;
				newItem.transform.localScale = Vector3.one;
				UIInventoryItem newInventoryItem = newItem.GetComponent<UIInventoryItem>();
				newInventoryItem.setUp(this);
				newInventoryItem.setName(itemType.name);
				newInventoryItem.setTexture(itemType.texture);
				newInventoryItem.setPrefab(itemType.prefab);
				newInventoryItem.addCount();
				currentItems.Insert(0, newInventoryItem);
				break;
			}
		}
	}
	
	public void useItem(UIInventoryItem usedItem) {
		
		if (usedItem.getCount() <= 1) {
			currentItems.Remove(usedItem);
			usedItem.goAway();
			lockList = true;
		} else {
			usedItem.removeCount();
		}
	}
	
	public GameObject getItem() {
		if (currentItems.Count < 1) return null;
		GameObject bomb = currentItems[0].getPrefab();
		useItem(currentItems[0]);
		return bomb;
	}
	
	public void moveItemToSelected(UIInventoryItem selectedItem) {
		currentItems.Remove(selectedItem);
		currentItems.Insert(0, selectedItem);
		
	}	
	public void scrollList(Vector2 amount) {
		if (!inventoryOpen) return;
		scrollOffset = Mathf.Clamp(scrollOffset + (0.01f * amount.y), (2.5f - currentItems.Count), 2.5f);
	}
	
	public void unlockList() {
		lockList = false;
	}
}
