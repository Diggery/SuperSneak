using UnityEngine;
using System.Collections;

public class LevelBuilder : MonoBehaviour {
	
	public GameObject roomPrefab;
	float timer;
	
	Transform[,] rooms;

	void Start () {
		generateLevel(new Vector2(10, 10));
	
	}
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > 0.5f) {
			if (timer < 9.0f) alignWalls();		
			timer = 10.0f;
		}
	
	}
	
	public void generateLevel(Vector2 levelSize) {
		rooms = new Transform[Mathf.RoundToInt(levelSize.x), Mathf.RoundToInt(levelSize.y)];
		for(int x = 0; x < levelSize.x; x++) {
			for(int z = 0; z < levelSize.y; z++) {
				GameObject newRoom = Instantiate(roomPrefab, new Vector3(x * 10.0f, 0.0f, z * 10.0f), Quaternion.identity) as GameObject;
				rooms[x,z] = newRoom.transform;
			}
		}
		
	}
	
	public void alignWalls() {
		for(int x = 0; x < rooms.GetLength(0); x++) {
			for(int z = 0; z < rooms.GetLength(1); z++) {
				RoomConfig currentRoom = rooms[x,z].GetComponent<RoomConfig>();
				for (int i = 0; i < 4; i++) {
					int currentWallType = currentRoom.getWallType(i);
						
   					Vector3 roomOffset;
   					int oppositeWall;
					
					switch (i) {
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

					if (Physics.Raycast (rooms[x,z].position + roomOffset, Vector3.down, out hit, 10.0f, floorMask)) {
						RoomConfig neighborRoom = hit.transform.parent.GetComponent<RoomConfig>();
						int neighborWallType = neighborRoom.getWallType(oppositeWall);
						if (neighborWallType < currentWallType) {
							currentRoom.setWallType(i, neighborWallType);
						} else {
							neighborRoom.setWallType(oppositeWall, currentWallType);
						}
					} else {
						print ("missed");
						currentRoom.setWallType(i, 2);	
					}
				}
			}
		}
	}
	
	public void listRooms() {
		int roomCount = 0;
		for(int x = 0; x < rooms.GetLength(0); x++) {
			for(int z = 0; z < rooms.GetLength(1); z++) {
				roomCount++;
			}
		}
		print (roomCount);
	}
	
}
