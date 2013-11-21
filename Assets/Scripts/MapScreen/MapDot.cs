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
	
	public GameObject greenPowerUpFX;
	public GameObject redPowerUpFX;



	void Start () {
		GameObject mapControlObj = GameObject.Find("MapControl");
		mapControl = mapControlObj.GetComponent<MapControl>();
		label = Instantiate(labelPrefab, transform.position, Quaternion.AngleAxis(180, Vector3.up)) as MapLabel;
		label.SetUp(this);
		label.SetLabelText(mapControl.GetLocationName());
		label.SetActionText1("Player");
		label.SetActionText2("Enemy");
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
	
	public void SetAsEnemyBase() {
		renderer.material.mainTexture = baseDotTexture;
		isEnemyBase = true;
		currentStatus = DotStatus.EnemyPowered;
		timer = 0.0f;
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
	
	public void AddConnection(Transform connectedPoint, MapLine connectedLine) {
		connectedPoints.Add(connectedPoint);
		connectedLines.Add(connectedLine);
	}
	
	public bool IsConnectedTo(Transform point) {
		return connectedPoints.Contains(point);
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
	}
	
	public void EnemyCapture() {
		currentStatus = DotStatus.EnemyUnpowered;
		foreach (MapLine line in connectedLines) line.SetOwners();
		timer = 0.0f;
		mapControl.PowerDots();
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
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

		colors[0] = newColor;
		colors[1] = newColor;
		colors[2] = newColor;
		colors[3] = newColor;
		
		mesh.colors = colors;
    }
	
	public void Select() {
		if (selected) return;
		selected = true;
		label.SetSelected();
	}
	
	public void UnSelect() {
		if (!selected) return;
		selected = false;
		label.SetUnSelected();
	}
	

		
	public void tap(TouchManager.TapEvent touchEvent) {
		if (touchEvent.touchTarget.name == "ActionText1") {
			PlayerCapture();
		} 
		if (touchEvent.touchTarget.name == "ActionText2") {
			EnemyCapture();
		} 
		mapControl.SelectDot(this);
	}
	
	public void drag(TouchManager.TouchDragEvent touchEvent) {
		mapControl.drag(touchEvent);
	}
}
