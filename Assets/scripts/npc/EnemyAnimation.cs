using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
public class EnemyAnimation : MonoBehaviour {


    [SerializeField] Animator animator;

    Waypoint waypointTarget;
    Vector3 lastPosition;
    PathFinder pathFinder;

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
    }

    private void Update()
    {
        float velocity = ((transform.position - lastPosition).magnitude) / Time.deltaTime;
        lastPosition = transform.position;
        animator.SetFloat("Vertical", velocity / pathFinder.Agent.speed);
    }

}
