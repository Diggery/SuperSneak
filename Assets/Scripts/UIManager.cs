using UnityEngine;
using System.Collections;


public class UIManager : MonoBehaviour {

	public float UIScale;
	public TextMesh debugText;
	public string debugString;
	
	
	void Start () {
	
	}
	
	void Update () {
		debugText.text = debugString;
	
	}
	
	public void lockToEdge(Transform newElement) {
		
		//find the new pos
		
		float screenX;
		float screenY;
			
		switch(newElement.name) {
			case "LowerLeft" :
				screenX = 0.0f;
				screenY = 0.0f;
				break;
			case "LowerRight" :
				screenX = Screen.width;
				screenY = 0.0f;
				break;
			case "LowerCenter" :
				screenX = Screen.width * 0.5f;
				screenY = 0.0f;
				break;
			case "UpperRight" :
				screenX = Screen.width;
				screenY = Screen.height;
				break;
			case "UpperCenter" :
				screenX = Screen.width * 0.5f;
				screenY = Screen.height;
				break;
			case "UpperLeft" :
				screenX = 0.0f;
				screenY = Screen.height;
				break;
			case "Center" :
				screenX = Screen.width * 0.5f;
				screenY = Screen.height * 0.5f;
				break;
			default :
				screenX = Screen.width * 0.5f;
				screenY = Screen.width * 0.5f;
				break;
				
		}
		
		//move element into position
		newElement.position = Camera.main.ScreenToWorldPoint(new Vector3 (screenX, screenY, Camera.main.nearClipPlane + 1));
		newElement.rotation = Camera.main.transform.rotation;
		newElement.Rotate(0.0f, 180.0f, 0.0f);
		newElement.localScale = new Vector3(UIScale, UIScale, UIScale);
	}
}