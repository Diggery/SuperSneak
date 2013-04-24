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

	void Start () {
		Transform inventoryObj = Camera.main.transform.Find("UI");
		inventory = inventoryObj.GetComponent<InventoryController>();
		if (!inventory) Debug.Log("ERROR: crate cant find inventory");
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
	}
	
	void Update () {
	
	}
	
	IEnumerator openCrate() {
		animation.Play("Open");
		opened = true;
		
		yield return new WaitForSeconds(animation["Open"].length);
		
		for (int i = 0; i < contents.Count; i++) {
			GameObject addeditem = Instantiate(inventory.getItemPrefab(contents[i]), transform.position, Quaternion.identity) as GameObject;
			addeditem.transform.name = contents[i];
			Transform itemEndPoint = inventory.getInventoryPos(contents[i]);
			addeditem.AddComponent<AddInventoryItem>().setUp(transform, midPoint, itemEndPoint, playerController, transCurve);
			yield return new WaitForSeconds(0.1f);
		}
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
