using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [System.Serializable]
    public class CameraRig
    {
        public GameObject target;
        public float CrouchHeight;
    }

    [SerializeField] CameraRig defaultCamera;
    [SerializeField] CameraRig aimCamera;


    Transform aimPivot;

    public float CameraMoveSpeed = 120.0f;
    public float clampAngle = 80.0f;
    public float damping = 1;
    public float inputSensitivity = 150.0f;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;

    private float rotY = 0.0f;
    private float rotX = 0.0f;
    // Use this for initialization
    void Start()
    {
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
        aimPivot = localPlayer.transform.Find("AimingPivot");
        defaultCamera.target = localPlayer.transform.Find("ThirdPersonCamera").gameObject;
        aimCamera.target = localPlayer.transform.Find("ThirdPersonCameraAimed").gameObject;       
    }

    // Update is called once per frame
    void Update()
    {
        if (localPlayer != null)
        {
            //We setup rotation of the sticks here for controller support
            var inputX = GameManager.Instance.InputController.Horizontal;
            var inputY = GameManager.Instance.InputController.Vertical;
            mouseX = GameManager.Instance.InputController.MouseInput.x;
            mouseY = GameManager.Instance.InputController.MouseInput.y;
            finalInputX = inputX + mouseX;
            finalInputZ = inputX + mouseY;
            rotY += finalInputX * inputSensitivity * Time.deltaTime;
            rotX += finalInputZ * inputSensitivity * Time.deltaTime;
            //rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            rotY = Mathf.Clamp(rotY, -clampAngle, clampAngle);
            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = aimPivot.rotation;
        }
    }
    private void LateUpdate()
    {
        if(localPlayer != null)
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

        //move towars game obj that is the targer
        float step = CameraMoveSpeed * Time.deltaTime;
        var targetHeight = new Vector3();


        // to do need to lerp rotation and postion for smooth transitions
        if (localPlayer.PlayerState.MoveState == PlayerState.EMoveState.CROUCHING)
        {
            // change camera height to move down a bit
            //to do
            targetHeight = new Vector3(cameraRig.target.transform.position.x, cameraRig.target.transform.position.y * cameraRig.CrouchHeight, cameraRig.target.transform.position.z);
        }
        else
            targetHeight = cameraRig.target.transform.position;

        
        transform.position = Vector3.MoveTowards(transform.position, targetHeight, step);       
    }
}
