using UnityEngine;
using System.Collections;



	


static public class Util : object {
    
    static public string Ellipsize(string text, int maxChars) {
        if (text.Length > maxChars) {
            return text.Substring(0, maxChars) + "...";
        }
        return text;
    }

    static public float getDirection(Transform transform1, Transform transform2) {
		Vector2 currentPos = new Vector2(transform1.position.x, transform1.position.z);
		Vector2 targetPos = new Vector2(transform2.position.x, transform2.position.z);
		float direction = Vector2.Angle(Vector2.up, (targetPos-currentPos));
		if (currentPos.x > targetPos.x) {
			direction = 360 - direction;
		}
		return direction;
    }
    static public float getDirection(Vector3 vec3Point1, Vector3 vec3Point2) {
		Vector2 point1 = new Vector2(vec3Point1.x, vec3Point1.z);
		Vector2 point2 = new Vector2(vec3Point2.x, vec3Point2.z);
		float direction = Vector2.Angle(Vector2.up, (point2-point1));
		if (point1.x > point2.x) {
			direction = 360 - direction;
		}
		return direction;
    }
    static public float getDirection(Vector2 point1, Vector2 point2) {
		float direction = Vector2.Angle(Vector2.up, (point2-point1));
		if (point1.x > point2.x) {
			direction = 360 - direction;
		}
		return direction;
    }
    static public float getDirection(Vector3 vector ) {
		float direction = Vector2.Angle(Vector2.up, new Vector2(vector.x, vector.z));
		if (vector.x < 0) {
			direction = 360 - direction;
		}
		return direction;
    }

	static public Vector3 BallisticVel(Vector3 launchPoint, Vector3 targetPoint) {
		Vector3 dir = targetPoint - launchPoint; // get target direction
		dir.y = 0;  // retain only the horizontal direction
		float dist = dir.magnitude ;  // get horizontal distance
		dir.y = dist;  // set elevation to 45 degrees
		float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
		return vel * dir.normalized;  // returns Vector3 velocity
	}    
    
 	static public string secondsToTime(int allSeconds) {
 	 	int hoursInt = Mathf.RoundToInt(Mathf.Floor(allSeconds / 3600));
		int minutesInt = Mathf.RoundToInt(Mathf.Floor((allSeconds - (hoursInt * 3600))/60));
	 	string minutesString = minutesInt + "";
	 	string hoursString;
		if (hoursInt > 0) {
			hoursString = hoursInt + ":";
			while (minutesString.Length < 2) {minutesString = '0' + minutesString;}
		} else {
			hoursString = "";
		}

		allSeconds -= ((hoursInt * 3600) + (minutesInt * 60));
		string secondsString = allSeconds + "";
		while (secondsString.Length < 2) {secondsString = "0" + secondsString;}

		return hoursString + minutesString + ":" + secondsString;
 	}
 	
}