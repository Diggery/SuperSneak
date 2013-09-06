using UnityEngine;
using System.Collections;

static public class CustomRoom {
	
	static public bool convertToCustomA(Room targetRoom) {
		
		LevelController level = LevelController.instance;
		
		//test to see if there is space
		int heading = targetRoom.getHeadingDirection();
		int x = targetRoom.x;
		int y = targetRoom.y;
		
		if (heading == 0) // North
			if (!targetRoom.IsValidLocation(x,y+1) || !targetRoom.IsValidLocation(x,y+2)) return false; 
		if (heading == 1) // East
			if (!targetRoom.IsValidLocation(x+1,y) || !targetRoom.IsValidLocation(x+2,y)) return false; 
		if (heading == 2) // South
			if (!targetRoom.IsValidLocation(x,y-1) || !targetRoom.IsValidLocation(x,y-2)) return false; 
		if (heading == 3) // West
			if (!targetRoom.IsValidLocation(x-1,y) || !targetRoom.IsValidLocation(x-2,y)) return false; 
		
		//there is space, so convert to a custom room
		targetRoom.customType = 1;
		level.numberCustomRoomA++;
		
		// add a placeholder because its a 2 squares
		targetRoom.child1 = targetRoom.AddPlaceHolder(heading);
		
		//stick a real room at the other door
		targetRoom.child1.child1 = targetRoom.child1.AddChild(heading);
		
		//make the real room have kids.
		targetRoom.child1.child1.GenerateChildren();
		
		// were done
		return true;
	}
	
	static public bool convertToCustomB(Room targetRoom) {
		LevelController level = LevelController.instance;
		
		//test to see if there is space
		int heading = targetRoom.getHeadingDirection();
		int x = targetRoom.x;
		int y = targetRoom.y;
		
		if (heading == 0) {// North
			if (!targetRoom.IsValidLocation(x,y+1) || 
				!targetRoom.IsValidLocation(x,y+2)) return false; 
		
			if (!targetRoom.IsValidLocation(x-1,y-1) ||
				!targetRoom.IsValidLocation(x-1,y) || 
				!targetRoom.IsValidLocation(x-1,y+1) || 
				!targetRoom.IsValidLocation(x-1,y+2)) return false; 
		}	
		
		if (heading == 1) {// East
			if (!targetRoom.IsValidLocation(x+1,y) || 
				!targetRoom.IsValidLocation(x+2,y)) return false; 
		
			if (!targetRoom.IsValidLocation(x-1,y+1) ||
				!targetRoom.IsValidLocation(x,y+1) || 
				!targetRoom.IsValidLocation(x+1,y+1) || 
				!targetRoom.IsValidLocation(x+2,y+1)) return false; 
		}

		if (heading == 2) {// South
			if (!targetRoom.IsValidLocation(x,y-1) || 
				!targetRoom.IsValidLocation(x,y-2)) return false; 
		
			if (!targetRoom.IsValidLocation(x+1,y+1) ||
				!targetRoom.IsValidLocation(x+1,y) || 
				!targetRoom.IsValidLocation(x+1,y-1) || 
				!targetRoom.IsValidLocation(x+1,y-2)) return false;
		}
		
		if (heading == 3) {// West
			if (!targetRoom.IsValidLocation(x-1,y) || 
				!targetRoom.IsValidLocation(x-2,y)) return false; 
		
			if (!targetRoom.IsValidLocation(x+1,y-1) ||
				!targetRoom.IsValidLocation(x,y-1) || 
				!targetRoom.IsValidLocation(x-1,y-1) || 
				!targetRoom.IsValidLocation(x-2,y-1)) return false; 
		}
		
		//there is space, so convert to a custom room
		targetRoom.customType = 2;
		level.numberCustomRoomB++;
				
		// add a placeholders because its a 4 squares
		targetRoom.child1 = targetRoom.AddPlaceHolder(heading); 
		
		targetRoom.child2 = targetRoom.AddPlaceHolder((heading + 3)%4); 
		targetRoom.child2.child1 = targetRoom.child2.AddPlaceHolder(heading); 
		
		//add real rooms at the doors
		targetRoom.child1.child1 = targetRoom.child1.AddChild(heading);
		targetRoom.child2.child2 = targetRoom.child2.AddChild((heading + 2)%4);
		targetRoom.child2.child1.child1 = targetRoom.child2.child1.AddChild(heading);
		
		//make the real room have kids.
		targetRoom.child1.child1.GenerateChildren();
		targetRoom.child2.child2.GenerateChildren();
		targetRoom.child2.child1.child1.GenerateChildren();
		
		// were done
		return true;
	}
	
	static public bool convertToCustomC(Room targetRoom) {
		LevelController level = LevelController.instance;
		
		//test to see if there is space
		int heading = targetRoom.getHeadingDirection();
		int x = targetRoom.x;
		int y = targetRoom.y;
		
		if (heading == 0) {// North
			if (!targetRoom.IsValidLocation(x,y+1)) return false; 
		
			if (!targetRoom.IsValidLocation(x+1,y) || 
				!targetRoom.IsValidLocation(x+1,y+1)) return false; 
		}	
		
		if (heading == 1) {// East
			if (!targetRoom.IsValidLocation(x-1,y)) return false; 
		
			if (!targetRoom.IsValidLocation(x,y+1) || 
				!targetRoom.IsValidLocation(x-1,y+1)) return false; 
		}

		if (heading == 2) {// South
			if (!targetRoom.IsValidLocation(x,y-1)) return false; 
		
			if (!targetRoom.IsValidLocation(x-1,y) || 
				!targetRoom.IsValidLocation(x-1,y-1)) return false;
		}
		
		if (heading == 3) {// West
			if (!targetRoom.IsValidLocation(x+1,y)) return false; 
		
			if (!targetRoom.IsValidLocation(x,y-1) || 
				!targetRoom.IsValidLocation(x+1,y-1)) return false; 
		}
		
		//there is space, so convert to a custom room
		targetRoom.customType = 3;
		level.numberCustomRoomC++;
				
		// add a placeholders because its a 4 squares
		targetRoom.child1 = targetRoom.AddPlaceHolder(heading); 
		
		targetRoom.child2 = targetRoom.AddPlaceHolder((heading + 1)%4); 
		targetRoom.child2.child1 = targetRoom.child2.AddPlaceHolder(heading); 

		// were done
		return true;
	}
}
