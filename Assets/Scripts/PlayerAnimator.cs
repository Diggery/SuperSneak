using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
	
	public enum AnimState { Idle, Sneaking, Running, Stunned, Dead, OpeningCrate, HackingServer } 
	public AnimState currentState = AnimState.Idle;

	float lookDirection;
	Vector3 headingOverride;
	
	Transform playerModel;
	PlayerController playerController;

	public void SetUp (Transform thiefObj, Transform upperBody) {
		playerModel = thiefObj;
		playerModel.animation.Stop();
		playerController = GetComponent<PlayerController>();

		playerModel.animation["Idle01"].wrapMode = WrapMode.Once;
		playerModel.animation["Idle01"].layer = 1;

		playerModel.animation["OpenCrate"].wrapMode = WrapMode.Once;
		playerModel.animation["OpenCrate"].layer = 1;
		playerModel.animation["HackServer"].wrapMode = WrapMode.Once;
		playerModel.animation["HackServer"].layer = 1;

		playerModel.animation["Walk"].wrapMode = WrapMode.Loop;
		playerModel.animation["Walk"].layer = 1;	

		playerModel.animation["Run"].wrapMode = WrapMode.Loop;
		playerModel.animation["Run"].layer = 1;	
		AnimationEvent runningFootstepEvent = new AnimationEvent();
		runningFootstepEvent.functionName = "playSound";
		runningFootstepEvent.stringParameter = "runningFootStep";
		runningFootstepEvent.time = 0.0f;
		playerModel.animation["Run"].clip.AddEvent(runningFootstepEvent);

		playerModel.animation["TakeOutBomb"].wrapMode = WrapMode.ClampForever;
		playerModel.animation["TakeOutBomb"].layer = 2;	
		playerModel.animation["TakeOutBomb"].AddMixingTransform(upperBody);
		AnimationEvent takeOutBombEvent = new AnimationEvent();
		takeOutBombEvent.functionName = "readyBomb";
		takeOutBombEvent.time = 0.5f;
		playerModel.animation["TakeOutBomb"].clip.AddEvent(takeOutBombEvent);	

		playerModel.animation["ShootWeapon"].wrapMode = WrapMode.ClampForever;
		playerModel.animation["ShootWeapon"].layer = 2;	
		playerModel.animation["ShootWeapon"].AddMixingTransform(upperBody);
				
		playerModel.animation["PutAwayBomb"].wrapMode = WrapMode.Once;
		playerModel.animation["PutAwayBomb"].layer = 2;	
		playerModel.animation["PutAwayBomb"].AddMixingTransform(upperBody);
		AnimationEvent putAwayBombEvent = new AnimationEvent();
		putAwayBombEvent.functionName = "putAwayBomb";
		putAwayBombEvent.time = 0.25f;
		playerModel.animation["PutAwayBomb"].clip.AddEvent(putAwayBombEvent);	
	
		playerModel.animation["Throw"].wrapMode = WrapMode.Once;
		playerModel.animation["Throw"].layer = 2;	
		playerModel.animation["Throw"].AddMixingTransform(upperBody);
		AnimationEvent ThrowBombEvent = new AnimationEvent();
		ThrowBombEvent.functionName = "throwBomb";
		ThrowBombEvent.time = 0.25f;
		playerModel.animation["Throw"].clip.AddEvent(ThrowBombEvent);	
	}
	
	void Update () {

		if (currentState == AnimState.Dead) return;
		
		if (currentState == AnimState.Stunned) { 

		} else if (currentState == AnimState.OpeningCrate) {
			if (!playerModel.animation.IsPlaying("OpenCrate")) selectState();
		} else if (currentState == AnimState.HackingServer) {
			if (!playerModel.animation.IsPlaying("HackServer")) selectState();
		} else {
			selectState();
		}
		
		
		switch (currentState) {
	
		case AnimState.Idle :
			if (!playerModel.animation.IsPlaying("Idle01")) {
				playerModel.animation.CrossFade("Idle01", 0.25f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Sneaking :
			if (!playerModel.animation.IsPlaying("Walk")) {
				playerModel.animation.CrossFade("Walk", 0.5f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Running :
			if (!playerModel.animation.IsPlaying("Run")) {
				playerModel.animation.CrossFade("Run", 0.5f, PlayMode.StopSameLayer);
			}
			break;
		case AnimState.Dead :
			break;		
			
		}
		
		float animSpeed = playerController.currentSpeed * 0.3f;
		
		if (playerController.shootInputOn) {
			float shootHeading = Util.getDirection(-playerController.currentShootInput);	
			float moveHeading = Util.getDirection(playerController.currentDirection);
			
			if (Mathf.Abs(shootHeading - moveHeading) > 90) animSpeed = -animSpeed;
		
		}
		
		playerModel.animation["Run"].speed = animSpeed;
		playerModel.animation["Walk"].speed = animSpeed;
		
		if (currentState != AnimState.Dead && currentState != AnimState.Stunned) {
			Quaternion heading;
			if (currentState == AnimState.OpeningCrate || currentState == AnimState.HackingServer) {
				heading = Quaternion.Euler(0, Util.getDirection(headingOverride) - 180, 0);
			} else {
				// if the player is trying to shoot rotate towards that, otherise head the direction of running
				if (playerController.shootInputOn) {
					heading = Quaternion.Euler(0, Util.getDirection(-playerController.currentShootInput), 0);
				} else {
					heading = Quaternion.Euler(0, Util.getDirection(playerController.currentDirection), 0);
				}
			}
			
			transform.rotation = Quaternion.Lerp(transform.rotation, heading, Time.deltaTime * 8.0f);
		}
	}
	
	void selectState() {
		if (currentState != AnimState.Idle && playerController.currentSpeed < 0.1f) playIdleAnim();
		if (playerController.moveInputOn) {
			if (playerController.currentSpeed > 0.1f && 
				playerController.currentSpeed < 3.0f ||
				playerController.shootInputOn) playSneakAnim();
			if (playerController.currentSpeed > 3.0f) playRunAnim();
		}
	}
	
	public bool isStopped() {
		if (currentState == AnimState.OpeningCrate ||
			currentState == AnimState.HackingServer ||
			currentState == AnimState.Dead ||
			currentState == AnimState.Stunned) {
			return true;
		} else {
			return false;
		}
	}
	
	void playIdleAnim() {
		currentState = AnimState.Idle;	
	}

	void playRunAnim() {
		currentState = AnimState.Running;	
	}
	
	void playSneakAnim() {
		currentState = AnimState.Sneaking;	
	}
	
	public void die(Vector3 attackOrigin) {
		if (currentState == AnimState.Dead) return;	
		currentState = AnimState.Dead;	
		playerModel.animation.Stop();

	}
	
	public float playReadyBombAnim() {
		playerModel.animation.CrossFade("TakeOutBomb", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["TakeOutBomb"].length;
	}
	
	public float playThrowBombAnim() {
		playerModel.animation.CrossFade("Throw", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["Throw"].length;
	}
	
	public float playPutAwayItemAnim() {
		playerModel.animation.CrossFade("PutAwayBomb", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["Throw"].length;
	}

	public float playShootWeaponAnim() {
		playerModel.animation.CrossFade("ShootWeapon", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["ShootWeapon"].length;
	}
	
	public float playOpenCrateAnim(Vector3 cratePos) {
		headingOverride = transform.position - cratePos;
		currentState = AnimState.OpeningCrate;
		playerModel.animation.CrossFade("OpenCrate", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["OpenCrate"].length;
	}
	public float playHackServerAnim(Vector3 serverPos) {
		headingOverride = transform.position - serverPos;
		currentState = AnimState.HackingServer;
		playerModel.animation.CrossFade("HackServer", 0.05f, PlayMode.StopSameLayer);
		return playerModel.animation["HackServer"].length;
	}	
}
