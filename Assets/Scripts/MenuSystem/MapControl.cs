using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapControl : MonoBehaviour {
	
	public int width;
	public int length;
	public int interval = 2;
	public float density;
	float lineRange = 4;
	
	public GameObject dotPrefab;
	public GameObject linePrefab;
	
	List<Transform> points = new List<Transform>();
	
	GameObject[] mapObjects;
	
	GameObject background;
	MapCameraControl cameraControl;
	GameControl gameControl;
	
	MapTextBox mapTextBox;

	Transform portraitThief;
	
	List<string> locationNameList = new List<string>();
	
	bool playersTurn;
	
	
	public bool IsPlayersTurn() {
		return playersTurn;
	}
	
	public void StartPlayersTurn() {
		playersTurn = true;
	}	
	
	void Start () {
		LoadLocationNames();
		background = GameObject.Find ("MapScreenBackground");
		if (!background) Debug.Log("Can't find map background");
		InputRepeater repeater = background.AddComponent<InputRepeater>();
		repeater.SetTarget(transform);
		
		GameObject cameraControlObj = GameObject.Find ("CameraPivot");
		cameraControl = cameraControlObj.GetComponent<MapCameraControl>();
		cameraControl.SetUp(this);
		if (!cameraControl) Debug.Log("Can't find map camera");
		
		GameObject gameControlObj = GameObject.Find ("GameControl");
		gameControl = gameControlObj.GetComponent<GameControl>();
		if (!gameControl) Debug.Log("Can't find GameControl");
		
		Random.seed = gameControl.GetGameMapSeed();
				
		GameObject point = null;
		if (interval < 1) {
			Debug.Log("Inverval too low");
			return;
		}
		int zOffset = 0;
		for (int x = 0; x < width; x += interval) {
			if (zOffset == 0) {
				zOffset = 1;
			} else {
				zOffset = 0;
			}
			for (int z = zOffset; z < length; z += interval) {
				if (Random.value < density) {
					point = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity) as GameObject;
					point.transform.name = "Point-" + x + "-" + z;
					Vector3 randomOffset = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(-0.25f, 0.25f));
					Vector3 centerOffset = new Vector3(-(float) width * 0.5f, 0, -(float)length * 0.5f);
					point.transform.parent = transform;
					point.transform.localPosition = new Vector3(x, 0, z) + randomOffset + centerOffset;
					point.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
					points.Add(point.transform);
				}
			}
		}
		point.renderer.sharedMaterial.renderQueue = 3100;
		CreatePlayerBase();
		CreateEnemyBase();
		ConnectBases();
		SetUpConnections();
		SetSpecialNodes();

		string nextStep = gameControl.currentLevel.Equals("Menu") ? "MapIntro" : "AddResults";

		Invoke (nextStep, 2);
		
		portraitThief = GameObject.Find ("MapScreenThief").transform;
		portraitThief.animation.Play("Intro");
		Invoke ("playPortraitLoop", portraitThief.animation["Intro"].length);
	}
	
	public void playPortraitLoop() {
		portraitThief.animation.Play("Loop");
		PowerPlayerDots();
	}
	
	public void MapIntro() {
		DisplayMessage("Select a location\nto capture", 5);	
		StartPlayersTurn();
	}

	
	public void AddResults() {
		Transform lastDotObj = transform.Find(gameControl.currentLevel);
		if (!lastDotObj) print ("ERROR: no point called " + gameControl.currentLevel );
		
		Camera.main.transform.parent.GetComponent<MapCameraControl>().SetFocus(lastDotObj);
		
		string locName = lastDotObj.GetComponent<MapDot>().GetName();
		
		if (gameControl.currentLevelPassed) {
			DisplayMessage("We captured " + locName, 5);	
			lastDotObj.GetComponent<MapDot>().PlayerCapture();
		} else {
			DisplayMessage("We failed to capture\n" + locName, 3);	
		}
		Invoke("ResultsDone", 4);	
	}
	
	void ResultsDone() {
		GetComponent<MapAI>().StartTurn(this);	
	}
	
	public List<Transform> GetPoints() {
		return points;
	}
	
	void LoadLocationNames() {
		TextAsset locationNamesText = Resources.Load("LocationNames") as TextAsset;
		string[] locationNamesArray = locationNamesText.text.Split();
		locationNameList.Clear();
		foreach(string name in locationNamesArray) {
			locationNameList.Add(name);
		}
	}
	
	public string GetLocationName(){
		int listIndex = Random.Range(0, locationNameList.Count);
		string name = locationNameList[listIndex];
		locationNameList.RemoveAt(listIndex);
		return name;
	}
	public Vector2 GetMapExtents(){
		return new Vector2(width, length);
	}
	
	
	Transform CreatePlayerBase() {
		Transform playerBase = null;
		float bestMatch = 0.0f;
		foreach (Transform point in points) {
			if (point.position.x < bestMatch) {
				bestMatch = point.position.x;
				playerBase = point;
			}
		}
		playerBase.GetComponent<MapDot>().SetAsPlayerBase();
		return playerBase;
	}
	
	Transform CreateEnemyBase() {
		Transform enemyBase = null;
		float bestMatch = 0.0f;
		foreach (Transform point in points) {
			if (point.position.x > bestMatch) {
				bestMatch = point.position.x;
				enemyBase = point;
			}
		}
		enemyBase.GetComponent<MapDot>().SetAsEnemyBase();
		return enemyBase;		
	}
	
	void ConnectBases() {
		int connections = 20;
		Transform lastPoint = GetPlayerBase();
		for (int i = 0; i < connections; i++) {
			Transform newPoint = FindPointToTheRight(lastPoint, lineRange);
			ConnectPoints(lastPoint, newPoint);
			if (newPoint.GetComponent<MapDot>().IsEnemyBase()) break;
			lastPoint = newPoint;
		}
	}
	
	void SetUpConnections() {
		foreach (Transform point in points) {
			if (point.GetComponent<MapDot>().IsEnemyBase()) continue;
			if (point.GetComponent<MapDot>().IsPlayerBase()) continue;
			ConnectPoints(point, FindPointInRange(point, lineRange));
			if (Random.value < 0.5f) ConnectPoints(point, FindPointInRange(point, lineRange));
		}
		GameObject[] lines = GameObject.FindGameObjectsWithTag("MapLine");
		foreach (GameObject line in lines) {
			line.GetComponent<MapLine>().SetOwners();
		}
	}
	
	void SetSpecialNodes() {
		foreach (Transform point in points) {
			MapDot dotControl = point.GetComponent<MapDot>();
			if (dotControl.IsEndPoint()) {
				dotControl.SetAsDepot();
			}
		}
		
	}
	
	Transform GetEnemyBase() {
		Transform enemyBase = null;
		foreach (Transform point in points) {
			if (point.GetComponent<MapDot>().IsEnemyBase()) enemyBase = point;
		}
		
		return enemyBase;		
	}
	
	Transform GetPlayerBase() {
		Transform playerBase = null;
		foreach (Transform point in points) {
			if (point.GetComponent<MapDot>().IsPlayerBase()) playerBase = point;
		}
		
		return playerBase;		
	}	
	
	Transform FindPointInRange(Transform startPoint, float range) {
		
		float closest = Mathf.Infinity;
		int selection = -1;
		
		for (int i = 0; i < points.Count; i++) {
			Transform point = points[i];
			if (point != startPoint) {
				if (!point.GetComponent<MapDot>().IsPlayerBase()) {
					if (!IsConnected(startPoint, point)) {
						float distance = Vector3.Distance(startPoint.position, point.position);
						if (distance < closest) {
							closest = distance;
							selection = i;
						}
					}
				}
			}
		}
		if (selection == -1) {
			if (range > 100) return null;
			return FindPointInRange(startPoint, 101);
		}
		
		return points[selection];
	}
	
	Transform FindPointToTheRight(Transform startPoint, float range) {
		float closest = Mathf.Infinity;
		int selection = -1;
		for (int i = 0; i < points.Count; i++) {
			Transform point = points[i];
			if (point.position.x > startPoint.position.x) {
				if (point != startPoint) {
					if (!point.GetComponent<MapDot>().IsPlayerBase() && !point.GetComponent<MapDot>().IsEnemyBase()) {
						if (!IsConnected(startPoint, point)) {
							float distance = Vector3.Distance(startPoint.position, point.position);
							if (distance < closest) {
								closest = distance;
								selection = i;
							}
						}
					}
				}
			}
		}
		if (selection == -1) {
			if (range > 100) return null;
			return FindPointInRange(startPoint, 101);
		}
		return points[selection];
	}	
	
	void ConnectPoints(Transform point1, Transform point2) {
		if (IsConnected(point1, point2)) return;
		Quaternion lineHeading = Quaternion.AngleAxis(Util.getDirection( point2.position, point1.position), Vector3.up);
		float lineDistance = Vector3.Distance(point1.position, point2.position);
		GameObject line = Instantiate(linePrefab, point1.position, lineHeading) as GameObject;
		line.transform.localScale = new Vector3(0.1f, 0.1f, lineDistance);
		line.transform.parent = transform;
		MapLine newLine = line.GetComponent<MapLine>();
		newLine.SetUp(point1, point2);
		point1.GetComponent<MapDot>().AddConnection(point2, newLine);
		point2.GetComponent<MapDot>().AddConnection(point1, newLine);
	}
	
	bool IsConnected(Transform point1, Transform point2) {
		if (point1.GetComponent<MapDot>().IsConnectedTo(point2)) return true;
		if (point2.GetComponent<MapDot>().IsConnectedTo(point1)) return true;
		return false;
	}

	
	public void LaunchLevel(Transform dotPressed) {

		gameControl.LaunchLevelFromMap(dotPressed);
	}
	
	public void SelectDot(MapDot dot) {
		if (!IsPlayersTurn()) return;
		foreach(Transform point in points) point.SendMessage("UnSelect");
		dot.Select();
	}
	
	public void PowerDots() {
		foreach (Transform point in points) point.GetComponent<MapDot>().PowerDown();
		print ("powering dot");
		PowerPlayerDots();
		PowerEnemyDots();
	}
	
	void PowerPlayerDots() {	
		
		MapDot playerBase = GetPlayerBase().GetComponent<MapDot>();
		playerBase.PowerUp();
		playerBase.TransmitPlayerPower(new List<MapDot>());
	}
	
	void PowerEnemyDots() {		
		MapDot enemyBase = GetEnemyBase().GetComponent<MapDot>();
		enemyBase.PowerUp();
		enemyBase.TransmitEnemyPower(new List<MapDot>());
	}
	
	public void SetTextBox(MapTextBox textBox) {
		mapTextBox = textBox;
	}
	
	public void DisplayMessage(string message, float duration) {
		mapTextBox.SetText(message, duration);
	}
	
	public void tap(TouchManager.TapEvent touchEvent) {
		if (!playersTurn) return;
		foreach(Transform point in points) point.SendMessage("UnSelect");
		if (touchEvent.touchTarget.name.Equals("UpperRight")) {
			gameControl.LoadNewLevel("MainMenu", 1);	
			gameControl.OpenInfoBox("Returning to Menu...", 3, 0.25f);
		}
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		if (!playersTurn) return;
		cameraControl.drag(touchEvent);
	}
	
	public void touchUp(TouchManager.TouchUpEvent touchEvent) {
		if (!playersTurn) return;
		cameraControl.touchUp(touchEvent);
	}
	
	public void InitScoreboard(Transform playerName, Transform playerScore, Transform enemyName, Transform enemyScore) {
		playerName.renderer.material.renderQueue = 4100;
		playerScore.renderer.material.renderQueue = 4100;
		enemyName.renderer.material.renderQueue = 4100;
		enemyScore.renderer.material.renderQueue = 4100;
		
	}
}
