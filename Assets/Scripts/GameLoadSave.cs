using UnityEngine;
using System.Collections;


public class GameLoadSave : object {
	
	public static void SetMapDotState(string mapDot, int currentState) {  
		PreviewLabs.PlayerPrefs.SetInt(mapDot, currentState);
	}
	
	public static int GetMapDotState(string mapDot) {
		if (PreviewLabs.PlayerPrefs.HasKey(mapDot)) {
			return PreviewLabs.PlayerPrefs.GetInt(mapDot);
		}
		return -1;
	}
	
	public static void DeleteAll() {
		PreviewLabs.PlayerPrefs.DeleteAll();
		PreviewLabs.PlayerPrefs.Flush();
		Debug.Log ("level deleted");
	}
	
	public static void SaveAll() {
		PreviewLabs.PlayerPrefs.Flush();
	}
}