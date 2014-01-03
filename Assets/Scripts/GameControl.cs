using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour {
	
	LevelController levelController; 
	
	int cratesOpened;
	public bool serverHacked;
	
	public int gameMapSeed = 1;
	int currentSeed;
	string levelToLoad;
	
	public string currentLevel;
	public bool currentLevelPassed;

	
	public GameObject dialogBoxPrefab;
	DialogControl dialogBox;
	
	void Awake () {
    	DontDestroyOnLoad (transform.gameObject);
		Events.Listen(gameObject, "KeysPressed");
		gameMapSeed = GameLoadSave.GetGameMapSeed();
	}

	public void CrateOpened() {
		if (!levelController) levelController = LevelController.instance;
		cratesOpened++;
		int totalCrates = levelController.getNumberOfCrates();
		Vector2 crateStatus = new Vector2 (cratesOpened, totalCrates);
		Events.Send(gameObject, "CrateOpened", crateStatus);
	}

	public bool IsLevelComplete() {
		return serverHacked;
	}
	
	public void ServerHacked() {
		print("SERVER IS HACKED");
		Events.Send(gameObject, "ServerStatus", "Hacked");

		serverHacked = true;

		GameObject[] servers = GameObject.FindGameObjectsWithTag("Server");
		
		foreach (GameObject server in servers) {
			server.GetComponent<ServerControl>().Hacked();
		}
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
		currentLevelPassed = false;
		Invoke ("ReturnToMap", 5);
	}
	
	void Update() {
		
	}
	
	public void ReturnToMap() {
		LoadNewLevel("MapScreen", 1);
		
		OpenInfoBox("Opening Map Screen...", 3, 0.25f);
	}
	
	public void CreateGameMapSeed() {
		gameMapSeed = Random.Range(1, 100);
		GameLoadSave.SetGameMapSeed(gameMapSeed);
	}	
		
	public int GetGameMapSeed() {
		return gameMapSeed;	
	}	
	
	public int GetSeed() {
		return currentSeed;	
	}
	public void SetSeed(int newSeed) {
		currentSeed = newSeed;
		print ("currentSeed is set to " + currentSeed);
	}
		
	public void KeysPressed(Events.Notification notification) {
		string key = (string)notification.data;
		if (key == "Back") LoadNewLevel("MainMenu", 1);

		OpenInfoBox("Returning to Menu...", 3, 0.25f);

	}
	
	public void LaunchLevelFromSeed(int newSeed) {
		SetSeed(Mathf.Abs(newSeed));
		LoadNewLevel("GameLevel", 1);
		OpenInfoBox("Traveling to Location...", 3, 0.25f);
	}
	
	public void LaunchLevelFromMap(Transform selectedDot) {
		currentLevel = selectedDot.name;
		currentLevelPassed = false;
		float seed = (selectedDot.position.x * 1000) + (selectedDot.position.z * 100) ;
		int seedAsInt = Mathf.CeilToInt(seed);
		LaunchLevelFromSeed(seedAsInt);
	}
	
	public void LeaveLevel() {
		currentLevelPassed = IsLevelComplete();
		
		//save crates
		GameObject[] crates = GameObject.FindGameObjectsWithTag("Crate");
		if (!levelController) levelController = LevelController.instance;
		foreach (GameObject crate in crates) {
			CrateController crateControl = crate.GetComponent<CrateController>();
			if (crateControl.IsOpened()) {
				int crateId = crateControl.GetId();
				Debug.Log ("Saving crate " + crateId);
				GameLoadSave.SetOpenCrates(levelController.currentLevelSeed, crateId);
			}
		}
		
		ReturnToMap();
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
	
	void CreateDialog() {
		GameObject dialogBoxObj = Instantiate(dialogBoxPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		dialogBox = dialogBoxObj.GetComponent<DialogControl>();
		dialogBox.transform.parent = Camera.main.transform;
		dialogBox.Init();
	}
	
	public void OpenInfoBox(string text, float delay, float scale) {
		if (!dialogBox) CreateDialog();
		dialogBox.OpenInfoBox(text, delay, scale);
	}
	
	public void OpenMessageBox(string text, float delay, float scale, DialogControl.DialogDelegate result) {
		if (!dialogBox) CreateDialog();
		dialogBox.OpenMessageBox(text, delay, scale, result);
	}
	
	public void OpenConfirmationBox(string text, float delay, float scale, DialogControl.DialogDelegate result) {
		if (!dialogBox) CreateDialog();
		dialogBox.OpenConfirmationBox(text, delay, scale, result);
	}
	

	public void LoadNewLevel(string levelname, float delay) {
		levelToLoad = levelname;
		Invoke("DoLoad", delay);	
	}
	
	public void DoLoad() {
		if (levelToLoad.Equals("empty")) return;
		Application.LoadLevel(levelToLoad);	
		levelToLoad = "empty";
	}
	
	public void OnApplicationQuit() {
		GameLoadSave.SaveAll();
	}
}
