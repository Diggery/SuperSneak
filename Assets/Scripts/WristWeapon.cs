using UnityEngine;
using System.Collections;

public class WristWeapon : MonoBehaviour {
	
	PlayerController playerController;
	
	public WeaponLaser weaponLaserPrefab;
	WeaponLaser weaponLaser;

	public void SetUp(Transform thiefRightHand) {
		transform.localRotation = Quaternion.AngleAxis(90, Vector3.down);
		transform.parent = thiefRightHand;
		
		weaponLaser = Instantiate(weaponLaserPrefab, transform.position, transform.rotation) as WeaponLaser;
		weaponLaser.transform.parent = transform;
			
	}
	
	void Update () {
	
	}
	
	public void Fire(string itemName) {
		switch (itemName) {
		case "Laser" :
			weaponLaser.Fire();
			break;
		}
	}
	
	public void StopFiring() {
		weaponLaser.StopFiring();
	}
}
