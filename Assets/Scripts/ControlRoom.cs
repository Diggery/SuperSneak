using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlRoom : MonoBehaviour {
	
	public List<Transform> entrances;
	public List<GameObject> enemyTypes;
	
	public int MaxEnemies = 10;
	public int currentEnemies;
	public float spawnCoolDown = 3.0f;
	float spawnTimer = 0.0f;
	int spawnQueue = 0;
	
	Hashtable characterPrefabs;
	
	void Start() {
		//load up the enemy hashtable
		characterPrefabs = new Hashtable();
		foreach (GameObject enemy in enemyTypes) {
			characterPrefabs[enemy.name] = enemy;
		}
		
		//spawnEnemy("Guard");
		
		spawnTimer = 3.0f;
	}
	
	void Update() {
		if (spawnTimer > 0.0f) 	spawnTimer -= Time.deltaTime;
		if (spawnTimer < 0.0f && currentEnemies < MaxEnemies) spawnEnemy("Guard");
	}
	
	public GameObject spawnEnemy(string type) {
		
		
		if (currentEnemies >= MaxEnemies) {
			spawnQueue++;
			return null;
		}
		if (spawnTimer > 0.0f) {
			spawnQueue++;
			return null;
		}
		
		GameObject newEnemy;
		
		Transform entrance = entrances[Random.Range(0, entrances.Count)];
		
		GameObject prefab = (GameObject)characterPrefabs[type];
		newEnemy = Instantiate(prefab, entrance.position, entrance.rotation) as GameObject;
		currentEnemies++;
		spawnQueue--;
		spawnTimer = spawnCoolDown;
		return newEnemy;
		
//		
//		switch (type) {
//		case "Guard" :
//			newEnemy = Instantiate(characterPrefabs["Guard"], entrance.position, entrance.rotation);
//			break;
//		default :
//			newEnemy = Instantiate(guardPrefab, entrance.position, entrance.rotation);
//			break;
//			
//		}
	}
}
