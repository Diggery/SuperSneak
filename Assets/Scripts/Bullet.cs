using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	public float lifeSpan;
	public float damage;
	public Vector3 startPos;

	void Start () {
		startPos = transform.position;
	}
	
	void Update () {
		lifeSpan -= Time.deltaTime;
		if (lifeSpan < 0) Destroy(gameObject);
	}
	
    void OnCollisionEnter(Collision collision) {
		lifeSpan = 0.25f;
		if (collision.transform.tag == "Player") {
			collision.gameObject.GetComponent<PlayerController>().addDamage(damage, startPos);
		}
	}
}
