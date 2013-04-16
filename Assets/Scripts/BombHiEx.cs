using UnityEngine;
using System.Collections;

public class BombHiEx : MonoBehaviour {
	
	public float timer = 3;
	public float blastRadius = 5;
	public float damage = 5;
	public GameObject explosionParticles;

	void Start () {
	
	}
	
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) detonate();
	
	}
	
	
	public void detonate() {
		Instantiate(explosionParticles, transform.position, Quaternion.identity);
		
        Collider[] victims = Physics.OverlapSphere(transform.position, blastRadius);
		foreach (Collider victim in victims) {
            victim.SendMessage("addDamage", damage, SendMessageOptions.DontRequireReceiver);
        }	
		
		Destroy(gameObject);
	}
	
    void OnCollisionEnter(Collision collision) {
		if (collision.transform.tag == "Enemy") detonate();
	}
}
