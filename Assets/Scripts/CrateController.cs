using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrateController : MonoBehaviour {
	
	public List<string> contents;
	bool opening;
	bool opened;
	Transform midPoint;
	PlayerController playerController;
	InventoryController inventory;
	public AnimationCurve transCurve;

	public GameObject itemPrefab;
	
	Transform miniMapDot;
	Transform miniMapDotOpened;

	void Start () {

		midPoint = transform.Find ("Box/MidPoint");
		
		//if crate is empty, add a random amount of cash and jewels
		if (contents.Count < 1) {
			for (int i = 0; i < Random.Range(3,10); i++) {
				if (Random.value < 0.5f) {
					contents.Add("Cash");
				} else {
					contents.Add("Jewel");
				}
			}
		}
		
		// switch the chests minimap dot
		miniMapDot = transform.Find("MiniMap");
		miniMapDotOpened = transform.Find("MiniMapOpened");

	}
	
	void GetInventory() {
		Transform inventoryObj = Camera.main.transform.Find("UI");
		inventory = inventoryObj.GetComponent<InventoryController>();
		if (!inventory) Debug.Log("ERROR: crate cant find inventory");		
	}
	
	public void AddItem(string itemType) {
		contents.Add(itemType);	
	}
	
	IEnumerator openCrate() {
		if (!inventory) GetInventory();
		
		animation.Play("Open");
		opened = true;
		
		yield return new WaitForSeconds(animation["Open"].length);
		
		for (int i = 0; i < contents.Count; i++) {
			GameObject itemObj = inventory.getItemPrefab(contents[i]);
			if (!itemObj) Debug.Log(contents[i] + " is not a item");
			GameObject addeditem = Instantiate(itemObj, transform.position, Quaternion.identity) as GameObject;
			addeditem.transform.name = contents[i];
			Transform itemEndPoint = inventory.getInventoryPos(contents[i]);
			addeditem.AddComponent<AddInventoryItem>().setUp(transform, midPoint, itemEndPoint, playerController, transCurve);
			yield return new WaitForSeconds(0.1f);
		}
		miniMapDot.renderer.enabled = false;
		miniMapDotOpened.renderer.enabled = true;
	}
	
    void OnTriggerEnter(Collider other) {
		if (!opened && other.transform.tag.Equals("Player")) {
			playerController = other.transform.GetComponent<PlayerController>();
			if (!playerController) Debug.Log("ERROR: crate cant find player");
			playerController.openCrate(transform.position);
			StartCoroutine(openCrate());
		}
	}	
}
