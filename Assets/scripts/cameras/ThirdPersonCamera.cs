using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float damping;

    Transform cameraLookTarget;
    Player localPlayer;
    private void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
    }

    private void Instance_OnLocalPlayerJoined(Player player)
    {
        localPlayer = player;
        cameraLookTarget = localPlayer.transform.Find("cameraLookTarget");

        if (cameraLookTarget == null)
        {
            cameraLookTarget = localPlayer.transform;
        }
    }

    private void Update()
    {
        if (localPlayer == null)
            return;

        Vector3 targetPosition = cameraLookTarget.position + 
            localPlayer.transform.forward * cameraOffset.z +
            localPlayer.transform.up * cameraOffset.y +
            localPlayer.transform.right * cameraOffset.x;

        Quaternion targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);

        transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, damping * Time.deltaTime);
    }
}
