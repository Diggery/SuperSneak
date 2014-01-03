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
	
	public static void SetOpenCrates(int mapSeed, int crateId) {
		if (PreviewLabs.PlayerPrefs.HasKey(mapSeed.ToString())) {
			if (!IsCrateOpened(mapSeed, crateId)) {
				string newOpenCrates = GetOpenCrates(mapSeed) + "," + crateId;
				PreviewLabs.PlayerPrefs.SetString(mapSeed.ToString(), newOpenCrates);
			}
		} else {
			PreviewLabs.PlayerPrefs.SetString(mapSeed.ToString(), crateId.ToString());
		}
	}
	
	public static string GetOpenCrates(int mapSeed) {
		if (PreviewLabs.PlayerPrefs.HasKey(mapSeed.ToString())) {
			return PreviewLabs.PlayerPrefs.GetString(mapSeed.ToString());
		}
		return "none";
	}	
	
	public static bool IsCrateOpened(int mapSeed, int crateId) {
		if (PreviewLabs.PlayerPrefs.HasKey(mapSeed.ToString())) {
			string[] openCrates = PreviewLabs.PlayerPrefs.GetString(mapSeed.ToString()).Split(new char[] {','});
			foreach (string crate in openCrates) {
				if (crate.Equals(crateId.ToString())) {
					Debug.Log ("crate " + crate + " is open");	
					return true;
				}
			}
		}
		return false;
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