using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {
	
	Transform inventoryPos;
	
	[System.Serializable]
	public class ItemTypes {	
		public InventoryItem.ItemTypes type;
		public string name;
		public Texture texture;
		public GameObject prefab;
	}
	
	public ItemTypes[] itemTypes;
	
	public GameObject itemPrefab;
	public GameObject cashFieldsPrefab;
	
	public int cash;
	public int jewels;
	public UICashDisplay cashDisplay;
	public List<InventoryItem> currentItems;
	
	Transform[] listPositions;
	
	bool inventoryOpen;
	bool lockList;
	
	float scrollOffset = 0.0f;
	public float inventoryTimeOut;
	float inventoryTimer;
	
	bool setUp = false;

	public void SetUp (Transform invPos, UICashDisplay newCashBox) {
		inventoryPos = invPos;
		cashDisplay = newCashBox;
		cashDisplay.setUp(cashFieldsPrefab);

		listPositions = new Transform[5];
		listPositions[0] = invPos.Find ("Slot0");
		listPositions[1] = invPos.Find ("Slot1");
		listPositions[2] = invPos.Find ("Slot2");
		listPositions[3] = invPos.Find ("Slot3");
		listPositions[4] = invPos.Find ("Slot4");
		
		foreach (Transform listPosition in listPositions) listPosition.renderer.enabled = false;

		GameObject player = GameObject.Find ("Player");
		PlayerController playerController = player.GetComponent<PlayerController>();
		playerController.setInventory(this);
		
		
		addItem("Cash");
		addItem("Jewels");
		addItem("Flash");
		addItem("Gas");
		addItem("HighEx");
		addItem("Smoke");
		addItem("Laser");
		
		setUp = true;
		if (currentItems.Count > 0) selectItem(currentItems[0]);
		
	}
	
	void Update () {
		if (!setUp) return;
		
		if (!lockList) {
			positionList();
			positionCash();
		}
		if (inventoryOpen) {
			inventoryTimer -= Time.deltaTime;
			if (inventoryTimer < 0) closeInventory();
		}
		
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
	void positionCash() {
		Vector3 cashPos = cashDisplay.transform.localPosition;
		if (inventoryOpen) {
			cashPos.x = Mathf.Lerp(cashPos.x, 0.0f, Time.deltaTime * 5);
		} else {
			cashPos.x = Mathf.Lerp(cashPos.x, -0.2f, Time.deltaTime * 5);
		}
		cashDisplay.transform.localPosition = cashPos;
		
	}
	
	public void toggleInventory(InventoryItem selectedItem) {
		if (inventoryOpen) {
			selectItem(selectedItem);
		} else {
			openInventory();
		}
	}	
	
	public void openInventory() {
		inventoryOpen = true;
		scrollOffset = 1;
		inventoryTimer = inventoryTimeOut;
	}
	
	public void closeInventory() {
		inventoryOpen = false;
		scrollOffset = 0.0f;
	}	
	
	public void selectItem(InventoryItem selectedItem) {
		foreach(InventoryItem item in currentItems) item.setUnselected();
		selectedItem.setSelected();
		moveItemToSelected(selectedItem);
		closeInventory();
	}
	
	public void addItem(string newItemType) {
		openInventory();
		
		if (newItemType.Equals("Cash")) {
			addCash(100);
			return;
		}
		if (newItemType.Equals("Jewel")) {
			addJewels(1);
			return;
		}
			
		//find if type is already in list
		foreach (InventoryItem item in currentItems) {
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
				InventoryItem newInventoryItem = newItem.GetComponent<InventoryItem>();
				newInventoryItem.setUp(this);
				newInventoryItem.setType(itemType.type);
				newInventoryItem.setName(itemType.name);
				newInventoryItem.setTexture(itemType.texture);
				newInventoryItem.setPrefab(itemType.prefab);
				newInventoryItem.addCount();
				currentItems.Insert(0, newInventoryItem);
				break;
			}
		}
	}
	
	public GameObject getItemPrefab(string name) {
		foreach (ItemTypes itemType in itemTypes) {
			if (itemType.name.Equals(name)) return itemType.prefab;
		}
		return null;
	}
	
	public void useItem(InventoryItem usedItem) {
		
		if (usedItem.getCount() <= 1) {
			currentItems.Remove(usedItem);
			usedItem.goAway();
			lockList = true;
		} else {
			usedItem.removeCount();
		}
	}
	
	public InventoryItem.ItemTypes GetSelectedItemType() {
		return currentItems[0].itemType;
	}
	
	public GameObject getBomb() {
		if (currentItems.Count < 1) return null;
		GameObject item = currentItems[0].getPrefab();
		useItem(currentItems[0]);
		return item;
	}
	
	public InventoryItem GetItemProperties() {
		if (currentItems.Count < 1) return null;
		return currentItems[0];
	}
	
	public void moveItemToSelected(InventoryItem selectedItem) {
		currentItems.Remove(selectedItem);
		currentItems.Insert(0, selectedItem);
		
	}	
	public void scrollList(Vector2 amount) {
		if (!inventoryOpen) return;
		inventoryTimer = inventoryTimeOut;
		scrollOffset = Mathf.Clamp(scrollOffset + (0.01f * amount.y), (2.5f - currentItems.Count), 2.5f);
	}
	
	public void unlockList() {
		lockList = false;
	}
	
	public Transform getInventoryPos() {
		return getInventoryPos("Other");
	}
	
	public Transform getInventoryPos(string itemType) {
		Transform endPos;
		
		switch (itemType) {
			
		case "Cash" :
			endPos = cashDisplay.transform;
			break;
		case "Jewel" :
			endPos = cashDisplay.transform;
			break;
		default :
			endPos = inventoryPos;
			break;
		}
		
		
		return endPos;
	}
	
	public void addCash(int newCash) {
		cash += newCash;
		cashDisplay.setCashAmount(cash);
	}
	
	public void addJewels(int newJewels) {
		jewels += newJewels;
		cashDisplay.setJewelAmount(jewels);
	}
}
