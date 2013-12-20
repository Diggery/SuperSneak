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
	public int maxCustomRoomA = 0;
	[HideInInspector]
	public int numberCustomRoomA = 0;
	public int maxCustomRoomB = 0;
	[HideInInspector]
	public int numberCustomRoomB = 0;	
	public int maxCustomRoomC = 0;
	[HideInInspector]
	public int numberCustomRoomC = 0;
	
	// Number of crates
	public int maxCrates;
	int numberOfCrates;
	
	// Room structure
	public Room[,] rooms;
	
	//Container object to hold all the rooms
	public GameObject levelContainer;

	//Container object to hold all the rooms
	public Pathfinder pathFinder;

	//prefabs for room types
	public GameObject[] Room_Entrance;
	public GameObject[] Room_1ExitsA;
	public GameObject[] Room_2ExitsA;
	public GameObject[] Room_2ExitsB;
	public GameObject[] Room_2ExitsC;
	public GameObject[] Room_3ExitsA;
	public GameObject[] Room_3ExitsB;
	public GameObject[] Room_3ExitsC;
	public GameObject[] Room_CustomA;
	public GameObject[] Room_CustomB;
	public GameObject[] Room_CustomC;
	public GameObject[] Room_CustomD;
	public GameObject[] Room_ServerRoom;
	public GameObject[] Room_GuardRoom;
	
	public GameObject[] cratePrefabs;
	
	//seed offset for retries on bad levels
	int seedOffset;
	
	int failedLevels = 0;
	
	public override void Init () {
				
		// get the pathfinder so we can set it up
		GameObject pathFinderObj = GameObject.Find("Pathfinder");
		pathFinder = pathFinderObj.GetComponent<Pathfinder>();
		
		pathFinder.MapStartPosition = new Vector2(-10, -10);
		pathFinder.MapEndPosition = new Vector2((LEVEL_SIZE_X * 10) + 10, (LEVEL_SIZE_Y * 10) + 10);
		
		//create a game object to stick all the rooms in
		levelContainer = new GameObject("LevelContainer");
		
		//fill out the matrix for the rooms with a seed
		GameObject gameControlObj = GameObject.Find ("GameControl");
		Random.seed = gameControlObj.GetComponent<GameControl>().GetSeed() + seedOffset;
		GenerateLevel();
		
		print (getNumberOfRooms() + " rooms built");

		//add a guard room
		AddServerRoom();
		
		//add a guard room
		AddGuardRoom();
				
		//reset if the level sucks
		if (getNumberOfRooms() < 6 || //too few rooms
			!hasServerRoom() || //no server room
			!hasGuardRoom()) { //no guard room
			failedLevels++;
			if (failedLevels > 5) {
				Debug.Log("Failed too many times trying to build a level");	
			}
			resetLevel();
			return;
		} else {
			failedLevels = 0;	
		}

		//add art for the rooms
		GenerateGameRooms();

	}
	
	public void GenerateLevel() {
		

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
			gameRoom.SetUp();
			
			if (room.IsFirstNode()) {
				g.name = "FirstRoom";
			} else {
				g.name = "Room " + room.x + " " + room.y;
			}
		}
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
	
	public bool hasServerRoom() {
		foreach (Room room in rooms) {
			if (room == null) {
				//Debug.Log("Error: room is null");
				return false;
			}
			if (room.serverRoom) return true;
		} 
		return false;
	}
	
	public bool hasGuardRoom() {
		foreach (Room room in rooms) {
			if (room == null) {
				//Debug.Log("Error: room is null");
				return false;
			}
			if (room.guardRoom) return true;
		} 
		return false;
	}

	public void resetLevel() {
		seedOffset++;
		Debug.Log("Resetting Level");
		Destroy(levelContainer);
		levelContainer = null;
		numberCustomRoomA = 0;
		numberCustomRoomB = 0;
		numberCustomRoomC = 0;
		Init();
	}

	
	void AddServerRoom() {
		foreach (Room room in rooms) {
			if (room != null) {
				if (!room.IsFirstNode() && !room.HasChildren() && !room.placeHolder&& !room.guardRoom) {
					room.SetToServerRoom();
					break;
				}
			}
		}
	}
	
	void AddGuardRoom() {
		foreach (Room room in rooms) {
			if (room != null) {
				if (!room.IsFirstNode() && !room.HasChildren() && !room.placeHolder && !room.serverRoom) {
					room.SetToGuardRoom();
					break;
				}
			}
		}
	}
	
	public int getNumberOfCrates() {
		return numberOfCrates;
	}	
		
	public GameObject AddCrate(GameRoom gameRoom, Transform location) {
		
		int depth = gameRoom.room.NumRoomsToEntrance();
		
		//chance for adding a crate
		if (Random.value < 0.5f) return null;
		
		//make sure we dont have too many crates
		if (numberOfCrates >= maxCrates) return null;
		
		// ok, add one
		numberOfCrates++;
		int crateSelection = Random.Range(0, cratePrefabs.Length);
		GameObject newCrate = Instantiate(cratePrefabs[crateSelection], location.position, location.rotation) as GameObject;
		
		CrateController crateController = newCrate.GetComponent<CrateController>();
		int itemSelection = Random.Range(0, 10);
		if (depth > 10) itemSelection += 2;
		if (depth > 15) itemSelection += 5;
		switch (itemSelection) {
		case 0 :
			crateController.AddItem("Cash");
			crateController.AddItem("Jewel");
			break;
		case 1 :
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			break;
		case 2 :
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			break;
		case 3 :
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			break;
		case 4 :
			crateController.AddItem("HighEx");
			break;
		case 5 :
			crateController.AddItem("Gas");
			break;
		case 6 :
			crateController.AddItem("Smoke");
			break;
		case 7 :
			crateController.AddItem("Flash");
			break;
		case 8 :
			crateController.AddItem("HighEx");
			crateController.AddItem("HighEx");
			crateController.AddItem("HighEx");
			break;
		case 9 :
			crateController.AddItem("Gas");
			crateController.AddItem("HighEx");
			crateController.AddItem("Flash");
			crateController.AddItem("Smoke");
			break;
		default :
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Jewel");
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			crateController.AddItem("Cash");
			crateController.AddItem("Gas");
			crateController.AddItem("HighEx");
			crateController.AddItem("Flash");
			crateController.AddItem("Smoke");
			break;
		}
		return newCrate;
	}
}
