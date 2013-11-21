using UnityEngine;
using System.Collections;

public class MapLabel : MonoBehaviour {
	
	Transform lightSpinner;
	
	Transform actionText1;
	Transform actionText2;
	
	Transform labelText;
	
	float labelScalePrev;
	float labelScaleNext;	
	
	Transform labelCircle;
	float circleScalePrev;
	float circleScaleNext;
	float circleFadePrev;
	float circleFadeNext;
	
	float timer;
	
	public Color selectedColor;
	public Color unSelectedColor;
	
	Color textColorPrev;
	Color textColorNext;
	
	public void SetUp (MapDot dot) {
		lightSpinner = transform.Find("LightOrbit");
		labelText = transform.Find("LabelText");
		//labelText.renderer.material.renderQueue = 4000;
		labelCircle = transform.Find("Circle");
		actionText1 = transform.Find("Circle/ActionText1");
		actionText1.gameObject.AddComponent<InputRepeater>().SetTarget(dot.transform);
		actionText2 = transform.Find("Circle/ActionText2");
		actionText2.gameObject.AddComponent<InputRepeater>().SetTarget(dot.transform);
		circleScalePrev = labelCircle.localScale.x;
		circleFadePrev = 1.0f;
		SetUnSelected();
	}
	
	void Update () {
		lightSpinner.Rotate(0, 90 * GameTime.deltaTime, 0);
		
		if (timer < 1.0f) {
			timer += Time.deltaTime;
			float amount  = Util.EaseInOutQuart(Mathf.Clamp01(timer));

			
			Vector3 prevLabelScale = new Vector3(labelScalePrev, labelScalePrev, labelScalePrev);
			Vector3 nextLabelScale = new Vector3(labelScaleNext, labelScaleNext, labelScaleNext);
			transform.localScale = Vector3.Lerp(prevLabelScale, nextLabelScale, amount);
			
			Vector3 prevCircleScale = new Vector3(circleScalePrev, circleScalePrev, circleScalePrev);
			Vector3 nextCircleScale = new Vector3(circleScaleNext, circleScaleNext, circleScaleNext);
			labelCircle.localScale = Vector3.Lerp(prevCircleScale, nextCircleScale, amount);
			
			float circleFade = Mathf.Lerp(circleFadePrev, circleFadeNext, amount);
			Util.SetVertColors(labelCircle, new Color(1.0f, 1.0f, 1.0f, circleFade));
			
			labelText.renderer.material.color = Color.Lerp (textColorPrev, textColorNext, amount);
			
			Color actionTextColor = new Color(1.0f, 1.0f, 1.0f, circleFade);
			actionText1.renderer.material.color = actionTextColor;
			actionText2.renderer.material.color = actionTextColor;

			if (actionText1.renderer.material.color.a < 0.1f) {
				actionText1.renderer.enabled = false;
				actionText2.renderer.enabled = false;
			} else {
				actionText1.renderer.enabled = true;
				actionText2.renderer.enabled = true;
			}


			
			if (timer > 1) {
				labelScalePrev = labelScaleNext;
				textColorPrev = textColorNext;
				circleScalePrev = circleScaleNext;
				circleFadePrev = circleFadeNext;
			}
		}
	}
	
	public Transform GetLightSpinner () {
		if (!lightSpinner) lightSpinner = transform.Find("LightOrbit");
		return lightSpinner;
	}	
	
	public void SetLabelText(string newLabelText) {
		labelText.GetComponent<TextMesh>().text = newLabelText;
	}
	
	public void SetActionText1(string newActionText) {
		actionText1.GetComponent<TextMesh>().text = newActionText;
	}
	
	public void SetActionText2(string newActionText) {
		actionText2.GetComponent<TextMesh>().text = newActionText;
	}
	
	public void SetSelected() {
		SetUpTransition(1.0f, 4.0f, 1.0f, selectedColor);
		actionText1.GetComponent<BoxCollider>().enabled = true;
		actionText2.GetComponent<BoxCollider>().enabled = true;
	}
	
	public void SetUnSelected() {
		SetUpTransition(0.75f, 1.0f, 0.0f, unSelectedColor);
		actionText1.GetComponent<BoxCollider>().enabled = false;
		actionText2.GetComponent<BoxCollider>().enabled = false;
	}
	
	public void SetUpTransition(float newLabelScale, float newCircleScale, float newFade, Color newTextColor) {
		if (timer < 1) {
			labelScalePrev = transform.localScale.x;
			circleScalePrev = labelCircle.localScale.x;
			circleFadePrev = Mathf.Lerp(circleFadePrev, circleFadeNext, timer);
		}
		labelScaleNext = newLabelScale;
		textColorNext = newTextColor;
		circleScaleNext = newCircleScale;
		circleFadeNext = newFade;
		timer = 0.0f;
	}

}
