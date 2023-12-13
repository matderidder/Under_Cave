using UnityEngine;
using System.Collections;

public class Node {
	//simple node class with walkable and postion in world and grid
	public bool walkable;
	public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	//define valuse of path finding with cost and parents
	public int gCost;
	public int hCost;
	public Node parent;
	
	//constructor
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) {
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	//fcost value
	public int FCost
	{
		get
		{
			return gCost + hCost;
		}
	}


	public int CompareTo(Node nodeToCompare)
	{
		int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

}
