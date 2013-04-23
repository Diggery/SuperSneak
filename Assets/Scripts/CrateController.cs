using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrateController : MonoBehaviour {
	
	public List<string> contents;
	bool opening;
	bool opened;
	Transform midPoint;
	Transform endPoint;
	PlayerController playerController;
	UIInventory inventory;
	public AnimationCurve transCurve;

	
	public GameObject bombPrefab;

	void Start () {
		Transform inventoryObj = Camera.main.transform.Find("UI");
		inventory = inventoryObj.GetComponent<UIInventory>();
		if (!inventory) Debug.Log("ERROR: crate cant find inventory");
		midPoint = transform.Find ("Box/MidPoint");
		endPoint = inventory.getInventoryPos();
	}
	
	void Update () {
	
	}
	
	void oxpenCrate() {
		animation.Play("Open");
		Invoke("crateOpened", animation["Open"].length);
		opened = true;
	}
	
	IEnumerator openCrate() {
		animation.Play("Open");
		opened = true;
		
		yield return new WaitForSeconds(animation["Open"].length);
		
		for (int i = 0; i < contents.Count; i++) {
			GameObject addedBomb = Instantiate(inventory.getItemPrefab(contents[i]), transform.position, Quaternion.identity) as GameObject;
			addedBomb.transform.name = contents[i];
			addedBomb.AddComponent<AddInventoryItem>().setUp(transform, midPoint, endPoint, playerController, transCurve);
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
