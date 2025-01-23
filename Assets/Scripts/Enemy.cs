using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0;
    [SerializeField] private List<Transform> waypoints;

    private int waypointIndex;
    private float range;
    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        range = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Move enemy through specified points
        Move();
    }
    //move enemy
    void Move()
    {
        transform.LookAt(waypoints[waypointIndex]);
        transform.Translate(Vector3.forward*speed *Time.deltaTime);
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < range)
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                //iterate through waypoints indefinitely
                waypointIndex = 0;
            }
        }
    }
}
