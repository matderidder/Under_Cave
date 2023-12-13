using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour 
{
	//currently testing the algorithm with set targets, need a way to have multiple seekers that ask at later points
	public Transform seeker, target;
	//get the grid
	Grid grid;

	void Awake() 
	{
		//set componet on grid completion
		grid = GetComponent<Grid> ();
	}

	void Update() 
	{
		//run path
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) 
	{
		//take in the start and the goal and pathfind with the nodes search percentage
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//initilze open list and a hash just to check if node is in closed
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		//while not goal or path still available
		while (openSet.Count > 0) 
		{
			//take the lowest f from the open list
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) 
			{
				if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost) 
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}
			//switch node to closed and remove from open list
			openSet.Remove(node);
			closedSet.Add(node);

			//end if target
			if (node == targetNode) 
			{
				RetracePath(startNode,targetNode);
				return;
			}

			//else get neighbhors and set values of them and put in open list
			foreach (Node neighbour in grid.GetNeighbours(node)) 
			{
				//make sure node is walkable or not already explored
				if (!neighbour.walkable || closedSet.Contains(neighbour)) 
				{
					continue;
				}

				//set cost by distance from start and to goal and set F
				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) 
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	// Get the path backward
	void RetracePath(Node startNode, Node endNode) 
	{
		//make list of path nodes and set the goal
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		//start node is saved from grid so get each parent and add till at the start
		while (currentNode != startNode) 
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		//reverse the path to go from start to end
		path.Reverse();

		//set path;
		grid.path = path;

	}

	//distance between two nodes on a grid to get g and h values at a reasonable level
	int GetDistance(Node nodeA, Node nodeB) 
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
