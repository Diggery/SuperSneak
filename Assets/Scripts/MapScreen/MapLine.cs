using UnityEngine;
using System.Collections;

public class MapLine : MonoBehaviour {
	
	
	MapDot.DotStatus startStatus = MapDot.DotStatus.None;
	MapDot.DotStatus endStatus = MapDot.DotStatus.None;
	
	Transform startDot;
	Transform endDot;
	
	bool selected;
	
	public Color playerColor = Color.green;
	public Color enemyColor = Color.red;
	public Color noneColor = new Color(25.0f/256, 30.0f/256, 40.0f/256, 1.0f);
	
	float timer = 0.0f;
	Color lastStartColor;
	Color lastEndColor;
	
	public void SetUp(Transform newStart, Transform newEnd) {
		startDot = newStart;
		endDot = newEnd;
		lastStartColor = noneColor;
		lastEndColor = noneColor;
		timer = 0.0f;
	}
	
	void Update() {
		if (timer < 1.0f) {
			timer += GameTime.deltaTime;
			//get start color
			Color newStartColor = Color.white;
			switch (startStatus) {
			case MapDot.DotStatus.PlayerPowered :
				newStartColor = playerColor;
				break;
			case MapDot.DotStatus.PlayerUnpowered :
				newStartColor = playerColor * 0.5f;
				break;
			case MapDot.DotStatus.EnemyPowered :
				newStartColor = enemyColor;
				break;
			case MapDot.DotStatus.EnemyUnpowered :
				newStartColor = enemyColor * 0.5f;
				break;
			case MapDot.DotStatus.None :
				newStartColor = noneColor;
				break;
			default :
				newStartColor = noneColor;
				break;
			}
			if (selected) newStartColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);
			
			Color transStartColor = Color.Lerp(lastStartColor, newStartColor, timer);
			
			//get end color
			Color newEndColor = Color.white;
			switch (endStatus) {
			case MapDot.DotStatus.PlayerPowered :
				newEndColor = playerColor;
				break;
			case MapDot.DotStatus.PlayerUnpowered :
				newEndColor = playerColor * 0.5f;;
				break;
			case MapDot.DotStatus.EnemyPowered :
				newEndColor = enemyColor;
				break;
			case MapDot.DotStatus.EnemyUnpowered :
				newEndColor = enemyColor * 0.5f;;
				break;
			case MapDot.DotStatus.None :
				newEndColor = noneColor;
				break;
			default :
				newStartColor = noneColor;
				break;
			}
			if (selected) newEndColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

			Color transEndColor = Color.Lerp(lastEndColor, newEndColor, timer);
			SetColors(transStartColor, transEndColor);
			
			if (timer > 1.0f) {
				lastStartColor = newStartColor;
				lastEndColor = newEndColor;				
			}
		}
	}
	
	
	public void SetOwners() {
		
		MapDot.DotStatus newStart = startDot.GetComponent<MapDot>().GetStatus();
		MapDot.DotStatus newEnd = endDot.GetComponent<MapDot>().GetStatus();

		if (startStatus == MapDot.DotStatus.PlayerUnpowered && newStart == MapDot.DotStatus.PlayerPowered) {
			lastStartColor = Color.white;
		}
				
		if (startStatus == MapDot.DotStatus.EnemyUnpowered && newStart == MapDot.DotStatus.EnemyPowered) {
			lastStartColor = Color.white;
		}

		if (endStatus == MapDot.DotStatus.PlayerUnpowered && newEnd == MapDot.DotStatus.PlayerPowered) {
			lastEndColor = Color.white;
		}
				
		if (endStatus == MapDot.DotStatus.EnemyUnpowered && newEnd == MapDot.DotStatus.EnemyPowered) {
			lastEndColor = Color.white;
		}
		
		startStatus = startDot.GetComponent<MapDot>().GetStatus();
		endStatus = endDot.GetComponent<MapDot>().GetStatus();
		timer = 0.0f;
	}
	
	public void SelectLine() {
		selected = true;
		timer = 0.0f;
	}
	public void UnSelectLine() {
		selected = false;
		SetOwners();
	}
	
    void SetColors(Color startColor, Color endColor) {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

		//start
		colors[0] = startColor;
		colors[1] = startColor;
		
		//end
		colors[2] = endColor;
		colors[3] = endColor;
		
		mesh.colors = colors;
    }	
}
