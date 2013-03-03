using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Rigidbody bullet;
	public float coolDown;
	float coolDownTimer;
	
	void Start() {
		
	}
	
	public void setUpWeapon(GameObject newBullet, float newCoolDown) {
		bullet = newBullet.rigidbody;
		coolDown = newCoolDown;
	}
	
	void Update () {
		coolDownTimer -= Time.deltaTime;
	}
	public void fire(Vector3 target) {
		if (coolDownTimer > 0) return;
		Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
		Vector3 shootVector = ( target - transform.position).normalized;
		newBullet.AddForce(shootVector * 10, ForceMode.Impulse);
		coolDownTimer = coolDown;
	}
}
