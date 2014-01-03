using UnityEngine;
using System.Collections;

public class InventoryItem : MonoBehaviour {
		
	public enum ItemTypes { None, Thrown, Projectile} 
	public ItemTypes itemType = ItemTypes.Thrown;
	
	Transform itemGraphic;
	Transform itemCount;
	Transform itemName;
	Transform nameField;
	Transform nameFieldActive;
	Transform background;
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
		nameField = transform.Find ("NameField");
		nameFieldActive = transform.Find ("NameFieldActive");
		background = transform.Find ("Background");
		itemCount.renderer.material.renderQueue = 4000;
		itemName.renderer.material.renderQueue = 4000;
		inventory = newInventory;
		height = GetComponent<BoxCollider>().size.y;
		setUnselected();
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

	public void setSelected() {
		nameField.renderer.enabled = false;	
		nameFieldActive.renderer.enabled = true;
		Util.SetVertColors(itemGraphic, Color.white);
		Util.SetVertColors(background, new Color(1.0f, 1.0f, 1.0f, 1.0f));
		itemGraphic.localScale = Vector3.one;
		itemName.renderer.material.color = Color.white;
	}
	public void setUnselected() {
		nameField.renderer.enabled = true;	
		nameFieldActive.renderer.enabled = false;			
		Util.SetVertColors(itemGraphic, Color.gray);
		Util.SetVertColors(background, new Color(1.0f, 1.0f, 1.0f, 0.5f));
		itemGraphic.localScale = new Vector3(0.75f, 0.75f, 0.75f);
		itemName.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
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
