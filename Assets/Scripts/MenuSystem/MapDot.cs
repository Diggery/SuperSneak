using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapDot : MonoBehaviour {

	public enum DotStatus { None, PlayerPowered, PlayerUnpowered, EnemyPowered, EnemyUnpowered, Depot }
	DotStatus currentStatus = DotStatus.None;

	List<Transform> connectedPoints = new List<Transform>();
	List<MapLine> connectedLines = new List<MapLine>();
	
	bool isEnemyBase;
	bool isPlayerBase;
	
	string locName;
	
	public Texture baseDotTexture;
	
	public MapLabel labelPrefab;
	MapLabel label;
	
	public Color playerColor;
	public Color enemyColor;
	public Color depotColor;
	public Color noneColor;
	Color lastColor;
	float timer = 0.0f;
	
	MapControl mapControl;
	
	bool selected;
	bool isFortified;
	
	public GameObject greenPowerUpFX;
	public GameObject redPowerUpFX;

	public Transform mapRing;

	void Start () {
		GameObject mapControlObj = GameObject.Find("MapControl");
		mapControl = mapControlObj.GetComponent<MapControl>();
		label = Instantiate(labelPrefab, transform.position, Quaternion.AngleAxis(180, Vector3.up)) as MapLabel;
		label.SetUp(this);
		locName = mapControl.GetLocationName();
		label.SetLabelText(locName);
		label.transform.parent = transform;
		LoadState();
		HideRing();
	}
	
	void Update () {
		if (timer < 1.0f) {
			timer += GameTime.deltaTime;
			//get start color
			Color newColor = Color.white;
			switch (currentStatus) {
			case DotStatus.PlayerPowered :
				newColor = playerColor;
				break;
			case DotStatus.PlayerUnpowered :
				newColor = playerColor * 0.5f;
				break;
			case DotStatus.EnemyPowered :
				newColor = enemyColor;
				break;
			case DotStatus.EnemyUnpowered :
				newColor = enemyColor * 0.5f;;
				break;
			case DotStatus.Depot :
				newColor = depotColor;
				break;
			case DotStatus.None :
				newColor = noneColor;
				break;
			}
			Color transColor = Color.Lerp(lastColor, newColor, timer);
			SetColors(transColor);
			if (timer > 1.0f) {
				lastColor = newColor;
			}
		}
	}
	
	public string GetName() {
		return locName;	
	}
	
	public void SetAsEnemyBase() {
		renderer.material.mainTexture = baseDotTexture;
		isEnemyBase = true;
		currentStatus = DotStatus.EnemyPowered;
		timer = 0.0f;
	}	
	
	public void SetAsFortified() {
		isFortified = true;
	}
	
	public bool IsEnemyBase() {
		return isEnemyBase;
	}
	
	public bool IsEnemyControlled() {
		if (currentStatus == DotStatus.EnemyPowered || currentStatus == DotStatus.EnemyUnpowered) return true;
		return false;
	}
		
	public void SetAsPlayerBase() {
		renderer.material.mainTexture = baseDotTexture;
		isPlayerBase = true;
		currentStatus = DotStatus.PlayerPowered;
		timer = 0.0f;
	}	
	
	public bool IsPlayerBase() {
		return isPlayerBase;
	}	
	
	public bool IsPlayerControlled() {
		if (currentStatus == DotStatus.PlayerPowered || currentStatus == DotStatus.PlayerUnpowered) return true;
		return false;
	}
	
	
	public void SetAsDepot() {
		currentStatus = DotStatus.Depot;
		timer = 0.0f;		
	}
	
	public bool IsDepot() {
		if (currentStatus == DotStatus.Depot) return true;
		return false;		
	}
	
	public bool IsEndPoint() {
		if (connectedPoints.Count <= 1) {
			if (!IsPlayerBase() && !IsEnemyBase()) {
				return true;
			}
		}
		return false;
		
	}
	
	public bool IsPowered() {
		if (currentStatus == DotStatus.PlayerPowered || currentStatus == DotStatus.EnemyPowered) return true;
		return false;
	}	
	
	public bool IsFortified() {
		return isFortified;
	}
		
	public void AddConnection(Transform connectedPoint, MapLine connectedLine) {
		connectedPoints.Add(connectedPoint);
		connectedLines.Add(connectedLine);
	}
	
	public bool IsConnectedTo(Transform point) {
		return connectedPoints.Contains(point);
	}
	
	public bool IsConnectedToPlayer() {
		foreach (Transform dot in connectedPoints) {
			if (dot.GetComponent<MapDot>().IsPlayerControlled() && dot.GetComponent<MapDot>().IsPowered()) return true;
		}
		return false;
	}
	
	public bool IsConnectedToEnemy() {
		foreach (Transform dot in connectedPoints) {
			if (dot.GetComponent<MapDot>().IsPlayerControlled() && dot.GetComponent<MapDot>().IsPowered()) return true;
		}
		return false;
	}
	
	public List<Transform> GetConnections() {
		return connectedPoints;
	}
	
	public DotStatus GetStatus() {
		return currentStatus;	
	}
	
	public void PlayerCapture() {
		currentStatus = DotStatus.PlayerUnpowered;
		foreach (MapLine line in connectedLines) line.SetOwners();
		timer = 0.0f;
		mapControl.PowerDots();
		SaveState();
	}
	
	public void EnemyCapture() {
		currentStatus = DotStatus.EnemyUnpowered;
		foreach (MapLine line in connectedLines) line.SetOwners();
		timer = 0.0f;
		mapControl.PowerDots();
		SaveState();
	}	
	
	public void PowerUp() {
		if (currentStatus == DotStatus.PlayerUnpowered) {
			currentStatus = DotStatus.PlayerPowered;
			Instantiate(greenPowerUpFX, transform.position, Quaternion.identity);
		}
		if (currentStatus == DotStatus.EnemyUnpowered) {
			currentStatus = DotStatus.EnemyPowered;
			Instantiate(redPowerUpFX, transform.position, Quaternion.identity);
		}
		timer = 0.0f;
	}
	
	public void PowerDown() {
		if (currentStatus == DotStatus.PlayerPowered) currentStatus = DotStatus.PlayerUnpowered;
		if (currentStatus == DotStatus.EnemyPowered) currentStatus = DotStatus.EnemyUnpowered;
		timer = 0.0f;
		foreach (MapLine line in connectedLines) line.SetOwners();
	}
		
	public void Release() {
		currentStatus = DotStatus.None;
	}	
	
	public void TransmitPlayerPower(List<MapDot> checkedList) {
		if (IsPlayerControlled()) {
			if (!checkedList.Contains(this)) {
				if (!IsPowered()) PowerUp();
				checkedList.Add(this);
				foreach (Transform dot in connectedPoints) dot.GetComponent<MapDot>().TransmitPlayerPower(checkedList);
			}
		}
	}
	public void TransmitEnemyPower(List<MapDot> checkedList) {
		if (IsEnemyControlled()) {
			if (!checkedList.Contains(this)) {
				if (!IsPowered()) PowerUp();
				checkedList.Add(this);
				foreach (Transform dot in connectedPoints) dot.GetComponent<MapDot>().TransmitEnemyPower(checkedList);
			}
		}
	}

    void SetColors(Color newColor) {
        Util.SetVertColors(transform, newColor);
    }
	
	public void Select() {
		if (selected) return;
		selected = true;
		label.SetSelected();
		
		if (IsPlayerBase()) {
			label.SetAction1("Scramble Grid");	
			label.SetAction2("Blank");	
			return;
		}
		

		
		if (IsConnectedToPlayer()) {
			if (IsPlayerControlled()) {
				label.SetAction1("Fortify Location");	
				label.SetAction2("Blank");
				
			} else if (IsEnemyBase()){
				label.SetAction1("Destroy Base");	
				label.SetAction2("Blank");
				
			} else if (IsDepot()) {
				label.SetAction1("Connect Depot");	
				label.SetAction2("Blank");
				
			} else {
				label.SetAction1("Capture Location");	
				label.SetAction2("Install Hack");	
			}
		} else {
			if (IsEnemyBase()){
				label.SetAction1("Gather Intel");	
				label.SetAction2("Blank");	
				
			} else if (IsDepot()) {
				label.SetAction1("Blank");	
				label.SetAction2("Blank");
				
			} else {
				label.SetAction1("Install Hack");	
				label.SetAction2("Blank");
			}
		}
		foreach (Transform dot in connectedPoints) {
			dot.GetComponent<MapDot>().ShowRing();	
		}

		foreach (MapLine line in connectedLines) {
			line.SelectLine();	
		}
		
		
	}
	
	public void UnSelect() {
		if (!selected) return;
		selected = false;
		label.SetUnSelected();
		foreach (Transform dot in connectedPoints) {
			dot.GetComponent<MapDot>().HideRing();	
		}		

		foreach (MapLine line in connectedLines) {
			line.UnSelectLine();	
		}
	}
	
	public void ShowRing() {
		mapRing.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		mapRing.renderer.enabled = true;
	}
	public void HideRing() {
		mapRing.renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
		mapRing.renderer.enabled = false;
	}
		

		
	public void tap(TouchManager.TapEvent touchEvent) {
		if (touchEvent.touchTarget.tag == "MapAction") {
			switch (touchEvent.touchTarget.name) {
			case "Fortify Location"	:
				print ("Not Hooked UP");
				break;
					
			case "Destroy Base" :
				print ("Not Hooked UP");
				break;
						
			case "Capture Location" :
				mapControl.LaunchLevel(transform);
				break;
				
			case "Install Hack" :
				print ("Not Hooked UP");
				break;
				
			case "Gather Intel" :
				print ("Not Hooked UP");
				break;
				
			default :
				print ("Unknown command");
				break;
			}
		}
		if (touchEvent.touchTarget.tag == "MapPoint") {
			mapControl.SelectDot(this);
		}
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		mapControl.drag(touchEvent);
	}
	
	void LoadState() {
		
		int statusId = GameLoadSave.GetMapDotState(transform.name);
		if (statusId < 0) return;
		
		switch (statusId) {

		case 0 :
			currentStatus = DotStatus.PlayerPowered;
			break;
		case 1 :
			currentStatus = DotStatus.PlayerUnpowered;
			break;
		case 2 :
			currentStatus = DotStatus.EnemyPowered;
			break;
		case 3 :
			currentStatus = DotStatus.EnemyUnpowered;
			break;
		case 4 :
			currentStatus = DotStatus.Depot;
			break;
		case 5 :
			currentStatus = DotStatus.None;
			break;
		}
		//print ("setting " + transform.name + " to " + currentStatus);
	}
	
	void SaveState() {
		int saveStatus = -1;
		switch (currentStatus) {
		case DotStatus.PlayerPowered :
			saveStatus = 0;
			break;
		case DotStatus.PlayerUnpowered :
			saveStatus = 1;
			break;
		case DotStatus.EnemyPowered :
			saveStatus = 2;
			break;
		case DotStatus.EnemyUnpowered :
			saveStatus = 3;
			break;
		case DotStatus.Depot :
			saveStatus = 4;
			break;
		case DotStatus.None :
			saveStatus = 5;
			break;
		}
		GameLoadSave.SetMapDotState(transform.name, saveStatus);
	}

}
