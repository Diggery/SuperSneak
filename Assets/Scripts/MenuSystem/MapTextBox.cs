using UnityEngine;
using System.Collections;

public class MapTextBox : MonoBehaviour {
	
	bool textBoxOpen;
	Transform textBoxText;

	string queuedText;
	float queuedDuration;

	float transitionTimer;
	public AnimationCurve transitionCurve;
	float openTimer;
		
	Vector3 startScale = new Vector3(0.1f, 0.25f, 1.0f);
	Vector3 endScale = new Vector3(0.6f, 0.6f, 0.6f);	
	
	public Transform textPos;

	void Start () {
		textBoxText = transform.Find("TextBoxText");
		textBoxText.renderer.material.renderQueue = 4000;
	}
	
	void Update () {

		if (textBoxOpen) {
			transitionTimer = Mathf.Clamp01(transitionTimer + Time.deltaTime * 2);
		} else {
			transitionTimer = Mathf.Clamp01(transitionTimer - Time.deltaTime * 4);
		}
	
		transform.localScale = Vector3.Lerp(startScale, endScale, transitionCurve.Evaluate(transitionTimer));
		Color boxColor = renderer.material.color;
		Color textColor = textBoxText.renderer.material.color;
		boxColor.a = transitionTimer;
		textColor.a = transitionTimer;
		renderer.material.color = boxColor;
		textBoxText.renderer.material.color = textColor;
		
		if (openTimer > 0) {
			openTimer -= Time.deltaTime;
			if (openTimer < 0) CloseTextBox();
		}
	}
	
	public void SetText(string newString, float newDuration) {
		
		if (textBoxOpen) {
			textBoxOpen = false;
			queuedText = newString;
			queuedDuration = newDuration;
			Invoke("ReOpenTextBox", 0.25f);
		} else {
			textBoxOpen = true;
			openTimer = newDuration;
			TextMesh textField = textBoxText.GetComponent<TextMesh>();
			textField.text = newString;			
		}
		
	}
	
	public void CloseTextBox() {
		textBoxOpen = false;
	}
	
	public void ReOpenTextBox() {
		SetText(queuedText, queuedDuration);
	}
	
	
	public bool IsTextBoxOpen() {
		return textBoxOpen;
	}

}
