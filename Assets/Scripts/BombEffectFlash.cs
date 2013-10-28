using UnityEngine;
using System.Collections;

public class BombEffectFlash : MonoBehaviour {
	
	public float blastRadius = 10;
	public float effectDuration = 5;
	public GameObject flashParticles;

	
	public void explode() {
		Vector4 soundEventData = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
		Events.Send(gameObject, "SoundEvents", soundEventData);

		Instantiate(flashParticles, transform.position, Quaternion.identity);
		
        Collider[] victims = Physics.OverlapSphere(transform.position, blastRadius);
		foreach (Collider victim in victims) {
			if (Util.CanSeeEachOther(victim.transform, transform)) {
            	EnemyController enemyController = victim.GetComponent<EnemyController>();
				if (enemyController) enemyController.Blinded(effectDuration);
			}
        }	
		
		Destroy(gameObject);
	}

}
