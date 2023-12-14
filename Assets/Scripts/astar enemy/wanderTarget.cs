using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class wanderTarget : MonoBehaviour
{
    public float wanderX;
    public float wanderZ;
    public GameObject seeker;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, seeker.transform.position) < 5.0f)
        {
            transform.position = new Vector3(Random.Range(0, wanderX), 0, Random.Range(0, wanderZ));
        }
    }
}
