using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour 
{

	//set the target and speed along with path use values
	public Transform target;
	float cSpeed = 5;
	Vector3[] path;
	int targetIndex;
	bool chasing = false;
	bool wandering = false;
	
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    //if pathfinder has a path start the movement along it, set values to start
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) 
	{
		if (pathSuccessful) 
		{
			path = newPath;
			targetIndex = 0;
            StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	//For each part of the path make sure to reach the vector and then go back to next
	IEnumerator FollowPath() 
	{
		Vector3 current = path[0];
		while (true) 
		{
			if (transform.position == current) 
			{
				targetIndex ++;
				if (targetIndex >= path.Length) 
				{
					targetIndex = 0;
					path = new Vector3[0];
					yield break;
				}
				current = path[targetIndex];
			}
            this.GetComponent<Animator>().SetTrigger("Patrol");
            transform.position = Vector3.MoveTowards(transform.position, current, cSpeed * Time.deltaTime);
			transform.rotation = Quaternion.LookRotation(target.position - transform.position);
			wandering = false;
			yield return null;

		}
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
		{
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
			chasing = true;
        }
		
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
		{
            PathRequestManager.RequestPath(transform.position, transform.position, OnPathFound);
        }
		chasing = false;
    }

    //debugging with apth draw
    public void OnDrawGizmos() 
	{
		if (path != null) 
		{
			for (int i = targetIndex; i < path.Length; i ++) 
			{
				Gizmos.color = Color.black;
				Gizmos.DrawCube(path[i], Vector3.one);

				if (i == targetIndex) 
				{
					Gizmos.DrawLine(transform.position, path[i]);
				}
				else 
				{
					Gizmos.DrawLine(path[i-1],path[i]);
				}
			}
		}
	}
}
