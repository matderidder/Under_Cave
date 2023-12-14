using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour
{
	//currently testing the algorithm with set targets, need a way to have multiple seekers that ask at later points
	public PathRequestManager requesetManager;
	//get the grid
	Grid grid;

	void Awake()
	{
		//set path manager
		requesetManager = GetComponent<PathRequestManager>();
		//set componet on grid completion
		grid = GetComponent<Grid>();
	}

	public void StartFindPath(Vector3 startPos, Vector3 targetPos)
	{
		StartCoroutine(FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

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
			for (int i = 1; i < openSet.Count; i++)
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
				pathSuccess = true;
				break;
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
		yield return null;
		if (pathSuccess)
		{
			waypoints = RetracePath(startNode, targetNode);
		}
		requesetManager.FinishedProcessingPath(waypoints, pathSuccess);
	}

	// Get the path backward
	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		//make list of path nodes and set the goal
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		path.Add(endNode);
		//start node is saved from grid so get each parent and add till at the start
		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		//Convert path to waypoints of vector3 for movement
		Vector3[] waypoints = SimplifyPath(path);

		//reverse the path to go from start to end
		Array.Reverse(waypoints);

		return waypoints;

	}

	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
			if (directionNew != directionOld)
			{
				waypoints.Add(path[i].worldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
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
