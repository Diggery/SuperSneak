using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathfinderType
{
    GridBased,
    WaypointBased
}

public class Pathfinding : MonoBehaviour 
{
    public List<Vector3> Path = new List<Vector3>();
    public PathfinderType PathType = PathfinderType.GridBased;
	public bool JS = false;

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        if (PathType == PathfinderType.GridBased)
        {
            Pathfinder.Instance.InsertInQueue(startPosition, endPosition, SetList);
        }
        else if (PathType == PathfinderType.WaypointBased)
        {
            WaypointPathfinder.Instance.InsertInQueue(startPosition, endPosition, SetList);          
        }
    }
	
	public void FindJSPath(Vector3[] arr)
    {
        if(arr.Length > 1)
		{	
			if (PathType == PathfinderType.GridBased)
	        {
	            Pathfinder.Instance.InsertInQueue(arr[0], arr[1], SetList);
	        }
	        else if (PathType == PathfinderType.WaypointBased)
	        {
	            WaypointPathfinder.Instance.InsertInQueue(arr[0], arr[1], SetList);
	        }
		}
    }

    protected virtual void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }
		
		if(!JS)
		{
	        Path.Clear();
	        Path = path;
            if (Path.Count > 0)
            {
                Path[0] = new Vector3(Path[0].x, Path[0].y - 1, Path[0].z);
                Path[Path.Count - 1] = new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y - 1, Path[Path.Count - 1].z);
            }
		}
		else
		{
			Vector3[] arr = new Vector3[path.Count];
			for(int i = 0; i < path.Count; i++)
			{
				arr[i] = path[i];
			}
            if (arr.Length > 0)
            {
                arr[0] = new Vector3(arr[0].x, arr[0].y - 1, arr[0].z);
                arr[arr.Length - 1] = new Vector3(arr[arr.Length - 1].x, Path[arr.Length - 1].y - 1, Path[arr.Length - 1].z);
                gameObject.SendMessage("GetJSPath", arr);
            }
		}
    }
}
	
