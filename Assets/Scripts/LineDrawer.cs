using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LineDrawer : MonoBehaviour {
	
	public Transform[] waypoints;
	public int knots;
	public float lineStretch;
	public float lineWidth;
	public float tension;
	Mesh mesh;
	bool loop;
	
	public Material lineMaterial;
	public Color startColor = Color.white;
	public Color endColor = Color.white;
	
	
	void SetUp () {
		GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cube);
		line.name = "Line";
		line.renderer.material = lineMaterial;
		
		mesh = line.GetComponent<MeshFilter>().mesh;
		if (!mesh) print ("No Mesh");
		mesh.Clear();
	}
		


    public void DrawLine(List<Vector3> nodes) {
		if (!mesh) SetUp();
        mesh.Clear();
		
		if (nodes.Count < 1) return;
		
		loop = (nodes[0] == nodes[nodes.Count - 1]) ? true : false;	
				
		for (int segment = 0; segment < nodes.Count - 1; segment++) {
			
			Vector3 p1, p2, p3, p4;
			
			if (segment == 0) {
				if (loop) {
					p1 = nodes[nodes.Count - 2];	
				} else {
					p1 = nodes[segment] - nodes[segment + 1];
				}
			} else {
				p1 = nodes[segment - 1];	
			}

			p2 = nodes[segment];
			p3 = nodes[segment + 1];
			
			if (segment + 2 >= nodes.Count) {
				if (loop) {
					p4 = nodes[1];
				} else {
					p4 = nodes[segment + 1];
				}
			} else {
				p4 = nodes[segment + 2];
			}
			
			p1.y += 0.1f;
			p2.y += 0.1f;
			p3.y += 0.1f;
			p4.y += 0.1f;
			
			Vector3 lastPoint = nodes[segment];
			
			for (int i = 1; i < knots + 1; i++) {
				Vector3 newPoint = Curve.GetHermitePoint(p1, p2, p3, p4, i/(float)knots, tension);
				
				bool isStart = (i == 1 && segment == 0) ? true : false;	
				
				bool isEnd =  (i == knots  && segment == nodes.Count - 2) ? true : false;
				
				AddLine(mesh, AddQuad(lastPoint, newPoint, lineWidth), isStart, isEnd);
				lastPoint = newPoint;
			}
		}
		setColorsAndUVs(mesh);
		mesh.RecalculateBounds();
    }

	void AddLine(Mesh mesh, Vector3[] quad, bool isStart, bool isEnd) {
						
		int numOfVerts = mesh.vertices.Length;
		
		Vector3[] verts = mesh.vertices;
		verts = resizeVertices(verts, 4);
		
		Vector2[] uvs = mesh.uv;
		uvs = resizeUVs(uvs, 4);
		
		Color[] colors = mesh.colors;
		colors = resizeColors(colors, 4);
		
		
		if (!isStart) {
			quad[0] = verts[numOfVerts - 2];
			quad[1] = verts[numOfVerts - 1];
		}
		
	
		verts[numOfVerts] = quad[0];
		
		verts[numOfVerts + 1] = quad[1];
		
		verts[numOfVerts + 2] = quad[2];
		
		verts[numOfVerts + 3] = quad[3];
		

		
		int numOfTris = mesh.triangles.Length;
		
		int[] tris = mesh.triangles;
		tris = resizeTraingles(tris, 6);
	
		tris[numOfTris] = numOfVerts + 2;
		tris[numOfTris+1] = numOfVerts + 1;
		tris[numOfTris+2] = numOfVerts;
		
		tris[numOfTris+3] = numOfVerts + 2;
		tris[numOfTris+4] = numOfVerts + 3;
		tris[numOfTris+5] = numOfVerts + 1;

		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.uv = uvs;
		mesh.colors = colors;
	}
	
	Vector3[] AddQuad(Vector3 start, Vector3 end, float width) {
				
		Vector3[] quad = new Vector3[4];

		Vector3 offset = (start - end).normalized * width;

		offset = Quaternion.AngleAxis(90, Vector3.up) * offset;
		
	
		quad[0] = (start + offset);
		quad[1] = (start - offset);
		
		quad[2] = (end + offset);
		quad[3] = (end - offset);
		
		return quad;
	}
	

	Vector3[] resizeVertices(Vector3[] oldVerts, int numAdded) {
		//print ("resizing verts by " + numAdded);
		Vector3[] newVerts = new Vector3[oldVerts.Length + numAdded];
		for(int i = 0; i < oldVerts.Length; i++) newVerts[i] = oldVerts[i];
		return newVerts;
	}
	
	Vector2[] resizeUVs(Vector2[] oldUVs, int numAdded) {
		//print ("resizing uvs by " + numAdded);
		Vector2[] newUVs = new Vector2[oldUVs.Length + numAdded];
		for(int i = 0; i < oldUVs.Length; i++) newUVs[i] = oldUVs[i];
		return newUVs;
	}
	
	Color[] resizeColors(Color[] oldColors, int numAdded) {
		//print ("resizing uvs by " + numAdded);
		Color[] newColors = new Color[oldColors.Length + numAdded];
		for(int i = 0; i < oldColors.Length; i++) newColors[i] = oldColors[i];
		return newColors;
	}
	
	int[] resizeTraingles(int[] oldTris, int numAdded) {
		int[] newTris = new int[oldTris.Length + numAdded];
		for(int i = 0; i < oldTris.Length; i++) newTris[i] = oldTris[i];
		return newTris;
	}
	
	void setColorsAndUVs(Mesh mesh) {
		int numOfVerts = mesh.vertices.Length;
		int numOfQuads = numOfVerts / 4;
		
		Vector2[] uvs = mesh.uv;
		Vector3[] verts = mesh.vertices;
		Color[] colors = mesh.colors;
		
		
		
		// put colors and UVs through the line
		float uvOffset = 0.0f;
		for (int i = 0; i < numOfQuads; i++) {
			
			float distanceOffset = Vector3.Distance(verts[i * 4], verts[(i * 4) + 2]) * lineStretch;
			if (uvOffset + distanceOffset > 0.75f) uvOffset = 0.25f;
			uvs[i * 4] 		 = new Vector2(0.0f, uvOffset);
			colors[i * 4] = Color.Lerp(startColor, endColor, i / (float)numOfQuads);
			uvs[(i * 4) + 1] = new Vector2(1.0f, uvOffset);
			colors[(i * 4) + 1] = Color.Lerp(startColor, endColor, i / (float)numOfQuads);

			
			uvOffset += distanceOffset;

			uvs[(i * 4) + 2] = new Vector2(0.0f, uvOffset);
			colors[(i * 4) + 2] = Color.Lerp(startColor, endColor, i / (float)numOfQuads);
			uvs[(i * 4) + 3] = new Vector2(1.0f, uvOffset);
			colors[(i * 4) + 3] = Color.Lerp(startColor, endColor, i / (float)numOfQuads);
		}
		
		
		// set the UVs for the end of the line
		uvOffset = 1.0f;

		for (int i = numOfQuads; i > 1; i--) {

			float distanceOffset = Vector3.Distance(verts[(i * 4) - 1], verts[(i * 4) - 3]) * lineStretch;
			
			uvs[(i * 4) - 1] = new Vector2(1.0f, uvOffset);
			uvs[(i * 4) - 2] = new Vector2(0.0f, uvOffset);
			
			uvOffset -= distanceOffset;

			uvs[(i * 4) - 3] = new Vector2(1.0f, uvOffset);
			uvs[(i * 4) - 4] = new Vector2(0.0f, uvOffset);
			
			if (uvOffset < 0.75f) break;

		}
		
		mesh.uv = uvs;
		mesh.colors = colors;
	}
}
