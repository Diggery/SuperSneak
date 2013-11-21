using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConvexHull : MonoBehaviour {
	
	public struct Segment {
		public Vector3 startPoint;
		public Vector3 endPoint;

		public bool Contains(Vector3 point) {
			if (startPoint.Equals(point) || endPoint.Equals(point)) {
				return true;
			}
			return false;
		}

		public bool EndsAt(Vector3 point) {
			if (endPoint.Equals(point)) {
				return true;
			}
			return false;
		}

		public bool StartsAt(Vector3 point) {
			if (startPoint.Equals(point)) {
				return true;
			}
			return false;
		}
		
		public bool isLeft(Vector3 point) {
			float D = 0;
			D = ((endPoint.x * point.z) - (endPoint.z * point.x)) - (startPoint.x *(point.z - endPoint.z)) + (startPoint.z * (point.x - endPoint.x));
			if (D <= 0) return false;
			return true;
		}		
	}

	public void DrawHull(List<Transform> pointsList) {
		List<Segment> edges = ComputeHull(pointsList);
		List<Vector3> line = MakeLine(edges);
		GetComponent<LineDrawer>().DrawLine(line);
	} 
	
	List<Segment> InitSegments(List<Transform> points) {
		List<Segment> newSegments = new List<Segment>();
		for (int i = 0; i < points.Count; i++) {
			for (int j = 0; j < points.Count; j++) {
				if (i != j) {
					Segment op = new Segment();
                    Vector3 p1 = points[i].position;
                    Vector3 p2 = points[j].position;
                    op.startPoint = p1;
                    op.endPoint = p2;

                    newSegments.Add(op);
                }
            }
        }
		return newSegments;
    }
	
	List<Segment> ComputeHull(List<Transform> points) {
		List<Segment> processedSegments = InitSegments(points);
		List<Vector3> processedPoints = new List<Vector3>();
		int i = 0;
		int j = 0;
		for (i = 0; i < processedSegments.Count; ) {
			
			foreach (Transform point in points) processedPoints.Add(point.position);

			for (j = 0; j < processedPoints.Count; ) {
       			if(processedSegments[i].Contains(processedPoints[j])) {
          			processedPoints.Remove(processedPoints[j]);
					j = 0;
					continue;
				}
         		j++;
			}

			if(!isEdge(processedPoints, processedSegments[i])) {
				processedSegments.Remove(processedSegments[i]);
				i = 0;
				continue;
			} else {
				i++;
			} 
		}
		return processedSegments;
	}
	
	bool isEdge(List<Vector3> processedPoints, Segment edge) {
		for(int k = 0; k < processedPoints.Count; k++) {  
			if(edge.isLeft(processedPoints[k])) return false;
		}
		return true;
	} 
	
	List<Vector3> MakeLine(List<Segment> perimeter) {
		List<Vector3> line = new List<Vector3>();
		line.Add(perimeter[0].startPoint);
		line.Add(perimeter[0].endPoint);
		for(int i = 1; i < perimeter.Count; i++) {
			for(int j = 0; j < perimeter.Count; j++) {
				if (perimeter[j].StartsAt(line[i])) {
					line.Add(perimeter[j].endPoint);
				}
			}
		}	
		return line;
	}
	
	List<Vector3> MakeCurve(List<Vector3> points) {
		float knots = 10.0f;
		List<Vector3> output = new List<Vector3>();
		for(int i = 0; i < points.Count - 1; i++) {
			int prev = i - 1;
			if (prev < 0) prev = points.Count - 2;
			int next = i + 2;
			if (next > points.Count - 1) next = 1;
			for(int knot = 0; knot < knots; knot++) {
				float time = (float)knot/(float)knots;
				output.Add(Curve.GetHermitePoint(points[prev], points[i], points[i + 1], points[next], time, 0.25f));
			}
		}
		return output;
	}
			

	
	void DrawLines(List<Segment> drawingSegments) {
		for(int i = 0; i < drawingSegments.Count; i++) {  
			Debug.DrawLine(drawingSegments[i].startPoint, drawingSegments[i].endPoint, Color.red);	
		}
	}
}
