using UnityEngine;
using System.Collections;


public class GameLoadSave : object {
	
	public static void SetGameMapSeed(int newSeed) {  
		PreviewLabs.PlayerPrefs.SetInt("gameMapSeed", newSeed);
	}
	public static int GetGameMapSeed() {  
		if (PreviewLabs.PlayerPrefs.HasKey("gameMapSeed")) {
			return PreviewLabs.PlayerPrefs.GetInt("gameMapSeed");
		}
		return -1;
	}

	public static void SetMapDotState(string mapDot, int currentState) {  
		if (mapDot.Contains(":")) {
			Debug.Log("ERROR: thats a pretty bad key name");	
		}
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