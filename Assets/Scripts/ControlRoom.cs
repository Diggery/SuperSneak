using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlRoom : MonoBehaviour {
	
	public List<Transform> entrances;
	public List<GameObject> enemyTypes;
	
	public int MaxEnemies = 10;
	public int currentEnemies;
	
	Hashtable characterPrefabs;
	
	void Start() {
		//load up the enemy hashtable
		characterPrefabs = new Hashtable();
		foreach (GameObject enemy in enemyTypes) {
			characterPrefabs[enemy.name] = enemy;
		}
		
		//spawnEnemy("Janitor");
	}

	
	public GameObject spawnEnemy(string type) {
		
		if (currentEnemies >= MaxEnemies) return null;
		
		GameObject newEnemy;
		
		Transform entrance = entrances[Random.Range(0, entrances.Count)];
		
		GameObject prefab = (GameObject)characterPrefabs[type];
		newEnemy = Instantiate(prefab, entrance.position, entrance.rotation) as GameObject;
		currentEnemies++;
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
