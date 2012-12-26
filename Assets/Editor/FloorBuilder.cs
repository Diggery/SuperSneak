using UnityEngine;
using UnityEditor;

public class FloorBuilder : EditorWindow {
    string floorName = "Floor Name";
    int floorx;
    int floory;
	GameObject floorPrefab;
	Transform[,] rooms;
    
    // Add menu named "My Window" to the Window menu
    [MenuItem ("Window/Build Floor")]
    static void Init () {
        // Get existing open window or if none, make a new one:
        FloorBuilder window = (FloorBuilder)EditorWindow.GetWindow (typeof (FloorBuilder));
		
    }
    
    void OnGUI () {
        GUILayout.Label ("Floor Settings", EditorStyles.boldLabel);
		floorName = EditorGUILayout.TextField ("Enter Name", floorName);
		floorPrefab = EditorGUILayout.ObjectField ("Floor Prefab", floorPrefab, typeof(GameObject), true) as GameObject;
		
		floorx = EditorGUILayout.IntField("Floor Width:", floorx);
		floory = EditorGUILayout.IntField("Floor Height:", floory);
		
        if(GUILayout.Button("Build Floor")) {
			generateLevel(new Vector2(floorx, floory));
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
        if(GUILayout.Button("Set Selection to Open")) {
			setWalls(Selection.gameObjects, 0);
		}		
		
        if(GUILayout.Button("Set Selection to Doorway")) {
			setWalls(Selection.gameObjects, 1);
		}	
		
        if(GUILayout.Button("Set Selection to Walls")) {
			setWalls(Selection.gameObjects, 2);
		}
	}
	
	public void generateLevel(Vector2 floorSize) {
		GameObject newFloor = new GameObject(floorName);
		rooms = new Transform[Mathf.RoundToInt(floorSize.x), Mathf.RoundToInt(floorSize.y)];
		int x;
		int z;
		for(x = 0; x < floorSize.x; x++) {
			for(z = 0; z < floorSize.y; z++) {
				GameObject newRoom = Instantiate(floorPrefab, new Vector3(x * 10.0f, 0.0f, z * -10.0f), Quaternion.identity) as GameObject;
				rooms[x,z] = newRoom.transform;
				RoomConfig roomConfig = newRoom.GetComponent<RoomConfig>(); 
				roomConfig.status();
				newRoom.transform.parent = newFloor.transform;
			}
		}
		for(x = 0; x < floorSize.x; x++) {
			for(z = 0; z < floorSize.y; z++) {		
				RoomConfig roomConfig = rooms[x,z].GetComponent<RoomConfig>(); 
				roomConfig.setRooms(rooms, new Vector2(x,z));

			}
		}
	}
	
	public void setWalls(GameObject[] currentSelection, int wallType) {
		RoomConfig[] currentRooms = FindObjectsOfType(typeof(RoomConfig)) as RoomConfig[];
		foreach (GameObject currentlySelected in currentSelection) {
			currentlySelected.SendMessageUpwards("selectRoom", SendMessageOptions.DontRequireReceiver);
		}
		foreach (RoomConfig room in currentRooms) {
			if (room.selected) room.setWallBySelection(wallType);
		}
		foreach (RoomConfig room in currentRooms) {
			room.SendMessageUpwards("deselectRoom", SendMessageOptions.DontRequireReceiver);
		}
	}
}

