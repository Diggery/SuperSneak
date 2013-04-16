using UnityEngine;
using System.Collections;

public class GuardConfig : MonoBehaviour {
	
	EnemyAnimator enemyAnimator;
	EnemyController enemyController;
	
	public Transform guardPrefab; 
	Transform guardObj; 

	public RagDollController.RagDollData setUpData;
	
	void Awake() {
		Transform guardProxy = transform.Find("GuardProxy");
		Destroy(guardProxy.gameObject);
		
		guardObj = Instantiate(guardPrefab, Vector3.zero, Quaternion.identity) as Transform;
		guardObj.name = "GuardModel";
		guardObj.parent = transform;
		guardObj.localPosition = Vector3.zero;
		guardObj.localRotation = Quaternion.identity;
		RagDollController guardRagDoll = guardObj.gameObject.AddComponent<RagDollController>();

		enemyAnimator = GetComponent<EnemyAnimator>();
		if (!enemyAnimator) Debug.Log("ERROR: Can't Find Enemy Animator");
		
		enemyController = GetComponent<EnemyController>();
		if (!enemyController) Debug.Log("ERROR: Can't Find Enemy Controller");

		Transform guardRoot = guardObj.Find ("Guard_Skeleton/Root");
		if (!guardRoot) Debug.Log("ERROR: Can't Find Guard Root");
		guardRoot.gameObject.AddComponent<ExplosionCatcher>();

		Transform guardHead = guardObj.Find ("Guard_Skeleton/Root/UpperBody/Spine1/Spine2/Neck/Head");
		if (!guardHead) Debug.Log("ERROR: Can't Find Guard Head");

		Transform guardGun = guardObj.Find ("Guard_Geometry/Guard_Gun");
		if (!guardGun) Debug.Log("ERROR: Can't Find Guard Gun");	

		//Transform guardGunHolster = guardObj.Find ("Guard_Geometry/Guard_GunHolster");
		//if (!guardGun) Debug.Log("ERROR: Can't Find Guard Gun Holster");	

		Transform guardRightHand = guardObj.Find ("Guard_Skeleton/Root/UpperBody/Spine1/Spine2/RightShoulder/RightElbow/RightWrist");
		if (!guardRightHand) Debug.Log("ERROR: Can't Find Guard Hand");	

		Transform guardUpperTorso = guardObj.Find ("Guard_Skeleton/Root/UpperBody/Spine1/Spine2");
		if (!guardUpperTorso) Debug.Log("ERROR: Can't Find Guard Torso");	

		Transform guardBadge = guardObj.Find ("Guard_Geometry/Guard_Badge");
		if (!guardBadge) Debug.Log("ERROR: Can't Find Guard Badge");	
		
		Transform guardHat = guardObj.Find ("Guard_Geometry/Guard_Hat");
		if (!guardHat) Debug.Log("ERROR: Can't Find Guard Hat");
		
		Transform[] accessories = new Transform[2];
		accessories[0] = guardBadge;
		accessories[1] = guardHat;
		
		guardGun.parent = guardRightHand;
		guardBadge.parent = guardUpperTorso;
		guardHat.parent = guardHead;
		
		enemyAnimator.setUp(guardObj);
		enemyController.setUp(guardHead, guardGun, guardRagDoll, accessories);
		
		setUpData.layerName = "EnemyRagDoll";
		setUpData.rootTransform = guardRoot;
		setUpData.root.collision.center = new Vector3(0.000f, 0.187f, -0.030f);
		setUpData.root.collision.size = new Vector3(0.507f, 0.575f, 0.305f);
		
		setUpData.leftHip.collision.center = new Vector3(-0.221f, 0.000f, 0.000f);
		setUpData.leftHip.collision.radius = 0.142f;
		setUpData.leftHip.collision.height = 0.475f;
		setUpData.leftHip.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.leftHip.joint.swingAxis = new Vector3(0.0f, 0.0f, 1.0f);
		
		setUpData.leftKnee.collision.center = new Vector3(-0.35f, 0.000f, 0.000f);
		setUpData.leftKnee.collision.radius = 0.114f;
		setUpData.leftKnee.collision.height = 0.459f;
		setUpData.leftKnee.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.leftKnee.joint.swingAxis = new Vector3(0.0f, -1.0f, 0.0f);	
		
		setUpData.rightHip.collision.center = new Vector3(0.237f, 0.000f, 0.000f);
		setUpData.rightHip.collision.radius = 0.142f;
		setUpData.rightHip.collision.height = 0.475f;
		setUpData.rightHip.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.rightHip.joint.swingAxis = new Vector3(0.0f, 0.0f, -1.0f);
		
		setUpData.rightKnee.collision.center = new Vector3(0.35f, 0.000f, 0.000f);
		setUpData.rightKnee.collision.radius = 0.114f;
		setUpData.rightKnee.collision.height = 0.459f;
		setUpData.rightKnee.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.rightKnee.joint.swingAxis = new Vector3(0.0f, 1.0f, 0.0f);	
		
		setUpData.spine.collision.center = new Vector3(-0.160f, -0.005f, -0.000f);
		setUpData.spine.collision.size = new Vector3(0.321f, 0.304f, 0.507f);
		setUpData.spine.joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		setUpData.spine.joint.swingAxis = new Vector3(0.0f, -1.0f, 0.0f);	
		
		setUpData.head.collision.center = new Vector3(0.000f, 0.126f, 0.000f);
		setUpData.head.collision.radius = 0.126f;
		setUpData.head.joint.axis = new Vector3(1.0f, 0.0f, 0.0f);
		setUpData.head.joint.swingAxis = new Vector3(0.0f, 0.0f, 1.0f);	
		
		setUpData.leftShoulder.collision.center = new Vector3(0.174f, 0.000f, 0.000f);
		setUpData.leftShoulder.collision.radius = 0.087f;
		setUpData.leftShoulder.collision.height = 0.343f;
		setUpData.leftShoulder.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.leftShoulder.joint.swingAxis = new Vector3(0.0f, 0.0f, 1.0f);
				
		setUpData.leftElbow.collision.center = new Vector3(0.270f, 0.000f, 0.000f);
		setUpData.leftElbow.collision.radius = 0.108f;
		setUpData.leftElbow.collision.height = 0.542f;
		setUpData.leftElbow.joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		setUpData.leftElbow.joint.swingAxis = new Vector3(0.0f, -1.0f, 0.0f);
				
		setUpData.rightShoulder.collision.center = new Vector3(-0.174f, 0.000f, 0.000f);
		setUpData.rightShoulder.collision.radius = 0.087f;
		setUpData.rightShoulder.collision.height = 0.343f;
		setUpData.rightShoulder.joint.axis = new Vector3(0.0f, 1.0f, 0.0f);
		setUpData.rightShoulder.joint.swingAxis = new Vector3(0.0f, 0.0f, -1.0f);
				
		setUpData.rightElbow.collision.center = new Vector3(-0.270f, 0.000f, 0.000f);
		setUpData.rightElbow.collision.radius = 0.108f;
		setUpData.rightElbow.collision.height = 0.542f;
		setUpData.rightElbow.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.rightElbow.joint.swingAxis = new Vector3(0.0f, 1.0f, 0.0f);
				
		guardRagDoll.setUp(setUpData);
		
	}
}
