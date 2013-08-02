using UnityEngine;
using System.Collections;

public class IKLegController : MonoBehaviour {
	
	float hipLength;
	float kneeLength;
	
	public Transform hipJoint;
	public Transform kneeJoint;
	public Transform footJoint;
	public Transform IKEnd;
	public Transform goal;
	public bool rightFoot;


	void Start () {
		hipLength = kneeJoint.localPosition.magnitude;
		kneeLength = footJoint.localPosition.magnitude;	
	}
	
	void Update () {
		float hipOffset = rightFoot ? 20 : -20;

		Vector3 hipAngle = Quaternion.AngleAxis(hipOffset, Vector3.up) * Vector3.forward;
		
		transform.LookAt(goal, hipAngle);
		IKEnd.position = goal.position;
		Vector2 targetPos = new Vector2(IKEnd.localPosition.z, -IKEnd.localPosition.y);
		
		float angle1 = 0.0f;
		float angle2 = 0.0f;

		
		IKSolver.CalcIK_2D(out angle1, out angle2, true, hipLength, kneeLength, targetPos.x, targetPos.y);
		
		hipJoint.localRotation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.right);
		kneeJoint.localRotation = Quaternion.AngleAxis(angle2 * Mathf.Rad2Deg, Vector3.right);
	}
}
