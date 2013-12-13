using UnityEngine;
using System.Collections;

public class MapLabel : MonoBehaviour {
	
	bool open;
	
	bool action1Active = true;
	Transform actionItem1;
	Transform actionText1;
	
	bool action2Active = true;
	Transform actionItem2;
	Transform actionText2;

	public AnimationCurve actionItem1Curve;
	public AnimationCurve actionItem2Curve;
	
	Transform labelText;

	float timer;
	
	public Color selectedColor;
	public Color unSelectedColor;
	
	public void SetUp (MapDot dot) {
		labelText = transform.Find("Label/LabelText");
		actionItem1 = transform.Find("Action1");
		actionText1 = transform.Find("Action1/ActionText1");
		actionText1.renderer.material.renderQueue = 4000;
		actionText1.gameObject.AddComponent<InputRepeater>().SetTarget(dot.transform);
		actionItem2 = transform.Find("Action2");
		actionText2 = transform.Find("Action2/ActionText2");
		actionText2.renderer.material.renderQueue = 4000;
		actionText2.gameObject.AddComponent<InputRepeater>().SetTarget(dot.transform);
		SetUnSelected();
	}
	
	void Update () {
		
		if (open) {
			timer = Mathf.Clamp01(timer + Time.deltaTime);
		} else {
			timer = Mathf.Clamp01(timer - Time.deltaTime);
		}
		

		if (timer > 0.0f || timer < 1.0f) {
			float amount  = Util.EaseInOutQuart(Mathf.Clamp01(timer));
				
			Vector3 closedScale = new Vector3(0.75f, 0.75f, 0.75f);
			Vector3 openScale = new Vector3(1.0f, 1.0f, 1.0f);
			
			transform.localScale = Vector3.Lerp(closedScale, openScale, amount);
			
			labelText.renderer.material.color = Color.Lerp (unSelectedColor, selectedColor, amount);
			
			if (action1Active) {
				float item1Amount = actionItem1Curve.Evaluate(timer);
				actionItem1.localScale = Vector3.Lerp(closedScale, openScale, item1Amount);
				actionText1.renderer.material.color = new Color(0.0f, 0.0f, 0.0f, item1Amount);
				Util.SetVertColors(actionItem1, Color.Lerp(Color.clear, Color.white, actionItem1Curve.Evaluate(timer)));
				actionText1.renderer.enabled = actionText1.renderer.material.color.a > 0.1f ? true : false;
			}
			if (action2Active) {
				float item2Amount = actionItem2Curve.Evaluate(timer);
				actionItem2.localScale = Vector3.Lerp(closedScale, openScale, item2Amount);
				actionText2.renderer.material.color = new Color(0.0f, 0.0f, 0.0f, item2Amount);
				Util.SetVertColors(actionItem2, Color.Lerp(Color.clear, Color.white, actionItem2Curve.Evaluate(timer)));
				actionText2.renderer.enabled = actionText2.renderer.material.color.a > 0.1f ? true : false;
			}
		}
	}
	
	public void SetLabelText(string newLabelText) {
		labelText.GetComponent<TextMesh>().text = newLabelText;
	}
	
	public void SetAction1(string newActionText) {
		if (newActionText.Equals("Blank")) {
			action1Active = false;
			return;
		}
			
		action1Active = true;
		actionText1.name = newActionText;
		actionText1.GetComponent<TextMesh>().text = newActionText;
	}
	
	public void SetAction2(string newActionText) {
		if (newActionText.Equals("Blank")) {
			action2Active = false;
			return;
		}
		
		action2Active = true;
		actionText2.name = newActionText;
		actionText2.GetComponent<TextMesh>().text = newActionText;
	}

	public void SetSelected() {
		open = true;
		actionText1.GetComponent<BoxCollider>().enabled = true;
		actionText2.GetComponent<BoxCollider>().enabled = true;
	}
	
	public void SetUnSelected() {
		open = false;
		actionText1.GetComponent<BoxCollider>().enabled = false;
		actionText1.name = "Empty";
		actionText2.GetComponent<BoxCollider>().enabled = false;
		actionText2.name = "Empty";
	}
}
