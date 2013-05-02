using UnityEngine;
using System.Collections;


[ExecuteInEditMode]

public class RoomConfig : MonoBehaviour {

	bool decorateRoom;
	public bool selected;
	
	public Transform[,] allRooms;
	public Vector2 floorLoc;
	
	public WallConfig[] walls;
	
	public int getWallType(int wall) {
		return walls[wall].getWallType();
	}
	
	public void setWallType(int wall, int wallType) {
		walls[wall].setWallType(wallType);
	}
	
	public void setWallBySelection(int wallType) {
		Vector3 roomOffset;
		int wallDirection = 0;
		int oppositeWall = 0;
		
		for (wallDirection = 0; wallDirection < 4; wallDirection++) {
			switch (wallDirection) {
				case 0 : //north
					roomOffset = new Vector3(0.0f, 5.0f, 10.0f);
					oppositeWall = 2;
					break;
				case 1 : // east
					roomOffset = new Vector3(10.0f, 5.0f, 0.0f);
					oppositeWall = 3;
					break;
				case 2 : //south
					roomOffset = new Vector3(0.0f, 5.0f, -10.0f);
					oppositeWall = 0;
					break;
				case 3 : //west
					roomOffset = new Vector3(-10.0f, 5.0f, 0.0f);
					oppositeWall = 1;
					break;
				default :
					roomOffset = new Vector3(0.0f, 5.0f, 0.0f);
					oppositeWall = 0;
					break;
			}		
			int floorMask = 1 << 10;
			RaycastHit hit;							
	
			if (Physics.Raycast (transform.position + roomOffset, Vector3.down, out hit, 10.0f, floorMask)) {

				RoomConfig neighborRoom = hit.transform.parent.GetComponent<RoomConfig>();
				if (neighborRoom.selected) {
					setWallType(wallDirection, wallType);
					neighborRoom.setWallType(oppositeWall, wallType);
				}
			} else {
				setWallType(wallDirection, 2);
			}
		}
	}
	
	public void setRooms(Transform[,] newRooms, Vector2 newFloorLoc) {
		allRooms = newRooms;
		floorLoc = newFloorLoc;
	}
	
	public void selectRoom() {
		selected = true;
	}
	
	public void deselectRoom() {
		selected = false;
	}
	
	public void removeRoom() {
		DestroyImmediate(gameObject);
	}
}
