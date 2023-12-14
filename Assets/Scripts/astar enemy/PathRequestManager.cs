using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour 
{
	//Queue of paths to make and the request
	Queue<PathRequest> Queue = new Queue<PathRequest>();
	PathRequest currentRequest;

	//instance of pathfinder and request
	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isPathing;

	void Awake() 
	{
		instance = this;
		pathfinding = GetComponent<Pathfinding>();
	}

	//request method for units to call when chasing player
	public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback) 
	{
		PathRequest Request = new PathRequest(pathStart,pathEnd,callback);
		instance.Queue.Enqueue(Request);
		instance.TryProcessNext();
	}

	//if more paths go to next in queue and start pathfinding
	void TryProcessNext() 
	{
		if (!isPathing && Queue.Count > 0) 
		{
            currentRequest = Queue.Dequeue();
            isPathing = true;
			pathfinding.StartFindPath(currentRequest.Start, currentRequest.End);
		}
	}

	//if finished with current path check for next path if succeded
	public void FinishedProcessingPath(Vector3[] path, bool success) 
	{
        currentRequest.callback(path,success);
        isPathing = false;
		TryProcessNext();
	}
	//path request structure to store info
	struct PathRequest 
	{
		public Vector3 Start;
		public Vector3 End;
		public Action<Vector3[], bool> callback;

		public PathRequest(Vector3 start, Vector3 end, Action<Vector3[], bool> _callback) 
		{
			Start = start;
			End = end;
			callback = _callback;
		}

	}
}
