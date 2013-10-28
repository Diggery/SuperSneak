using UnityEngine;
using System.Collections;

public class BombEffectSmoke : MonoBehaviour {
	
	public float blastRadius = 5;
	public float duration = 5;
	public GameObject smokeCloudParticles;
	public GameObject smokeJetParticles;
	GameObject smokeCloud;
	GameObject smokeJet;
	GameObject areaOfEffect;
	
	public void explode() {
		Vector4 soundEventData = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
		Events.Send(gameObject, "SoundEvents", soundEventData);

		smokeCloud = Instantiate(smokeCloudParticles, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		smokeJet = Instantiate(smokeJetParticles, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		
		rigidbody.drag = 2;
		
		areaOfEffect = new GameObject("Area Of Effect");
		areaOfEffect.layer = LayerMask.NameToLayer("AreaOfEffect");
		areaOfEffect.transform.parent = transform;
		areaOfEffect.transform.localPosition = Vector3.zero;
		SphereCollider collision = areaOfEffect.AddComponent<SphereCollider>();
		collision.isTrigger = true;
		collision.radius = blastRadius;
		AreaOfEffect effect = areaOfEffect.AddComponent<AreaOfEffect>();
		effect.SetEffect ("Smoked");
		collision.isTrigger = true;
		
	}
	
	void Update() {
		if (smokeJet) {
			smokeJet.transform.position = transform.position;
			smokeCloud.transform.position = transform.position;
			duration -= Time.deltaTime;
			if (duration < 0) {
				smokeJet.GetComponent<ParticleSystem>().Stop();
				smokeCloud.GetComponent<ParticleSystem>().Stop();
				Destroy(gameObject);
			}
		}
	}
}
