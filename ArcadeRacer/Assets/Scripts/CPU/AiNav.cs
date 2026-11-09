using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class AiNav : MonoBehaviour
{
    public Tracker tracker;
    public List<Transform> waypoints;
    public int currentWaypoint;
    private CarController carController;
    public float waypointRange;
    private float currentAngle;

    void Start()
    {
        carController = GetComponent<CarController>();
        waypoints = tracker.waypoints;
        currentWaypoint = 0;
    }

    void Update()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) < waypointRange)
        {
            currentWaypoint++;
            if (currentWaypoint == waypoints.Count) currentWaypoint = 0;
        }
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        currentAngle = Vector3.SignedAngle(fwd, waypoints[currentWaypoint].position - transform.position, Vector3.up);
        carController.SetInput(1, currentAngle, 0, 0);
    }
}
