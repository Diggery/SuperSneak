using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public Rigidbody bullet;
	public float coolDown;
	public float spinUp;
	public float coolDownTimer;
	public float spinUpTimer;
	public bool idle;

	
	public enum WeaponState { Off, WarmingUp, Ready }
	public WeaponState currentState = WeaponState.Off;
		
	
	void Start() {
		
	}
	
	public void setUpWeapon(GameObject newBullet, float newCoolDown, float newSpinUp) {
		bullet = newBullet.rigidbody;
		coolDown = newCoolDown;
		spinUp = newSpinUp;
	}
	
	void Update () {
		
		if (currentState == WeaponState.Ready) {
			coolDownTimer -= Time.deltaTime;
			if (coolDownTimer < -spinUp) {
				currentState = WeaponState.Off;
				coolDownTimer = 0.0f;
			}
		}
		
		if (currentState == WeaponState.WarmingUp) {
			spinUpTimer +=	Time.deltaTime;
			if (spinUpTimer > spinUp) {
				spinUpTimer = 0.0f;	
				currentState = WeaponState.Ready;
			}
		}

	}
	
	public void fire(Vector3 target) {
		switch (currentState) {
			case WeaponState.Off :
				currentState = WeaponState.WarmingUp;
			break;
			
			case WeaponState.WarmingUp :
			
			break;
		
			case WeaponState.Ready :
				if (coolDownTimer > 0) return;
				Rigidbody newBullet = Instantiate(bullet, transform.position, transform.rotation) as Rigidbody;
				Vector3 shootVector = ( target - transform.position).normalized;
				newBullet.AddForce(shootVector * 10, ForceMode.Impulse);
				coolDownTimer = coolDown;			
			break;
		
		}
	}
}
