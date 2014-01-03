using UnityEngine;
using System.Collections;

public class MapLine : MonoBehaviour {
	
	
	MapDot.DotStatus startStatus = MapDot.DotStatus.None;
	MapDot.DotStatus endStatus = MapDot.DotStatus.None;
	
	Transform startDot;
	Transform endDot;
		
	public Color playerColor;
	public Color enemyColor;
	public Color hackedColor;
	public Color noneColor;
	
	float timer = 0.0f;
	Color lastStartColor;
	Color lastEndColor;
	
	public void SetUp(Transform newStart, Transform newEnd) {
		startDot = newStart;
		endDot = newEnd;

		SetOwners();
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
			case MapDot.DotStatus.Hacked :
				newStartColor = hackedColor;
				break;
			case MapDot.DotStatus.None :
				newStartColor = noneColor;
				break;
			default :
				newStartColor = noneColor;
				break;
			}
			if (startDot.GetComponent<MapDot>().IsSelected()) newStartColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			
			
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
			case MapDot.DotStatus.Hacked :
				newEndColor = hackedColor;
				break;
			case MapDot.DotStatus.None :
				newEndColor = noneColor;
				break;
			default :
				newEndColor = noneColor;
				break;
			}
			if (endDot.GetComponent<MapDot>().IsSelected()) newEndColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

			Color transStartColor = Color.Lerp(lastStartColor, newStartColor, timer);
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
	
	public void UpdateLine() {
		timer = 0.0f;
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
