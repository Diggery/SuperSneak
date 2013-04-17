using UnityEngine;
using System.Collections;

public class PlayerConfig : MonoBehaviour {
	
	PlayerController playerController;
	PlayerAnimator playerAnimator;
	BombThrower playerBombThrower;
	public Transform bombTargetPrefab;
	
	public Transform playerPrefab; 
	Transform thiefObj; 
	
	public RagDollController.RagDollData setUpData;
	
	void Awake() {
		Transform playerProxy = transform.Find("ThiefProxy");
		Destroy(playerProxy.gameObject);
		
		thiefObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as Transform;
		thiefObj.name = "Thief";
		thiefObj.parent = transform;
		thiefObj.localPosition = Vector3.zero;
		thiefObj.localRotation = Quaternion.identity;
		RagDollController playerRagDoll = thiefObj.gameObject.AddComponent<RagDollController>();
		thiefObj.gameObject.AddComponent<AnimationEventListener>();

		playerAnimator = GetComponent<PlayerAnimator>();
		if (!playerAnimator) Debug.Log("ERROR: Can't Find Player Animator");
		
		playerController = GetComponent<PlayerController>();
		if (!playerController) Debug.Log("ERROR: Can't Find Player Controller");
		
		playerBombThrower = GetComponent<BombThrower>();
		if (!playerBombThrower) Debug.Log("ERROR: Can't Find Bomb Thrower");

		Transform thiefHead = thiefObj.Find ("Thief_Skeleton/Root/UpperBody/Spine1/Spine2/Neck/Head");
		if (!thiefHead) Debug.Log("ERROR: Can't Find Player Head");

		Transform thiefRightHand = thiefObj.Find ("Thief_Skeleton/Root/UpperBody/Spine1/Spine2/RightShoulder/RightElbow/RightWrist");
		if (!thiefRightHand) Debug.Log("ERROR: Can't Find Player Hand");	
		
		Transform thiefUpperBody = thiefObj.Find ("Thief_Skeleton/Root/UpperBody");
		if (!thiefUpperBody) Debug.Log("ERROR: Can't Find Player UpperBody");	

		Transform thiefUpperTorso = thiefObj.Find ("Thief_Skeleton/Root/UpperBody/Spine1/Spine2");
		if (!thiefUpperTorso) Debug.Log("ERROR: Can't Find Player Torso");	

		
		playerAnimator.setUp(thiefObj, thiefUpperBody);
		playerController.setUp(thiefHead, playerRagDoll, playerBombThrower);
		playerBombThrower.setUp(thiefRightHand, playerAnimator, playerController, bombTargetPrefab);
		
		setUpData.layerName = "PlayerRagDoll";
		setUpData.rootTransform = thiefObj.Find("Thief_Skeleton/Root");
		setUpData.root.collision.center = new Vector3(0.0f, 0.0f, 0.0f);
		setUpData.root.collision.size = new Vector3(0.296f, 0.321f, 0.479f);
		
		setUpData.leftHip.collision.center = new Vector3(-0.221f, 0.000f, 0.000f);
		setUpData.leftHip.collision.radius = 0.133f;
		setUpData.leftHip.collision.height = 0.443f;
		setUpData.leftHip.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.leftHip.joint.swingAxis = new Vector3(1.0f, 0.0f, 0.0f);
		
		setUpData.leftKnee.collision.center = new Vector3(-0.35f, 0.000f, 0.000f);
		setUpData.leftKnee.collision.radius = 0.119f;
		setUpData.leftKnee.collision.height = 0.476f;
		setUpData.leftKnee.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.leftKnee.joint.swingAxis = new Vector3(1.0f, 0.0f, 0.0f);	
		
		setUpData.rightHip.collision.center = new Vector3(0.221f, 0.000f, 0.000f);
		setUpData.rightHip.collision.radius = 0.133f;
		setUpData.rightHip.collision.height = 0.443f;
		setUpData.rightHip.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.rightHip.joint.swingAxis = new Vector3(-1.0f, 0.0f, 0.0f);
		
		setUpData.rightKnee.collision.center = new Vector3(0.23f, 0.000f, 0.000f);
		setUpData.rightKnee.collision.radius = 0.119f;
		setUpData.rightKnee.collision.height = 0.476f;
		setUpData.rightKnee.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.rightKnee.joint.swingAxis = new Vector3(-1.0f, 0.0f, 0.0f);	
		
		setUpData.spine.collision.center = new Vector3(-0.1487f, -0.001f, -0.000f);
		setUpData.spine.collision.size = new Vector3(0.296f, 0.321f, 0.479f);
		setUpData.spine.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.spine.joint.swingAxis = new Vector3(-1.0f, 0.0f, 0.0f);	
		
		setUpData.head.collision.center = new Vector3(0.000f, 0.120f, 0.000f);
		setUpData.head.collision.radius = 0.120f;
		setUpData.head.joint.axis = new Vector3(1.0f, 0.0f, 0.0f);
		setUpData.head.joint.swingAxis = new Vector3(0.0f, 1.0f, 0.0f);	
		
		setUpData.leftShoulder.collision.center = new Vector3(0.172f, 0.000f, 0.000f);
		setUpData.leftShoulder.collision.radius = 0.086f;
		setUpData.leftShoulder.collision.height = 0.345f;
		setUpData.leftShoulder.joint.axis = new Vector3(0.0f, 0.0f, -1.0f);
		setUpData.leftShoulder.joint.swingAxis = new Vector3(0.0f, -1.0f, 0.0f);
				
		setUpData.leftElbow.collision.center = new Vector3(0.285f, 0.000f, 0.000f);
		setUpData.leftElbow.collision.radius = 0.114f;
		setUpData.leftElbow.collision.height = 0.570f;
		setUpData.leftElbow.joint.axis = new Vector3(0.0f, -1.0f, 0.0f);
		setUpData.leftElbow.joint.swingAxis = new Vector3(0.0f, 0.0f, -1.0f);
				
		setUpData.rightShoulder.collision.center = new Vector3(-0.174f, 0.000f, 0.000f);
		setUpData.rightShoulder.collision.radius = 0.086f;
		setUpData.rightShoulder.collision.height = 0.345f;
		setUpData.rightShoulder.joint.axis = new Vector3(0.0f, 0.0f, 1.0f);
		setUpData.rightShoulder.joint.swingAxis = new Vector3(0.0f, 1.0f, 0.0f);
				
		setUpData.rightElbow.collision.center = new Vector3(-0.285f, 0.000f, 0.000f);
		setUpData.rightElbow.collision.radius = 0.114f;
		setUpData.rightElbow.collision.height = 0.570f;
		setUpData.rightElbow.joint.axis = new Vector3(0.0f, 1.0f, 0.0f);
		setUpData.rightElbow.joint.swingAxis = new Vector3(0.0f, 0.0f, 1.0f);
				
		playerRagDoll.setUp(setUpData);
		
	}
}
