using UnityEngine;
using System.Collections;

public class BombFuse : MonoBehaviour {
	
	public float fuseTime = 1;
	float timer = 0.0f;
	SphereCollider collision;
	bool dud = false;
	bool primed = false;

	void Start () {
		collision = GetComponent<SphereCollider>();
	}
	
	void Update () {
		if (dud) return;
		if (rigidbody.useGravity) timer += Time.deltaTime;
		if (!collision.enabled && timer >  0.25f) collision.enabled = true;
		if (timer > fuseTime && primed) detonate();
	
	}
	
	
	public void detonate() {
		dud = true;
		SendMessage("explode");
	}
	
    void OnCollisionEnter(Collision collision) {
		primed = true;
		timer = 0.0f;
		if (dud) return;
		if (collision.transform.tag == "Enemy") detonate();
	}
}
