using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour {

    public GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;
    public enum CameraMode
    {
        FIRSTPERSON,
        THIRDPERSON
    };

    public CameraMode CamMode;

    private void Update()
    {
        // change to local player input state
        if (GameManager.Instance.InputController.ChangeCamera)
        {
            if (CamMode == CameraMode.FIRSTPERSON)
                CamMode = CameraMode.THIRDPERSON;
            else
                CamMode = CameraMode.FIRSTPERSON;

            StartCoroutine(CamChange());
        }
    }

    IEnumerator CamChange()
    {
        yield return new WaitForSeconds(0.01f);

        if (CamMode == CameraMode.THIRDPERSON)
        {
            ThirdPersonCamera.SetActive(true);
            FirstPersonCamera.SetActive(false);
        }

        if (CamMode == CameraMode.FIRSTPERSON)
        {
            ThirdPersonCamera.SetActive(false);
            FirstPersonCamera.SetActive(true);
        }
    }
}
