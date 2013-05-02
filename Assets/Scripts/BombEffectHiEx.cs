using UnityEngine;
using System.Collections;

public class BombEffectHiEx : MonoBehaviour {
	
	public float blastRadius = 5;
	public float damage = 5;
	public GameObject explosionParticles;

	
	public void explode() {
		Vector4 soundEventData = new Vector4(transform.position.x, transform.position.y, transform.position.z, 5.0f);
		Events.Send(gameObject, "SoundEvents", soundEventData);

		Instantiate(explosionParticles, transform.position, Quaternion.identity);
		
        Collider[] victims = Physics.OverlapSphere(transform.position, blastRadius);
		foreach (Collider victim in victims) {
			
			Vector4 expData = new Vector4(transform.position.x, transform.position.y, transform.position.z, damage);
            victim.SendMessage("addExpDamage", expData, SendMessageOptions.DontRequireReceiver);
        }	
		
		Destroy(gameObject);
	}

}
