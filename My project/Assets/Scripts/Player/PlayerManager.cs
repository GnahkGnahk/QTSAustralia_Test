using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] Rigidbody playerRigidbody;
    [SerializeField] float jumpForce, speed, runMaxSpeed, walkMaxSpeed, rotationSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] CapsuleCollider playerCapsuleCollider;
    [SerializeField] Transform ModelPlayer;

    internal PlayerAnimationNew p_AnimationNew_Instance;
    internal PlayerInputSys p_InputSys_Instance;
    internal PlayerSwitchCharacter p_SwitchCharacter_Instance;
    internal CameraManager cameraMN_Instance;
    internal GameManager gameMN_Instance;

    internal float rotationAngle_Run = -30f;
    internal bool isJumping = false, isMoving = false;

    internal bool isRotate = false;

    Vector2 inputVector;
    Vector3 oldPos;

    float manitude;

    public float playerHeight_Idle = 1.8f, playerHeight_Jump = 1.4f,
        playerRadius_Idle = 0.3f, playerRadius_Laid = 0.1f,
        playerCenter_Y_Idle = 0.9f, playerCenter_Y_Crouching = 0.75f;


    private void Start()
    {
        InitSetting();
    }


    private void FixedUpdate()
    {
        Move();
        Animation();
        CameraFollowPlayer();
    }

    void Move() //  Add Force
    {
        inputVector = p_InputSys_Instance.GetInputVector();

        if (groundCheck.isGrounded)
        {
            if (Mathf.Round(playerRigidbody.velocity.magnitude) < runMaxSpeed)
            {
                playerRigidbody.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
            }
        }
    }
    
    void Animation()
    {
        if (isJumping) { return; }

        if (groundCheck.isGrounded && Mathf.Floor(playerRigidbody.velocity.y) == 0)
        {
            manitude = playerRigidbody.velocity.magnitude;
            p_AnimationNew_Instance.MoveAnimation(manitude);

            if (manitude < 0.1f)
            {
                //Debug.Log("Idle");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);
                p_AnimationNew_Instance.MoveBool(isIdle: true);
                isMoving = false;
            }
            else if (manitude <= walkMaxSpeed)
            {
                //Debug.Log("Walk");
                ModelPlayer.localRotation = Quaternion.Euler(0f, 0f, 0f);

                p_AnimationNew_Instance.MoveBool(isStandardWalk: true);
                isMoving = true;
            }
            else
            {
                //Debug.Log("Run: " + manitude);
                ModelPlayer.localRotation = Quaternion.Euler(0f, rotationAngle_Run, 0f);
                p_AnimationNew_Instance.MoveBool(isRun: true);
                isMoving = true;
            }
        }
        else
        {
            //Debug.Log("Not on Ground");
        }
    }

    internal void HoldShiftPerformed()
    {
        cameraMN_Instance.SetCameraOn(CameraType.TOP_DOWN2);
    }

    internal void HoldShiftCanceled()
    {
        cameraMN_Instance.SetCameraOn(CameraType.TOP_DOWN2, false);
    }
    Coroutine rotateCoroutine;
    Vector3 targetDirection;
    public void RotationPerformed(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0.1f)
        {
            targetDirection = new Vector3(moveDir.x, 0, moveDir.y);
            if (!isRotate)
            {
                isRotate = true;
                rotateCoroutine = StartCoroutine(RotateCoroutine());
            }
        }
    }
    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
    public void RotationCanceled()
    {
        isRotate = false;
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
    }

    public void Jump()
    {
        if (groundCheck.isGrounded && Mathf.Round(playerRigidbody.velocity.y) == 0)
        {
            p_AnimationNew_Instance.Jump();
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
            if (manitude < 0.1f) isMoving = false;
            p_InputSys_Instance.SetJump(false);
        }
    }

    public void SwitchCharacter(int choice)
    {
        if(isMoving || isJumping) { return; }
        ModelPlayer = p_SwitchCharacter_Instance.SwitchCharacter(choice);
        p_AnimationNew_Instance.ChangeAvatar(ModelPlayer.GetComponent<Animator>().avatar);
    }

    void CameraFollowPlayer()
    {
        if (oldPos == transform.position) return;
        cameraMN_Instance.SetPosition(transform.position - oldPos);
        oldPos = transform.position;
    }

    void InitSetting()
    {
        p_AnimationNew_Instance = PlayerAnimationNew.Instance;
        p_InputSys_Instance = PlayerInputSys.Instance;
        p_SwitchCharacter_Instance = PlayerSwitchCharacter.Instance;
        cameraMN_Instance = CameraManager.Instance;
        gameMN_Instance = GameManager.Instance;

        playerHeight_Idle = playerCapsuleCollider.height;
        playerRadius_Idle = playerCapsuleCollider.radius;
        oldPos = transform.position;

    }


    //=======================================================================================
    //=================================== Animation event ===================================
    //=======================================================================================
    void ResetColliderPlayer()
    {
        //Debug.Log("ResetColliderPlayer");
        playerCapsuleCollider.height = playerHeight_Idle;
        playerCapsuleCollider.radius = playerRadius_Idle;
        playerCapsuleCollider.center = new Vector3(playerCapsuleCollider.center.x, playerCenter_Y_Idle, playerCapsuleCollider.center.z);
    }
    void AdjustPlayerColliderJump() // For jump
    {
        //Debug.Log("AdjustPlayerColliderJump");
        playerCapsuleCollider.height = playerHeight_Jump;
        playerRigidbody.velocity /= 2;
    }

    void FallFlatEvent()
    {
        //Debug.Log("FallFlatEvent");
        p_InputSys_Instance.SetPlayerControls(false);
        playerCapsuleCollider.height = 0.1f;
        playerCapsuleCollider.radius = 0.1f;
    }

    void JumpDone()
    {
        //Debug.Log("Jump Done");
        isJumping = false;
        p_InputSys_Instance.SetJump(true);
    }
}