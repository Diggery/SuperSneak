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
		
	void Start () {
		LevelController level = LevelController.instance;
		GameObject[] prefabSet = level.Room_0Exits01;
		
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
			GameObject entranceRoom = Instantiate(level.Room_Entrance[0], transform.position, Quaternion.AngleAxis(180, Vector3.up)) as GameObject;	
			entranceRoom.transform.parent = this.transform;	
			return;
		}
		
		if (room.customType > 0) {
			
			roomsToEntrance = room.NumRoomsToEntrance();
				
			if (room.customType == 1) prefabSet = level.Room_Custom01;
			if (room.customType == 2) prefabSet = level.Room_Custom02;
			if (room.customType == 3) prefabSet = level.Room_Custom03;
			
			Quaternion customRotation = Quaternion.AngleAxis((room.getHeadingDirection() * 90) + 180, Vector3.up);
			int customSelection = Random.Range(0, prefabSet.Length);
			GameObject customRoom = Instantiate(prefabSet[customSelection], transform.position, customRotation) as GameObject;	
			customRoom.transform.parent = this.transform;			
			custom = true;
			return;	
		}

		bool exitWest = false, exitEast = false, exitNorth = false, exitSouth = false;
		
		if (room.IsConnectedTo(room.GetNorth())) exitNorth = true;
		if (room.IsConnectedTo(room.GetEast())) exitEast = true;
		if (room.IsConnectedTo(room.GetSouth())) exitSouth = true;
		if (room.IsConnectedTo(room.GetWest())) exitWest = true;
		
		int prefabRotation = 0;
		
		int childCount = room.NumExits();
		switch (childCount) {
		case 1:
			
			roomsToEntrance = room.NumRoomsToEntrance();
			
			if        (exitWest && !exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_0Exits01;
				prefabRotation = 270;				
			} else if (!exitWest && exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_0Exits01;
				prefabRotation = 90;					
			} else if (!exitWest && !exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_0Exits01;
				prefabRotation = 0;				
			} else if (!exitWest && !exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_0Exits01;
				prefabRotation = 180;				
			}
			break;
		case 2:
			if        (exitWest && exitEast && !exitNorth && !exitSouth) {
				prefabSet = level.Room_1Exits02;
				prefabRotation = 90;			
			} else if (exitWest && !exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_1Exits03;
				prefabRotation = 270;					
			} else if (exitWest && !exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_1Exits03;
				prefabRotation = 180;				
			} else if (!exitWest && exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_1Exits01;
				prefabRotation = 90;				
			} else if (!exitWest && exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_1Exits01;
				prefabRotation = 180;	
			} else if (!exitWest && !exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_1Exits02;
				prefabRotation = 0;				
			}

			break;
		case 3:
			if        (!exitWest && exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_2Exits01;
				prefabRotation = 180;				
			} else if (exitWest && !exitEast && exitNorth && exitSouth) {
				prefabSet = level.Room_2Exits02;
				prefabRotation = 180;					
			} else if (exitWest && exitEast && !exitNorth && exitSouth) {
				prefabSet = level.Room_2Exits03;
				prefabRotation = 180;				
			} else if (exitWest && exitEast && exitNorth && !exitSouth) {
				prefabSet = level.Room_2Exits03;
				prefabRotation = 0;				
			}			
			break;
		default:
			prefabSet = level.Room_0Exits01;
			prefabRotation = 0;				
			break;
		}
		int prefabSelection = Random.Range(0, prefabSet.Length);
		GameObject newRoom = Instantiate(prefabSet[prefabSelection], transform.position, Quaternion.AngleAxis(prefabRotation, Vector3.up)) as GameObject;	
		newRoom.transform.parent = this.transform;
		
	}
	
	void Update () 
	{
		
	}
}
