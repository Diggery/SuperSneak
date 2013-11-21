using UnityEngine;
using System.Collections;

static public class Curve : object {

	 static public Vector3 GetBezierPoint(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t) {

		Vector3 ap1 = Vector3.Lerp(p1, p2, t);
		Vector3 ap2 = Vector3.Lerp(p2, p3, t);
		Vector3 ap3 = Vector3.Lerp(p3, p4, t);

		Vector3 bp1 = Vector3.Lerp(ap1, ap2, t);
		Vector3 bp2 = Vector3.Lerp(ap2, ap3, t);

        Vector3 p = Vector3.Lerp(bp1, bp2, t);

        return p;
    }
	
	static public Vector3 GetHermitePoint(Vector3 prevPoint, Vector3 startPoint, Vector3 endPoint, Vector3 nextPoint, float time, float tension) {
		float time2 = time * time;
		float time3 = time2 * time;

		Vector3 P0 = prevPoint;
		Vector3 P1 = startPoint;
		Vector3 P2 = endPoint;
		Vector3 P3 = nextPoint;

		//float tension = 0.25f;	// 0.5 equivale a catmull-rom

		Vector3 T1 = tension * (P2 - P0);
		Vector3 T2 = tension * (P3 - P1);

		float Blend1 = 2 * time3 - 3 * time2 + 1;
		float Blend2 = -2 * time3 + 3 * time2;
		float Blend3 = time3 - 2 * time2 + time;
		float Blend4 = time3 - time2;

		return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
	}
	
}
