using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlRoom : MonoBehaviour {
	
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
		
	}
	
	void Update() {
		if (spawnQueue.Count > 0) 	{
			spawnTimer -= Time.deltaTime;
			if (spawnTimer < 0.0f) SpawnEnemyFromQueue();
		}
	}
	
	public void SpawnPartrol() {
		QueueEnemy("Janitor");
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
}
