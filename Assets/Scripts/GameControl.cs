using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	
	LevelController levelController; 
	
	
	int cratesOpened;
	bool allCratesOpened;

	public void Init () {
		levelController = LevelController.instance;

	}
	
	void Update () {
		
		if (!allCratesOpened && levelController.getNumberOfCrates() <= cratesOpened) {
			print("All Crates Gone");
			allCratesOpened = true;
		}
	
	}
	
	public void CrateOpened() {
		print("CrateOpened");
		cratesOpened++;
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
}
