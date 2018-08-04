using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{

    [SerializeField] float scanSpeed;
    [SerializeField] LayerMask mask;
    [SerializeField]
    [Range(0, 360)]
    float fieldOfView;


    SphereCollider rangeTrigger;
    // only players for now, could expand
    List<Player> targets;

    public event System.Action<Vector3> OnTargetSelected;


    Player m_selectedTarget;    
    Player selectedTarget
    {
        get
        {
            return m_selectedTarget;
        }

        set
        {
            m_selectedTarget = value;

            if (m_selectedTarget == null)
                return;

            if (OnTargetSelected != null)
                OnTargetSelected(m_selectedTarget.transform.position);
        }
    }


    private void Start()
    {
        rangeTrigger = GetComponent<SphereCollider>();

        targets = new List<Player>();

        PrepareScan();
    }

    void PrepareScan()
    {
        if (selectedTarget != null)
            return;



        GameManager.Instance.Timer.Add(ScanForTarget, scanSpeed);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        if (selectedTarget != null)
        {
            Gizmos.DrawLine(transform.position, selectedTarget.transform.position);
        }

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(fieldOfView / 2) * GetComponent<SphereCollider>().radius);
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(-fieldOfView / 2) * GetComponent<SphereCollider>().radius);
    }

    Vector3 GetViewAngle(float angle)
    {
        float radian = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    void ScanForTarget()
    {
        // this would make the target line disappear after a target is out of scanner range
        //selectedTarget = null;


        Collider[] results = Physics.OverlapSphere(transform.position, rangeTrigger.radius);

        for (int i = 0; i < results.Length; i++)
        {
            var player = results[i].transform.GetComponent<Player>();

            if (player == null)
                continue;

            if (!InLineOfSight(Vector3.up, player.transform.position))
                continue;

            if(!targets.Contains(player))
                targets.Add(player);

        }

        if (targets.Count == 1)
        {
            selectedTarget = targets[0];
        }
        else
        {
            // check for closet target;
            float closetTarget = rangeTrigger.radius;

            foreach (var target in targets)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < closetTarget)
                    selectedTarget = target;
            }
        }

        PrepareScan();
    }

    bool InLineOfSight(Vector3 eyeheight, Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;

        if (Vector3.Angle(transform.forward, direction.normalized) < fieldOfView / 2)
        {
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            if (Physics.Raycast(transform.position + eyeheight, direction.normalized, distanceToTarget, mask))
            {
                // not working something always blocking view even when nothing there
                // something blocking view
               // return false;
            }

            return true;
        }

        return false;
    }
}
