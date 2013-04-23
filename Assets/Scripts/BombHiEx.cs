using UnityEngine;
using System.Collections;

public class BombHiEx : MonoBehaviour {
	
	public float fuseTime = 3;
	public float blastRadius = 5;
	public float damage = 5;
	float timer = 0.0f;
	public GameObject explosionParticles;
	SphereCollider collision;
	bool dud = false;

	void Start () {
		collision = GetComponent<SphereCollider>();
	}
	
	void Update () {
		if (dud) return;
		if (rigidbody.useGravity) timer += Time.deltaTime;
		if (!collision.enabled && timer >  0.25f) collision.enabled = true;
		if (timer > fuseTime) detonate();
	
	}
	
	
	public void detonate() {
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
	
    void OnCollisionEnter(Collision collision) {
		if (dud) return;
		if (collision.transform.tag == "Enemy") detonate();
	}
}
