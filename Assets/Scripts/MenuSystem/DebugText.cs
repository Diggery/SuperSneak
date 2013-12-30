using UnityEngine;
using System.Collections;

public class DebugText : MonoBehaviour {
	
	public static GUIText Create(Vector2 newPos) {
		
		GameObject go = new GameObject("DebugText");
		go.transform.position = new Vector3(newPos.x, newPos.y, 0);
		GUIText newText = go.AddComponent<GUIText>();
		newText.text = "EmptyText";
		return newText;
		
	}
}
