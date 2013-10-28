using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardRoom : MonoBehaviour {
	
	public List<Transform> entrances;
	public List<GameObject> enemyTypes;
	
	public int MaxEnemies = 10;
	public int currentEnemies;
	public float spawnCoolDown = 3.0f;
	public float spawnTimer = 0.0f;
	public List<string> spawnQueue;
	
	Hashtable characterPrefabs;
	
	bool poweredOn;
	
	void Start() {
		//load up the enemy hashtable
		characterPrefabs = new Hashtable();
		foreach (GameObject enemy in enemyTypes) {
			characterPrefabs[enemy.name] = enemy;
		}
		spawnTimer = -1.0f;
		
		TurnOn();
		
		Invoke("SpawnPartrol", 5);
		
		Events.Listen(gameObject, "SoundEvents");  
	}
	
	void Update() {
		if (spawnQueue.Count > 0) 	{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer < 0.0f) SpawnEnemyFromQueue();
		}
	}
	
	public void SpawnPartrol() {
		QueueEnemy("Guard");
	}
	
	public void QueueEnemy(string type) {
		spawnQueue.Add(type);
		print (spawnQueue[0] + " is queued");
	}

	
	public void ShutDown() {
		poweredOn = false;
	}
	
	public void TurnOn() {
		poweredOn = true;
	}	
	
	public IEnumerator SoundEvents(Events.Notification notification) {
		Vector4 soundData = (Vector4)notification.data;
		Vector3 soundPos = new Vector3(soundData.x, soundData.y, soundData.z);
		float volume = soundData.w;
		yield return new WaitForSeconds(1);
		
		if (volume >= 5.0f) {
			GameObject responder = ForceSpawnEnemy("Guard");
			responder.GetComponent<EnemyController>().investigate(soundPos);
		}		
	}
	
	
	public GameObject SpawnEnemyFromQueue() {
		
		if (!poweredOn) return null;
		
		if (currentEnemies > MaxEnemies) {
			spawnTimer = spawnCoolDown;
			return null;
		}
		
		print ("Spawning a " + spawnQueue[0]);
				
		Transform entrance = entrances[Random.Range(0, entrances.Count)];
		
		GameObject prefab = (GameObject)characterPrefabs[spawnQueue[0]];
		GameObject newEnemy = Instantiate(prefab, entrance.position, entrance.rotation) as GameObject;
		currentEnemies++;
		spawnQueue.RemoveAt(0);
		spawnTimer = spawnCoolDown;
		return newEnemy;
	}
	
	public GameObject ForceSpawnEnemy(string enemyType) {
		currentEnemies++;
		GameObject prefab = (GameObject)characterPrefabs[enemyType];
		Transform entrance = entrances[Random.Range(0, entrances.Count)];
		GameObject newEnemy = Instantiate(prefab, entrance.position, entrance.rotation) as GameObject;
		spawnTimer = spawnCoolDown;
		return newEnemy;
	}
}
