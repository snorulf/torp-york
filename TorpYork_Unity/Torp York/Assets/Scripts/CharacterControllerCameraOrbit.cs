using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerCameraOrbit : MonoBehaviour
{
    [SerializeField]
    float directionDampTime = 0.25f;

    [SerializeField]
    float directionSpeed = 3.0f;

    [SerializeField]
    float rotationDegreePerSecond = 120f;

    private Animator animator = null;

    public int m_LocomotionId = 0;

    private Camera _gameCamera = null;

    private float _direction = 0.0f;
    private float _speed = 0.0f;

    private float _horizontal = 0.0f;
    private float _vertical = 0.0f;

    private AnimatorStateInfo stateInfo;

    void Awake()
    {
        _gameCamera = Camera.main;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }

        m_LocomotionId = Animator.StringToHash("Base Layer.Blend Tree");
    }

    void Update()
    {
        if (animator) 
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            GetInput();

            StickToWorldSpace(this.transform, _gameCamera.transform, ref _direction, ref _speed);

            _speed = new Vector2(_horizontal, _vertical).sqrMagnitude;

            animator.SetFloat("Speed", _speed);
            //animator.SetFloat("Direction",  _horizontal/*_direction*/, directionDampTime, Time.deltaTime);

            if (_speed > 0.1f)
            {
                // rotate this.transform around y axis using _direction
                this.transform.Rotate(0, _direction * rotationDegreePerSecond * Time.deltaTime, 0);
            }

            _horizontal = 0.0f;
            _vertical = 0.0f;
        }
    }

    // void FixedUpdate()
    // {
    //     if (IsInLocomotion() && ((_direction >= 0 && _horizontal >= 0) || (_direction < 0 && _horizontal < 0)))
    //     {
    //         Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, 100 * (_horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(_horizontal));
    //         Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
    //         this.transform.rotation = this.transform.rotation * deltaRotation;
    //     }
    // }

    private bool IsInLocomotion()
    {
        return stateInfo.fullPathHash == m_LocomotionId;
    }

    public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(_horizontal, 0, _vertical);

        speedOut = stickDirection.sqrMagnitude;

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), axisSign, Color.red);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), stickDirection, Color.blue);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        angleRootToMove /= 180f;

        directionOut = angleRootToMove * 360f;
    }

    private void GetInput()
    {
        if(Input.GetKey(KeyCode.W))
        {
            _vertical += 1;
        }
        if(Input.GetKey(KeyCode.S))
        {
            _vertical -= 1;
        }
        if(Input.GetKey(KeyCode.D))
        {
            _horizontal += 1;
        }
        if(Input.GetKey(KeyCode.A))
        {
            _horizontal -= 1;
        }
    }
}
