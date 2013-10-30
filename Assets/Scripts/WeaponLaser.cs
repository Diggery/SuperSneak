using UnityEngine;
using System.Collections;

public class WeaponLaser : MonoBehaviour {

	public float spinUp;
	float spinUpTimer;
	
	public ParticleSystem muzzleEffect;
	public ParticleSystem hitEffect;
	public LineRenderer beamEffect;
	
	public float range;
	
	public enum WeaponState { Off, WarmingUp, Firing }
	public WeaponState currentState = WeaponState.Off;
	
	PlayerController playerController;
		
	
	void Start () {
	}
	
	void Update () {

		
		switch (currentState) {
	
		case WeaponState.Off :
			beamEffect.SetColors(Color.black, Color.black);
			if (hitEffect.isPlaying) hitEffect.Stop();
			if (muzzleEffect.isPlaying) muzzleEffect.Stop();
			break;
			
		case WeaponState.WarmingUp :
			beamEffect.SetColors(Color.black, Color.black);
			if (hitEffect.isPlaying) hitEffect.Stop();
			if (!muzzleEffect.isPlaying) muzzleEffect.Play();
			muzzleEffect.emissionRate = (spinUpTimer / spinUp) * 30;
			muzzleEffect.startSize = (spinUpTimer / spinUp) * 0.5f;
			spinUpTimer +=	Time.deltaTime;
			if (spinUpTimer > spinUp) {
				spinUpTimer = 0.0f;	
				currentState = WeaponState.Firing;
			}
			break;
		
		case WeaponState.Firing :
			
			if (!muzzleEffect.isPlaying) muzzleEffect.Play();
			
			int layer1 = LayerMask.NameToLayer("PlayerRagDoll"); 
			int layer2 = LayerMask.NameToLayer("Player"); 
			int layer3 = LayerMask.NameToLayer("AreaOfEffect"); 
			int layer4 = LayerMask.NameToLayer("Obstacle"); 
			int layermask = ~((1 << layer1) | (1 << layer2) | (1 << layer3) | (1 << layer4));
			
			RaycastHit hit;
			
			Vector3 startPos = transform.position + new Vector3(0.0f,0.0f,0.0f);
			Vector3 endPos = startPos + transform.forward * range;
			endPos.y = 1.0f;
			beamEffect.SetPosition(0, startPos);
			beamEffect.SetPosition(1, endPos);

			if (Physics.Linecast (startPos, endPos, out hit, layermask)) {
				hitEffect.transform.position = hit.point;
				beamEffect.SetPosition(1, hit.point);
				if (!hitEffect.isPlaying) hitEffect.Play();
				beamEffect.SetColors(Color.red, Color.red);
				if (hit.transform.tag.Equals("Enemy")) {
					hit.transform.GetComponent<EnemyController>().Blinded(5);
				}
			} else {
				if (hitEffect.isPlaying) hitEffect.Stop();
				beamEffect.SetColors(Color.red, Color.black);

			}
			break;	
		}
	}
	
	public void Fire() {
		if (currentState == WeaponState.Off) currentState = WeaponState.WarmingUp;
	}
	
	public void StopFiring() {
		currentState = WeaponState.Off;
	}
}
