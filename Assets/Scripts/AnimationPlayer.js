#pragma strict

var animToPlay		: String;

function Start () {

	animation[animToPlay].wrapMode = WrapMode.Loop;
	animation.Play(animToPlay);
}

function Update () {

}