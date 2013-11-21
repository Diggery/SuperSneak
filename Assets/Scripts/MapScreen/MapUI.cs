using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour {
	
	Transform upperLeft;
	Transform lowerLeft;
	Transform upperRight;
	Transform lowerRight;
	
	MapControl mapControl;

	void Start () {
		
		GameObject mapControlObj = GameObject.Find("MapControl");
		mapControl = mapControlObj.GetComponent<MapControl>();	
		
		Transform textBox = transform.Find("UpperLeft/TextBox");
		mapControl.SetTextBox(textBox.GetComponent<MapTextBox>());


		upperLeft = transform.Find("UpperLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperLeft);

		lowerLeft = transform.Find("LowerLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerLeft);

		upperRight = transform.Find("UpperRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperRight);
		
		lowerRight = transform.Find("LowerRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerRight);
		
	}
	
	void Update () {
	
	}
}
