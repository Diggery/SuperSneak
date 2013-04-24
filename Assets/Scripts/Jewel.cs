using UnityEngine;
using System.Collections;

public class Jewel : MonoBehaviour {

	void Start () {
		int colorSelection = Random.Range(0,3);
		Color color;
		switch (colorSelection) {
		case 0 :
			color = Color.red;
			break;
		case 1 :
			color = Color.green;
			break;
		case 2 :
			color = Color.blue;
			break;
		default :
			color = Color.cyan;
			break;
		}
		
		
		renderer.material.color = color;
	}

}
