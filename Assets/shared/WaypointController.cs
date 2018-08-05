using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointController : MonoBehaviour {

    Waypoint[] waypoints;

    int currentWaypointIndex = -1;

    public event System.Action<Waypoint> OnWaypointChanged;


    private void Awake()
    {
        waypoints = GetWaypoints();
    }

    public void SetNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex == waypoints.Length)
            currentWaypointIndex = 0;

        if (OnWaypointChanged != null)
            OnWaypointChanged(waypoints[currentWaypointIndex]);
    }

    private Waypoint[] GetWaypoints()
    {
        return GetComponentsInChildren<Waypoint>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 previousWaypoint = Vector3.zero;

        foreach (var waypoint in GetWaypoints())
        {
            Gizmos.DrawWireSphere(waypoint.transform.position, .2f);

            Vector3 waypointPosition = waypoint.transform.position;

            if (previousWaypoint != Vector3.zero)
                Gizmos.DrawLine(previousWaypoint, waypointPosition);

            previousWaypoint = waypointPosition;

                

        }
    }

}
