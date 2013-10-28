using UnityEngine;
using System.Collections;

public class BombEffectGas : MonoBehaviour {
	
	public float blastRadius = 5;
	public float duration = 5;
	public GameObject gasCloudParticles;
	public GameObject gasJetParticles;
	GameObject gasCloud;
	GameObject gasJet;
	GameObject areaOfEffect;
	
	public void explode() {
		Vector4 soundEventData = new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f);
		Events.Send(gameObject, "SoundEvents", soundEventData);

		gasCloud = Instantiate(gasCloudParticles, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		//ParticleSystem gasPorticles = gasCloud.GetComponent<ParticleSystem>();
		gasJet = Instantiate(gasJetParticles, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
		
		rigidbody.drag = 2;
		
		areaOfEffect = new GameObject("Area Of Effect");
		areaOfEffect.layer = LayerMask.NameToLayer("AreaOfEffect");
		areaOfEffect.transform.parent = transform;
		areaOfEffect.transform.localPosition = Vector3.zero;
		SphereCollider collision = areaOfEffect.AddComponent<SphereCollider>();
		collision.isTrigger = true;
		collision.radius = blastRadius;
		AreaOfEffect effect = areaOfEffect.AddComponent<AreaOfEffect>();
		effect.SetEffect ("Gassed");
		collision.isTrigger = true;
		
	}
	
	void Update() {
		if (gasJet) {
			gasJet.transform.position = transform.position;
			gasCloud.transform.position = transform.position;
			duration -= Time.deltaTime;
			if (duration < 0) {
				gasJet.GetComponent<ParticleSystem>().Stop();
				gasCloud.GetComponent<ParticleSystem>().Stop();
				Destroy(gameObject);
			}
		}
	}
}
