using UnityEngine;
using System.Collections;

public class ConfigJanitor : MonoBehaviour {
	
	EnemyAnimator enemyAnimator;
	EnemyController enemyController;
	
	public Transform janitorModel; 
	Transform janitorObj; 

	public RagDollController.RagDollData setUpData;
	
	void Awake() {
		Transform janitorProxy = transform.Find("JanitorProxy");
		Destroy(janitorProxy.gameObject);
		
		janitorObj = Instantiate(janitorModel, Vector3.zero, Quaternion.identity) as Transform;
		janitorObj.name = "JanitorModel";
		janitorObj.parent = transform;
		janitorObj.localPosition = Vector3.zero;
		janitorObj.localRotation = Quaternion.identity;
		RagDollController janitorRagDoll = janitorObj.gameObject.AddComponent<RagDollController>();

		enemyAnimator = GetComponent<EnemyAnimator>();
		if (!enemyAnimator) Debug.Log("ERROR: Can't Find Enemy Animator");
		
		enemyController = GetComponent<EnemyController>();
		if (!enemyController) Debug.Log("ERROR: Can't Find Enemy Controller");

		Transform janitorRoot = janitorObj.Find ("Janitor_Skeleton/Root");
		if (!janitorRoot) Debug.Log("ERROR: Can't Find janitor Root");
		janitorRoot.gameObject.AddComponent<ExplosionCatcher>();

		Transform janitorHead = janitorObj.Find ("Janitor_Skeleton/Root/UpperBody/Spine1/Spine2/Neck/Head");
		if (!janitorHead) Debug.Log("ERROR: Can't Find Janitor Head");

		Transform janitorUpperBody = janitorObj.Find ("Janitor_Skeleton/Root/UpperBody");
		if (!janitorUpperBody) Debug.Log("ERROR: Can't Find janitorUpperBody");

		Transform janitorRightHand = janitorObj.Find ("Janitor_Skeleton/Root/UpperBody/Spine1/Spine2/RightShoulder/RightElbow/RightWrist");
		if (!janitorRightHand) Debug.Log("ERROR: Can't Find Janitor Hand");	

		Transform janitorUpperTorso = janitorObj.Find ("Janitor_Skeleton/Root/UpperBody/Spine1/Spine2");
		if (!janitorUpperTorso) Debug.Log("ERROR: Can't Find Janitor Torso");	
		
		Transform janitorMop = janitorObj.Find ("Janitor_Geometry/Janitor_Mop");
		if (!janitorMop) Debug.Log("ERROR: Can't Find Janitor Mop");	

		Transform janitorBucket = janitorObj.Find ("Janitor_Geometry/Janitor_Bucket");
		if (!janitorBucket) Debug.Log("ERROR: Can't Find Janitor Bucket");	

		Transform[] accessories = new Transform[2];
		accessories[0] = janitorMop;
		accessories[1] = janitorBucket;

		enemyAnimator.setUp(janitorObj, janitorUpperBody);
		enemyController.setUp(janitorHead, janitorRagDoll, accessories);
		
		setUpData.layerName = "EnemyRagDoll";
		setUpData.rootTransform = janitorRoot;
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
				
		janitorRagDoll.setUp(setUpData);
//		
	}
}
