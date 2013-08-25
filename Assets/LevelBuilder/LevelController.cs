using UnityEngine;
using System.Collections;

public class LevelController : MonoSingleton<LevelController> 
{
	// Level Rooms
	public int LEVEL_SIZE_X = 20;
	public int LEVEL_SIZE_Y = 20;
	
	// Size of 3D Model Prefab in World Space
	public int ROOM_SIZE_X = 14; 
	public int ROOM_SIZE_Z = 9;
	
	// Number of possible custom rooms
	public int maxCustomRoom01 = 0;
	public int numberCustomRoom01 = 0;
	public int maxCustomRoom02 = 0;
	public int numberCustomRoom02 = 0;	
	public int maxCustomRoom03 = 0;
	public int numberCustomRoom03 = 0;	
	
	// Room structure
	public Room[,] rooms;
	
	//Container object to hold all the rooms
	public GameObject levelContainer;

	//prefabs for room types
	public GameObject[] Room_Entrance;
	public GameObject[] Room_0Exits01;
	public GameObject[] Room_1Exits01;
	public GameObject[] Room_1Exits02;
	public GameObject[] Room_1Exits03;
	public GameObject[] Room_2Exits01;
	public GameObject[] Room_2Exits02;
	public GameObject[] Room_2Exits03;
	public GameObject[] Room_Custom01;
	public GameObject[] Room_Custom02;
	public GameObject[] Room_Custom03;
	public GameObject[] Room_Custom04;
	public GameObject[] Room_GuardRoom;
	
	public override void Init () {
		
		//create a game object to stick all the rooms in
		levelContainer = new GameObject("LevelContainer");
		
		//fill out the matrix for the rooms with a raondom seed
		GenerateLevel(Random.Range(0,100));
		
		print (getNumberOfRooms() + " rooms built");
		
		//reset if there are too few rooms
		if (getNumberOfRooms() < 6) {
			resetLevel();
			return;
		}
		
		//add art for the rooms
		GenerateGameRooms();
		

	}
	
	public void GenerateLevel(int newSeed) {
		
		Random.seed = newSeed;
		//Debug.Log("Generation with seed " + newSeed);
		
		// Create room structure
		rooms = new Room[LEVEL_SIZE_X,LEVEL_SIZE_Y];
		
		// Create our first room at a random position
		int roomX = Mathf.FloorToInt(LEVEL_SIZE_X * 0.5f);
		int roomY = 0;
		
		Room firstRoom = AddRoom(null, roomX,roomY); // null parent because it's the first node
		
		// Generate childrens
		firstRoom.AddChild(0).GenerateChildren();
	}
	
	void GenerateGameRooms() {
		// For each room in our matrix generate a 3D Model from Prefab
		foreach (Room room in rooms) {
			if (room == null) continue;
			
			// Real world position
			float worldX = room.x * ROOM_SIZE_X;
			float worldZ = room.y * ROOM_SIZE_Z;
			
			GameObject g = new GameObject(); 
			g.transform.parent = levelContainer.transform;
			g.transform.position = new Vector3(worldX,0,worldZ);
			
			// Add the room info to the GameObject main script 
			GameRoom gameRoom = g.AddComponent<GameRoom>();
			gameRoom.room = room;
			
			if (room.IsFirstNode()) {
				g.name = "FirstRoom";
			} else {
				g.name = "Room " + room.x + " " + room.y;
			}
		}
		//add some guard rooms
		AddGuardRooms();
	}
	
	public Room AddRoom(Room parent, int x, int y) {
		Room room = new Room(parent, x, y, false);
		rooms[x,y] = room;
		return room;
	}
	
	public Room AddPlaceHolder(Room parent, int x, int y) {
		Room room = new Room(parent, x, y, true);
		rooms[x,y] = room;
		return room;
	}
	
	public int getNumberOfRooms() {
		int roomCount = 0;
		foreach (Room room in rooms) {
			if (room != null) roomCount++;
		} 
		return roomCount;
	}
	
	public void resetLevel() {
		Debug.Log("Resetting Level");
		Destroy(levelContainer);
		levelContainer = null;
		numberCustomRoom01 = 0;
		numberCustomRoom02 = 0;
		numberCustomRoom03 = 0;
		Init();
	}
	
	void AddGuardRooms() {
		foreach (Room room in rooms) {
			if (room != null) {
				Debug.Log(room.roomsToEntrance);
				if (room.roomsToEntrance > 1) {// && !room.placeHolder && room.customType < 1) {
					Debug.Log(room.name + "is the guard room");
					break;
				}
			}
		}
	}
}
