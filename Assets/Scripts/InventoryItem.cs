using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {
	
	//public string type;
	
	public enum ItemTypes { None, Thrown, Projectile} 
	public ItemTypes itemType = ItemTypes.Thrown;
	
	Transform itemGraphic;
	Transform itemCount;
	Transform itemName;
	GameObject bombPrefab;
	int currentCount;
	float height;
	Vector3 posGoal;
	Vector3 scaleGoal;
	bool dying;
	
	
	InventoryController inventory;
	
	public void setUp(InventoryController newInventory) {
		itemGraphic = transform.Find ("ItemGraphic");
		itemCount = transform.Find ("NameField/ItemCount");
		itemName = transform.Find ("NameField/ItemCount/ItemName");
		itemCount.renderer.material.renderQueue = 4000;
		itemName.renderer.material.renderQueue = 4000;
		inventory = newInventory;
		height = GetComponent<BoxCollider>().size.y;
	}
	
	void Update() {
		transform.localPosition = Vector3.Lerp (transform.localPosition, posGoal, Time.deltaTime * 10);
		transform.localScale = Vector3.Lerp (transform.localScale, scaleGoal, Time.deltaTime * 10);
		if (dying && transform.localScale.x < 0.1) Destroy (gameObject);
	}

	public float getHeight() {
		return height;
	}

	public void setPosGoal(Vector3 newPos) {
		posGoal = newPos;
	}		
	public Vector3 getPosGoal() {
		return posGoal;
	}		

	public void setScaleGoal(Vector3 newScale) {
		scaleGoal = newScale;
	}
	
	public void setType(ItemTypes newType) {
		itemType = newType;
	}
	
	public ItemTypes getType() {
		return itemType;
	}
		
	public void setName(string newName) {
		itemName.GetComponent<TextMesh>().text = newName;
	}

	public string getName() {
		return itemName.GetComponent<TextMesh>().text;
	}
	
	public void setTexture(Texture newTexture) {
		itemGraphic.renderer.material.mainTexture = newTexture;
	}

	public void addCount() {
		currentCount++;
		itemCount.GetComponent<TextMesh>().text = currentCount.ToString();
	}
	public void removeCount() {
		currentCount--;
		itemCount.GetComponent<TextMesh>().text = currentCount.ToString();
	}
	public int getCount() {
		return currentCount;
	}	
	public void setPrefab(GameObject prefab) {
		bombPrefab = prefab;
	}
	public GameObject getPrefab() {
		return bombPrefab;
	}
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		inventory.scrollList(touchEvent.touchDelta);
	}
	
	public void tap(TouchManager.TapEvent touchEvent) {
		inventory.toggleInventory(this);
	}
	
	public void goAway() {
		scaleGoal = Vector3.zero;
		dying = true;
	}
}
