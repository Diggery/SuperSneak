using UnityEngine;
using System.Collections;
//using Events;

public class GameTime : MonoBehaviour {

	// GameTime component of a game object used to track 
	// the unpaused delta time in its update
	static GameTime theGameTime;
	static bool gamePaused = false;

	float lastUpdateTime = 0;
	float fixedTime = 0;
	
	static void Initialize() {
		if (!theGameTime) {
			GameObject go = new GameObject("GameTime");
			theGameTime = go.AddComponent(typeof(GameTime)) as GameTime;
		}
	}
	
	public static float unpausedDeltaTime { 
		get { 
			if (!theGameTime) {
				Initialize();
			}
			if (theGameTime) 
				return Time.realtimeSinceStartup - theGameTime.lastUpdateTime;
			return 0;
		}
	}
	
	public static bool paused {
		get { return gamePaused; }
		set {
			if (gamePaused != value) {
				gamePaused = value;
				Time.timeScale = gamePaused ? 0.0f : 1.0f;
			    Events.Send(theGameTime.gameObject, "OnPauseChanged", (object)gamePaused);
				if (gamePaused)
				    Events.Send(theGameTime.gameObject, "OnPauseGame");
				else 		
				    Events.Send(theGameTime.gameObject, "OnResumeGame");
			}	
		}
	}
	
	void Update() {
		lastUpdateTime = Time.realtimeSinceStartup;
		fixedTime += 0.016f;
		Shader.SetGlobalFloat("fixedTime", fixedTime);
	}
}
