using UnityEngine;
using System.Collections;


public class EnemyManager : MonoBehaviour {

	GameObject[] enemies;
	
	public float guardRunSpeed;
	public float guardWalkSpeed;
	public float captainRunSpeed;
	public float captainWalkSpeed;
	public float enforcerRunSpeed;
	public float enforcerWalkSpeed;
	public float bossRunSpeed;
	public float bossWalkSpeed;
	
	public bool alerted;
	
	void Start () {
   	 	enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
	void Update () {
	
	}
	
	public void alert (Vector3 alertPos) {
		
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<EnemyController>().investigate(alertPos);		
		}
		alerted = true;
	}
	
	public float getRunSpeed (EnemyTypes type) {
		switch (type) {
		case EnemyTypes.Guard : return guardRunSpeed;
		case EnemyTypes.Captain : return captainRunSpeed;
		case EnemyTypes.Enforcer : return enforcerRunSpeed;
		case EnemyTypes.Boss : return bossRunSpeed;
		}
		return 0;
	}
	
	public float getWalkSpeed (EnemyTypes type) {
		switch (type) {
		case EnemyTypes.Guard : return guardWalkSpeed;
		case EnemyTypes.Captain : return captainWalkSpeed;
		case EnemyTypes.Enforcer : return enforcerWalkSpeed;
		case EnemyTypes.Boss : return bossWalkSpeed;
		}
		return 0;
	}
}
