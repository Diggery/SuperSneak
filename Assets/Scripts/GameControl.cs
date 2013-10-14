using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	
	LevelController levelController; 
	
	int cratesOpened;
	bool allCratesOpened;
	
	public int currentSeed;
	
	void Awake () {
    	DontDestroyOnLoad (transform.gameObject);
		Events.Listen(gameObject, "KeysPressed"); 
	}

	public void CrateOpened() {
		print("CrateOpened");
		
		cratesOpened++;
		if (!levelController) levelController = LevelController.instance;
		if (!allCratesOpened && levelController.getNumberOfCrates() <= cratesOpened) {
			print("All Crates Gone");
			allCratesOpened = true;
		}		
	}	
	
	public bool LevelComplete() {
		return allCratesOpened;
	}	
		
	public void PlayerIsDead() {
		print("Game Over");
		
		GameObject guardRoom = GameObject.FindGameObjectWithTag("GuardRoom");
		guardRoom.GetComponent<ControlRoom>().ShutDown();
		
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in allEnemies) {
			EnemyController controller = enemy.GetComponent<EnemyController>();
			controller.StandDown();
		}
		
	}
	
	public int GetSeed() {
		return currentSeed;	
	}
	
	public void KeysPressed(Events.Notification notification) {
		string key = (string)notification.data;
		if (key == "Back") Application.LoadLevel("MainMenu");
	}
	
	public void LeaveLevel() {
		Application.LoadLevel("MainMenu");
		
	}
	
}
