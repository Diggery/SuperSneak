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
		guardRoom.GetComponent<GuardRoom>().ShutDown();
		
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		
		GameObject killer = FindClosestEnemy(player.transform.position);
				
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in allEnemies) {
			if (enemy == killer) {
				enemy.GetComponent<EnemyController>().LookAtDeadPlayer();
			} else {
				EnemyController controller = enemy.GetComponent<EnemyController>();
				controller.StandDown();
			}
		}
		
	}
	
	public int GetSeed() {
		return currentSeed;	
	}
	public void SetSeed(int newSeed) {
		currentSeed = newSeed;	
	}
		
	public void KeysPressed(Events.Notification notification) {
		string key = (string)notification.data;
		if (key == "Back") Application.LoadLevel("MainMenu");
	}
	
	public void LeaveLevel() {
		Application.LoadLevel("MainMenu");
		
	}
	
    GameObject FindClosestEnemy(Vector3 point) {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos) {
            Vector3 diff = go.transform.position - point;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
	
}
