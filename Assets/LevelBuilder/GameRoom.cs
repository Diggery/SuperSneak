using UnityEngine;
using System.Collections;

public class GameRoom : MonoBehaviour {
	
	public Room room;
	public string parent;
	public string child1;
	public string child2;
	public bool placeHolder;
	public bool custom;
	public int roomsToEntrance;
	LevelController level;

			
	public void SetUp () {
		level = LevelController.instance;
		GameObject[] prefabSet = level.Room_1ExitsA;
		
		gameObject.tag = "Room";
		
		if (room.parent != null) parent = room.parent.name;
		if (room.child1 != null) child1 = room.child1.name;
		if (room.child2 != null) child2 = room.child2.name;
		

		//Placeholders dont need any art
		if (room.placeHolder) {
			placeHolder = true;
			return;
		}
		
		//Give the first room special art
		if (room.parent == null) {
			CreateRoom(level.Room_Entrance[0], 180);
			return;
		}
		
		if (room.customType > 0) {
			
			roomsToEntrance = room.NumRoomsToEntrance();
				
			if (room.customType == 1) prefabSet = level.Room_CustomA;
			if (room.customType == 2) prefabSet = level.Room_CustomB;
			if (room.customType == 3) prefabSet = level.Room_CustomC;
			
			int customRotation = (room.getHeadingDirection() * 90) + 180;
			int customSelection = Random.Range(0, prefabSet.Length);
			CreateRoom(prefabSet[customSelection], customRotation);
			custom = true;
			return;	
		}

		bool exitWest = false, exitEast = false, exitNorth = false, exitSouth = false;
		
		if (room.IsConnectedTo(room.GetNorth())) exitNorth = true;
		if (room.IsConnectedTo(room.GetEast())) exitEast = true;
		if (room.IsConnectedTo(room.GetSouth())) exitSouth = true;
		if (room.IsConnectedTo(room.GetWest())) exitWest = true;
		
		int prefabRotation = 0;
		
		int exitCount = room.NumExits();
		switch (exitCount) {
		case 1:
			
			roomsToEntrance = room.NumRoomsToEntrance();
			
			if        (exitWest && !exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_1ExitsA;
				prefabRotation = 270;				
			} else if (!exitWest && exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_1ExitsA;
				prefabRotation = 90;					
			} else if (!exitWest && !exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_1ExitsA;
				prefabRotation = 0;				
			} else if (!exitWest && !exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_1ExitsA;
				prefabRotation = 180;				
			}
			
			//override is it is a guard room
			if (room.guardRoom) prefabSet = level.Room_GuardRoom;
			
			break;
		case 2:
			if        (exitWest && exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_2ExitsB;
				prefabRotation = 90;			
			} else if (exitWest && !exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_2ExitsC;
				prefabRotation = 270;					
			} else if (exitWest && !exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_2ExitsC;
				prefabRotation = 180;				
			} else if (!exitWest && exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_2ExitsA;
				prefabRotation = 90;				
			} else if (!exitWest && exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_2ExitsA;
				prefabRotation = 180;	
			} else if (!exitWest && !exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_2ExitsB;
				prefabRotation = 0;				
			}

			break;
		case 3:
			if        (!exitWest && exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_3ExitsA;
				prefabRotation = 180;				
			} else if (exitWest && !exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_3ExitsB;
				prefabRotation = 180;					
			} else if (exitWest && exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_3ExitsC;
				prefabRotation = 180;				
			} else if (exitWest && exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_3ExitsC;
				prefabRotation = 0;				
			}			
			break;
		default:
			prefabSet = level.Room_3ExitsC;
			prefabRotation = 0;				
			break;
		}
		
		// everything is set up, so create the room
		int prefabSelection = Random.Range(0, prefabSet.Length);
		CreateRoom(prefabSet[prefabSelection], prefabRotation);
		
	}
	
	void CreateRoom(GameObject roomPrefab, int roomRotation) { 
		GameObject newRoom = Instantiate(roomPrefab, transform.position, Quaternion.AngleAxis(roomRotation, Vector3.up)) as GameObject;	
		newRoom.transform.parent = this.transform;	
		//switch any obstacle tags over to obstacle layers for the enemies view collision
		Transform collisionObj = newRoom.transform.Find ("Collision");
		if (collisionObj) {
			foreach	(Transform child in collisionObj) {
				if (child.tag == "Obstacle") child.gameObject.layer = LayerMask.NameToLayer("Obstacle");
			}
		}
		
		//create any crates needed
		Transform cratesObj = newRoom.transform.Find ("Crates");
		if (cratesObj) {
			foreach	(Transform child in cratesObj) {
				GameObject newCrate = level.AddCrate(this, child);
				if (newCrate) newCrate.transform.parent = transform;
			}
			Destroy(cratesObj.gameObject);
		}
	}
}
