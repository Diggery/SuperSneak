using UnityEngine;
using System.Collections;

public class ViewShapeDrawer : MonoBehaviour {

	public Transform viewTarget;

	public bool showView;
	public int distanceCheck;
	Mesh viewMesh;
	public float fadeGoal;
	public float fadeAmount;
	public float innerRadius;
	public float outerRadius;
	public float angleGoal;
	public float angle;

 	Color viewColor;
 	Color goalColor;
	public Color patrolColor;
	public Color chaseColor;
	public Color investigateColor;
	public Color lookColor;
	public Color breakColor;
	public Color hitColor;
	
	EnemyController enemyController;

	void Start () {
	    viewMesh = GetComponent<MeshFilter>().mesh;
		enemyController = transform.root.GetComponent<EnemyController>();
	}
	
	
	void Update () {
		
		
		//check to see if the unit is looking around
		if (enemyController.looking) fadeGoal = 1.0f; else fadeGoal = 0.0f;
		fadeAmount = Mathf.Lerp(fadeAmount, fadeGoal, Time.deltaTime * 2);
		if (fadeAmount > 0.1) drawViewShape ();
		
		// check to see if they are in range
//		Transform currentPlayer = GameObject.FindWithTag("Player").transform;

//		if ((transform.position - currentPlayer.position).sqrMagnitude > distanceCheck) {
//			showView = false;
//		} else {
//			showView = true;
//		}
		
		float heading = Util.getDirection(Vector3.zero, viewTarget.localPosition);
		transform.localRotation = Quaternion.Euler(0, heading, 0);

	}
	
	void drawViewShape () {
		Color controlColor;
		
		if (showView) {
			controlColor = Color.white;
		} else {
			controlColor = Color.clear;
		}
		
		renderer.material.color = Color.Lerp(renderer.material.color, controlColor, Time.deltaTime * 5);
		if (renderer.material.color.a < 0.05f) return;
		
		EnemyAI.Activity currentActivity = enemyController.getCurrentActivity();
		switch (currentActivity) {
			case EnemyAI.Activity.Patrolling : 
				goalColor = patrolColor; 
				angleGoal = 60;
				break;
			case EnemyAI.Activity.Chasing : 
				goalColor = chaseColor; 
				angleGoal = 90;
				break;
			case EnemyAI.Activity.Investigating : 
				goalColor = investigateColor; 
				angleGoal = 120;
				break;
			case EnemyAI.Activity.Looking : 
				goalColor = lookColor; 
				angleGoal = 170;
				break;
			case EnemyAI.Activity.OnBreak : 
				goalColor = breakColor; 
				angleGoal = 30;
				break;
			default : 
				goalColor = patrolColor; 
				angleGoal = 45;
				break;
			}
		viewColor = Color.Lerp (viewColor, goalColor, Time.deltaTime * 3);
		angle = Mathf.Lerp (angle, angleGoal, Time.deltaTime * 3);
			    
	    Vector3[] vertices = viewMesh.vertices;
	    Color[] colors = viewMesh.colors;
		
		float segmentAngle = 2 * (angle / vertices.Length) * (Mathf.PI/180);
		float radianOffset = (90 + angle/2.0f - segmentAngle * 0.5f) * (Mathf.PI/180);
		
		Vector3 innerPos = new Vector3(0.0f, 0.1f, 0.0f);
		Vector3 outerPos = new Vector3(0.0f, 0.1f, 0.0f);
		int segments = vertices.Length / 2;
		

		for (int i = 0; i < segments; i++) {
			innerPos.x = innerRadius * fadeAmount * Mathf.Cos(-segmentAngle * i + radianOffset);
			innerPos.z = innerRadius * fadeAmount * Mathf.Sin(-segmentAngle * i + radianOffset);
			outerPos.x = outerRadius * fadeAmount * Mathf.Cos(-segmentAngle * i + radianOffset);
			outerPos.z = outerRadius * fadeAmount * Mathf.Sin(-segmentAngle * i + radianOffset);
			
			colors[i] = viewColor * fadeAmount;
			colors[i+segments] = viewColor * fadeAmount;
			
			RaycastHit hit;
			
			Vector3 worldInnerPos = transform.TransformPoint(innerPos);
			Vector3 worldOuterPos = transform.TransformPoint(outerPos);
			if (Physics.Linecast (worldInnerPos, worldOuterPos, out hit)) {
				outerPos = transform.InverseTransformPoint(hit.point);
				if (hit.transform.tag == "Player") {
					colors[i+segments] = hitColor;
					enemyController.spotPlayer(hit.transform);
				}
			}
			
			vertices[i] = innerPos;
			vertices[i+segments] = outerPos;		
	    }
	
		viewMesh.vertices = vertices;
		viewMesh.colors = colors;	
		viewMesh.RecalculateBounds();
	}
	
}	