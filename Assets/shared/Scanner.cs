using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;

[RequireComponent(typeof(SphereCollider))]
public class Scanner : MonoBehaviour
{

    [SerializeField] float scanSpeed;
    [SerializeField] LayerMask mask;
    [SerializeField]
    [Range(0, 360)]
    float fieldOfView;


    SphereCollider rangeTrigger;

    public float ScanRange
    {
        get
        {
            if (rangeTrigger == null)
                rangeTrigger = GetComponent<SphereCollider>();

            return rangeTrigger.radius;
        }
    }

    public event System.Action OnScanReady;

    private void Start()
    {
        rangeTrigger = GetComponent<SphereCollider>();
    }
    


    void PrepareScan()
    {
        GameManager.Instance.Timer.Add(() =>
        {
            if (OnScanReady != null)
                OnScanReady();
        }, scanSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(fieldOfView / 2) * GetComponent<SphereCollider>().radius);
        Gizmos.DrawLine(transform.position, transform.position + GetViewAngle(-fieldOfView / 2) * GetComponent<SphereCollider>().radius);
    }

    Vector3 GetViewAngle(float angle)
    {
        float radian = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }

    public List<T> ScanForTargets<T>()
    {
        print("Scanning for targets");
        // this would make the target line disappear after a target is out of scanner range
        //selectedTarget = null;
        List<T> targets = new List<T>();

        Collider[] results = Physics.OverlapSphere(transform.position, ScanRange);

        for (int i = 0; i < results.Length; i++)
        {
            var player = results[i].transform.GetComponent<T>();

            if (player == null)
                continue;

            if (!transform.InLineOfSight(results[i].transform.position, fieldOfView, mask, Vector3.up))
                continue;

            if (!targets.Contains(player))
                targets.Add(player);

        }

        PrepareScan();

        return targets;
    }
}
