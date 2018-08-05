using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [System.Serializable]
    public class CameraRig
    {
        public GameObject target;
        public float CrouchHeight;
    }

    [SerializeField] CameraRig defaultCamera;
    [SerializeField] CameraRig aimCamera;
    [SerializeField] CameraRig crouchCamera;
    [SerializeField] CameraRig proneCamera;




    public float CameraMoveSpeed = 120.0f;
    Vector3 FollowPOS;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;

    private float rotY = 0.0f;
    private float rotX = 0.0f;
    // Use this for initialization
    void Start () {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
	}


    private void Awake()
    {
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
    }

    Player localPlayer;

    private void Instance_OnLocalPlayerJoined(Player player)
    {
        localPlayer = player;
    }

    // Update is called once per frame
    void Update () {

        //We setup rotation of the sticks here for controller support
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputX + mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

	}
    private void LateUpdate()
    {
        CameraUpdater();
    }

    void CameraUpdater()
    {
        CameraRig cameraRig = defaultCamera;

        if (localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING || localPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING)
        {
            //set the target to follow
            cameraRig = aimCamera;
        }

        if (localPlayer.PlayerState.MoveState == PlayerState.EMoveState.CROUCHING)
        {
            // change camera height to move down a bit
            //to do
        }

        //move towars game obj that is the targer
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, cameraRig.target.transform.position, step);
    }
}
