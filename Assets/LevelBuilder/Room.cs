using UnityEngine;
using System.Collections;

public class Room {
	public string name;
	public int x;
	public int y;
	public Room parent;
	public Room child1, child2;
	private LevelController level;
	public bool placeHolder;
	public bool guardRoom;
	public int customType;
	public int roomsToEntrance;

	
	private static int MAX_TRIES = 3;
	
	public Room(Room _parent, int _x, int _y, bool _placeHolder) {
		parent = _parent;
		x = _x;
		y = _y;
		level = LevelController.instance;
		name = "room " + x + "," + y;
		placeHolder = _placeHolder;
	}
	
	public bool IsFirstNode()
	{
		if (parent == null) return true;
		return false;
	}
	
	public bool HasChildren() {
		return NumChildren() > 0;
	}
	
	public int NumChildren() {
		int n = 0;
		if (child1 != null) n++;
		if (child2 != null) n++;
		return n;
	}
	
	public int NumExits() {
		int n = 0;
		if (parent != null) n++;
		if (child1 != null) n++;
		if (child2 != null) n++;
		return n;
	}
		
	public void GenerateChildren() {
		if (parent != null && NumChildren() == 0) {
			if (Random.value < 0.3f && level.numberCustomRoomA < level.maxCustomRoomA) {
				if (CustomRoom.convertToCustomA(this)) return;
			}
			if (Random.value < 0.3f && level.numberCustomRoomB < level.maxCustomRoomB) {
				if (CustomRoom.convertToCustomB(this)) return;
			}
			if (Random.value < 0.3f && level.numberCustomRoomC < level.maxCustomRoomC) {
				if (CustomRoom.convertToCustomC(this)) return;
			}
		}
		if (NumChildren() == 2) return;
		if (NumEmptyNeighbours() == 0) return;
		
		int dir_child_1 = GetValidDirection(1);
		if (dir_child_1 >= 0) child1 = AddChild(dir_child_1);

		int dir_child_2 = GetValidDirection(1);
		if (dir_child_1 >= 0) child2 = AddChild(dir_child_2);
		
		if (child1 != null) child1.GenerateChildren();
		if (child2 != null) child2.GenerateChildren();
	}
	
	public Room AddChild(int direction) {
		if (direction == 0) return level.AddRoom(this,x,y+1); // North
		if (direction == 1) return level.AddRoom(this,x+1,y); // East
		if (direction == 2) return level.AddRoom(this,x,y-1); // South
		if (direction == 3) return level.AddRoom(this,x-1,y); // West
		return null;
	}
	
	public Room AddPlaceHolder(int direction) {
		if (direction == 0) return level.AddPlaceHolder(this,x,y+1); // North
		if (direction == 1) return level.AddPlaceHolder(this,x+1,y); // East
		if (direction == 2) return level.AddPlaceHolder(this,x,y-1); // South
		if (direction == 3) return level.AddPlaceHolder(this,x-1,y); // West
		return null;
	}		
	

	
	public int GetValidDirection(int num_tries)
	{
		if (num_tries > MAX_TRIES) return -1;
		
		int direction = Random.Range(0,4);
		if (direction == 0) {// North
			if (y >= level.LEVEL_SIZE_Y - 1) return GetValidDirection(num_tries+1);
			if (GetNorth() != null) return GetValidDirection(num_tries+1);
		} else if (direction == 1) {// East
			if (x >= level.LEVEL_SIZE_X - 1) return GetValidDirection(num_tries+1);
			if (GetEast() != null) return GetValidDirection(num_tries+1);
		} else if (direction == 2) {// South
			if (y == 0) return GetValidDirection(num_tries+1);
			if (GetSouth() != null) return GetValidDirection(num_tries+1);
		} else if (direction == 3) {// West
			if (x == 0) return GetValidDirection(num_tries++);
			if (GetWest() != null) return GetValidDirection(num_tries+1);
		}
		return direction;
	}
	
	public bool IsValidLocation(int x, int y) {
		if (x < 0 || x > level.LEVEL_SIZE_X - 1) return false;
		if (y < 0 || y > level.LEVEL_SIZE_Y - 1) return false;
		if (level.rooms[x, y] != null) return false;
		return true;
	}
	
	public bool IsConnectedTo(Room room)
	{
		if (room == null) return false;
		if (room.parent == this) return true;
		if (room == this.parent) return true;
		return false;
	}
	
	public Room GetEast()
	{
		int tileX = x + 1;
		if (tileX >= level.LEVEL_SIZE_X) return null;
		
		int tileY = y;
		
		return level.rooms[tileX, tileY];
	}
	
	public Room GetWest()
	{
		int tileX = x - 1;
		if (tileX < 0) return null;
		
		int tileY = y;
		return level.rooms[tileX, tileY];
	}
	
	public Room GetNorth()
	{
		int tileY = y + 1;
		if (tileY >= level.LEVEL_SIZE_Y) return null;
		
		int tileX = x;		
		
		return level.rooms[tileX, tileY];
	}
	
	public Room GetSouth()
	{
		int tileY = y - 1;
		if (tileY < 0) return null;
		
		int tileX = x;		
		return level.rooms[tileX, tileY];
	}
	
	public bool IsThereRoomsAround()
	{
		if (GetNorth() != null) return true;
		if (GetEast() != null) return true;
		if (GetSouth() != null) return true;
		if (GetWest() != null) return true;
		return false;
	}
	
	public int NumNeighbours() {
		int n = 0;
		if (GetNorth() != null) n++;
		if (GetEast() != null) n++;
		if (GetSouth() != null) n++;
		if (GetWest() != null) n++;
		return n;
	}
			
	public int NumEmptyNeighbours() {
		int n = 0;
		if (GetNorth() == null && y < level.LEVEL_SIZE_Y - 1) n++;
		if (GetEast() == null && x < level.LEVEL_SIZE_X - 1) n++;
		if (GetSouth() == null && y > 0) n++;
		if (GetWest() == null && x > 0) n++;
		return n;
	}
	public int NumRoomsToEntrance() {
		int n = 0;
		Room nextRoom = this;
        while (nextRoom.parent != null) {
            nextRoom = nextRoom.parent;
			n++;
        }
		roomsToEntrance = n;
		return n;
	}
						
	public int getHeadingDirection() {
		int direction = -1;
		if (GetNorth() == this.parent) direction = 2;
		if (GetEast() == this.parent) direction = 3;
		if (GetSouth() == this.parent) direction = 0;
		if (GetWest() == this.parent) direction = 1;
		if (direction == -1) Debug.Log (this.parent.name + " has is no parent");
		return direction;
	}
	
	public void SetToGuardRoom() {
		guardRoom = true;
	}
}