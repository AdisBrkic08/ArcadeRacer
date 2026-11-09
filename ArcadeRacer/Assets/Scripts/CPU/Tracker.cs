using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public List<Transform> waypoints;

    void Awake()
    {
      foreach(Transform tr in gameObject.GetComponentInChildren<Transform>())
        {
            waypoints.Add(tr);
        }
        waypoints.Remove(waypoints[0]);
    }


}