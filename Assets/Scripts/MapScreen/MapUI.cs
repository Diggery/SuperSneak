using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour {
	
	Transform upperLeft;
	Transform lowerLeft;
	Transform upperRight;
	Transform lowerRight;

	Transform playerName;
	Transform playerScore;

	Transform enemyName;
	Transform enemyScore;
	
	MapControl mapControl;

	void Start () {
		
		GameObject mapControlObj = GameObject.Find("MapControl");
		mapControl = mapControlObj.GetComponent<MapControl>();	
		
		Transform textBox = transform.Find("UpperLeft/TextBox");
		mapControl.SetTextBox(textBox.GetComponent<MapTextBox>());


		upperLeft = transform.Find("UpperLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperLeft);
		
		upperRight = transform.Find("UpperRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(upperRight);
		upperRight.gameObject.AddComponent<InputRepeater>().SetTarget(mapControl.transform);
		
		lowerLeft = transform.Find("LowerLeft");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerLeft);
		Transform playerName = lowerLeft.Find("PlayerName");
		Transform playerScore = lowerLeft.Find("PlayerScore");

		lowerRight = transform.Find("LowerRight");
		Camera.main.transform.GetComponent<UIManager>().lockToEdge(lowerRight);
		Transform enemyName = lowerRight.Find("EnemyName");
		Transform enemyScore = lowerRight.Find("EnemyScore");
		
		mapControl.InitScoreboard(playerName, playerScore, enemyName, enemyScore);
	}
}
