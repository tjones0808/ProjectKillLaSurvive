using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerNetwork : NetworkBehaviour {

    Player player;
    PlayerAnimation playerAnimation;
    NetworkState state;
    NetworkState lastSendState;
    NetworkState lastSendRpcState;
    NetworkState lastReceivedState;

    List<NetworkState> predictedStates;

    [System.Serializable]
    public partial class NetworkState : InputController.InputState
    {
        public float PositionX;
        public float PositionY;
        public float PositionZ;
        public float RotationAngleY;
        public float TimeStamp;
        public float AimTargetX;
        public float AimTargetY;
        public float AimTargetZ;
    }

	// Use this for initialization
	void Start () {
        player = GetComponent<Player>();
        playerAnimation = GetComponent<PlayerAnimation>();
        predictedStates = new List<NetworkState>();
        state = new NetworkState();

        if (isLocalPlayer)
            player.SetAsLocalPlayer();
	}

    private NetworkState CollectInput()
    {
        var state = new NetworkState()
        {
            CoverToggle = GameManager.Instance.InputController.CoverToggle,
            Fire1 = GameManager.Instance.InputController.Fire1,
            Fire2 = GameManager.Instance.InputController.Fire2,
            Horizontal = GameManager.Instance.InputController.Horizontal,
            Vertical = GameManager.Instance.InputController.Vertical,
            IsCrouched = GameManager.Instance.InputController.IsCrouched,
            IsSprinting = GameManager.Instance.InputController.IsSprinting,
            IsWalking = GameManager.Instance.InputController.IsWalking,
            Reload = GameManager.Instance.InputController.Reload,
            RotationAngleY = transform.rotation.eulerAngles.y,
            TimeStamp = Time.time
        };

        if (state.Fire1)
        {
            Vector3 shootingSolution = player.WeaponControllerr.GetImpactPoint();
            state.AimTargetX = shootingSolution.x;
            state.AimTargetY = shootingSolution.y;
            state.AimTargetZ = shootingSolution.z;
        }

        return state;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            state = CollectInput();

            player.SetInputController(state);
            player.Move(state.Horizontal, state.Vertical);
        }

        if (lastReceivedState == null)
            return;

        UpdateState();
    }

    void UpdateState()
    {

        Vector3 serverPosition = new Vector3(lastReceivedState.PositionX, lastReceivedState.PositionY, lastReceivedState.PositionZ);

        if (isLocalPlayer && !isServer)
        {
            // remove old states
            predictedStates.RemoveAll(x => x.TimeStamp < lastReceivedState.TimeStamp);

            // expected state to be there
            var predictedState = predictedStates.Where(x => x.TimeStamp == lastReceivedState.TimeStamp).First();

            Vector3 predictedPosition = new Vector3(predictedState.PositionX, predictedState.PositionY, predictedState.PositionZ);
            float positionDifferenceFromServer = Vector3.Distance(predictedPosition, serverPosition);

            if (positionDifferenceFromServer > .3f)
                transform.position = Vector3.Lerp(transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime);

        }

        if (!isLocalPlayer)
        {
            playerAnimation.Vertical = lastReceivedState.Vertical;
            playerAnimation.Horizontal = lastReceivedState.Vertical;
            playerAnimation.IsWalking = lastReceivedState.IsWalking;
            playerAnimation.IsSprinting = lastReceivedState.IsSprinting;
            playerAnimation.IsCrouched = lastReceivedState.IsCrouched;
            playerAnimation.IsInCover = lastReceivedState.IsInCover;
            playerAnimation.AimAngle = lastReceivedState.AimAngle;

            Vector3 shootingSolution = new Vector3(lastReceivedState.AimTargetX, lastReceivedState.AimTargetY, lastReceivedState.AimTargetZ);

            player.SetInputController(lastReceivedState);

            if (shootingSolution != Vector3.zero)
                player.WeaponControllerr.ActiveWeapon.SetAimPoint(shootingSolution);

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lastReceivedState.RotationAngleY, transform.rotation.eulerAngles.z);
            player.Move(lastReceivedState.Horizontal, lastReceivedState.Vertical);

            if (!isServer)
            {
                float positionDifferenceFromServer = Vector3.Distance(transform.position, serverPosition);
                if (positionDifferenceFromServer > .3f)
                {
                    transform.position = Vector3.Lerp(transform.position, serverPosition, player.Settings.RunSpeed * Time.deltaTime);
                }
            }
        }

    }
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (isInputStateDirty(state, lastSendState))
            {
                lastSendState = state;
                Cmd_HandleInput(SerializeState(lastSendState));

                state.PositionX = transform.position.x;
                state.PositionY = transform.position.y;
                state.PositionZ = transform.position.z;

                predictedStates.Add(state);
            }
        }

        //if we are server, update remote clients
        if (isServer && lastReceivedState != null)
        {
            NetworkState stateSolution = new NetworkState()
            {
                PositionX = transform.position.x,
                PositionY = transform.position.y,
                PositionZ = transform.position.z,
                Horizontal = lastReceivedState.Horizontal,
                Vertical = lastReceivedState.Vertical,
                IsAiming = lastReceivedState.IsAiming,
                AimAngle = lastReceivedState.AimAngle,
                CoverToggle = lastReceivedState.CoverToggle,
                Fire1 = lastReceivedState.Fire1,
                Fire2 = lastReceivedState.Fire2,
                IsWalking = lastReceivedState.IsWalking,
                IsRunning = lastReceivedState.IsRunning,
                IsSprinting = lastReceivedState.IsSprinting,
                Reload = lastReceivedState.Reload,
                RotationAngleY = lastReceivedState.RotationAngleY,
                TimeStamp = lastReceivedState.TimeStamp
            };

            if (isInputStateDirty(stateSolution, lastSendRpcState))
            {
                lastSendRpcState = stateSolution;
                Rpc_HandleStateSolution(SerializeState(lastSendRpcState));
            }

        }
    }

    [Command]
    void Cmd_HandleInput(byte[] data)
    {
        lastReceivedState = DeserializeState(data);
    }

    [ClientRpc]
    void Rpc_HandleStateSolution(byte[] data)
    {
        lastReceivedState = DeserializeState(data);
    }

    bool isInputStateDirty(NetworkState a, NetworkState b)
    {
        if (b == null)
            return true;

        return a.AimAngle != b.AimAngle ||
            a.CoverToggle != b.CoverToggle ||
            a.Fire1 != b.Fire1 ||
            a.Fire2 != b.Fire2 ||
            a.Vertical != b.Vertical ||
            a.Horizontal != b.Horizontal ||
            a.IsAiming != b.IsAiming ||
            a.IsCrouched != b.IsCrouched ||
            a.IsSprinting != b.IsSprinting ||
            a.Reload != b.Reload ||
            a.IsWalking != b.IsWalking ||
            a.RotationAngleY != b.RotationAngleY;
    }

    private BinaryFormatter bf = new BinaryFormatter();
    private byte[] SerializeState(NetworkState state)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            bf.Serialize(stream, state);

            return stream.ToArray();
        }
    }

    private NetworkState DeserializeState(byte[] bytes)
    {
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return (NetworkState)bf.Deserialize(stream);
        }
    }
}
