using UnityEngine;
using System.Collections;

public class DialogControl : MonoBehaviour {
	
	
	bool opened;
	float timer;
	float delay;
	BoxCollider collision;
	Transform dialogBox;
	Transform okButton;
	TextMesh dialogText;
	bool tapToDismiss;
	float scaleOffset;
	
	public Vector3 startScale;
	public Vector3 endScale;

	public void Init () {
		transform.localPosition = new Vector3(0,0,1);
		transform.localRotation = Quaternion.identity;
		collision = GetComponent<BoxCollider>();
		dialogBox = transform.Find("DialogBox");
		dialogBox.renderer.material.renderQueue = 4100;
		okButton = dialogBox.Find("OKButton");
		okButton.renderer.material.renderQueue = 4102;
		dialogText = transform.Find("DialogText").GetComponent<TextMesh>();
		dialogText.renderer.material.renderQueue = 4101;
		CloseBox();
	}
	
	void Update () {
		if (timer > 0.0f) {
			if (opened) {
				timer += Time.deltaTime;	
			} else {
				timer -= Time.deltaTime;	
			}
			
			float amount = Util.EaseInOutQuart(Mathf.Clamp01(timer * 2));
			transform.localScale = Vector3.Lerp (startScale, endScale, amount) * scaleOffset;
			dialogBox.renderer.material.color = Color.Lerp (new Color(1,1,1,0), Color.white, amount);
			dialogText.renderer.material.color = Color.Lerp (Color.clear, Color.black, amount);
			
			if (timer < 0.0f) CloseBox();
				
			if (timer > delay) opened = false;
		}

	}

	public void tap(TouchManager.TapEvent touchEvent) {
		if (!touchEvent.touchTarget.name.Equals("OKButton")) return;
		if (tapToDismiss && timer > 0.5f) {
			opened = false;
			timer = Mathf.Clamp(timer, 0.0f, 0.5f);
		}
	}
	
	void CloseBox() {
		transform.localScale = startScale;
		dialogBox.renderer.material.color = Color.clear;
		dialogText.renderer.material.color = Color.clear;
		collision.enabled = false;
		dialogBox.renderer.enabled = false;
		dialogText.renderer.enabled = false;		
	}
	
	public void SetText(string newText, float newDelay, float newScale, bool newTapToDismiss) {
		scaleOffset = newScale;
		collision.enabled = true;
		dialogBox.renderer.enabled = true;
		dialogText.renderer.enabled = true;
		opened = true;
		timer = 0.01f;
		dialogText.text = newText;
		delay = newDelay;
		if (newTapToDismiss) { 		
			tapToDismiss = true;
			okButton.renderer.enabled = true;
			okButton.GetComponent<BoxCollider>().enabled = true;
		} else {
			tapToDismiss = false;
			okButton.renderer.enabled = false;
			okButton.GetComponent<BoxCollider>().enabled = false;			
		}
	}
}
