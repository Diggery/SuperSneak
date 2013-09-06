using UnityEngine;
using System.Collections;

public class BombTarget : MonoBehaviour {
	
	GameObject player;
	Transform noBombObj;
	Transform bombTargetObj;
	Transform targetOverlayObj;
	Transform bombArc;
	
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		noBombObj = transform.Find ("NoBomb");
		if (!noBombObj) Debug.Log("ERROR: Cant find NoBomb Transform");
		bombTargetObj = transform.Find ("BombTarget");
		if (!bombTargetObj) Debug.Log("ERROR: Cant find BombTarget Transform");
		targetOverlayObj = transform.Find ("TargetOverlay");
		if (!targetOverlayObj) Debug.Log("ERROR: Cant find TargetOverlay Transform");	
		bombArc = transform.Find ("BombArc");
		if (!targetOverlayObj) Debug.Log("ERROR: Cant find TargetOverlay Transform");	
	}
	
	void Update() {
		if (!Camera.main) return;
			
		float heading = Util.getDirection(transform, Camera.main.transform);
		transform.eulerAngles = new Vector3(0.0f, heading, 0.0f);
		
		float targetColorGoal;
		
		if (targetOverlayObj.renderer.enabled) 
			targetOverlayObj.renderer.material.mainTextureOffset += new Vector2(0.0f, -Time.deltaTime * 0.5f);	
		
		if (targetOverlayObj.renderer.enabled) {
			bombTargetObj.localScale = Vector3.Lerp(bombTargetObj.localScale, new Vector3(1.5f, 1.5f, 1.5f), Time.deltaTime);
			
			float arcHeading = Util.getDirection(transform, player.transform);
			bombArc.eulerAngles = new Vector3(0.0f, arcHeading, 0.0f);
			float arcScale = (transform.position - player.transform.position).magnitude;
			bombArc.localScale = new Vector3( 1.0f, arcScale, arcScale);
			bombArc.renderer.material.mainTextureOffset += new Vector2(0.0f, -Time.deltaTime * 1.5f);
			targetColorGoal = 1.0f;
			
		} else {
			bombTargetObj.localScale = Vector3.Lerp(bombTargetObj.localScale, new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime);	
			targetColorGoal = 0.0f;
		}
		
		Color targetColor = bombTargetObj.renderer.material.color;
		targetColor.a = Mathf.Lerp (targetColor.a, targetColorGoal, Time.deltaTime);
		bombTargetObj.renderer.material.color = targetColor;
		
	}

	
	public void reset() {

	}
	
	public void targetOn() {
		noBombObj.renderer.enabled = false;
		bombTargetObj.renderer.enabled = true;	
		targetOverlayObj.renderer.enabled = true;
		bombArc.renderer.enabled = true;
	}
	
	public void targetTooClose() {
		noBombObj.renderer.enabled = true;
		bombTargetObj.renderer.enabled = false;	
		targetOverlayObj.renderer.enabled = false;		
		bombArc.renderer.enabled = false;
	}
	
	public void targetOff() {
		noBombObj.renderer.enabled = false;
		//bombTargetObj.renderer.enabled = false;	
		targetOverlayObj.renderer.enabled = false;		
		bombArc.renderer.enabled = false;
	}	
}
