using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    private Vector2 beforeDir;
    public float jumpForce;
    public float jumpStamina;
    public LayerMask groundLayerMask;
    public bool isMove;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    public Action inventory;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
          Cursor.lockState = CursorLockMode.Locked;
          isMove=true;
    }
    
    void FixedUpdate()
    {
        if(isMove)
        {
            Move();
        }        
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            Look();
        }
    }

    void Move()
    {        
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        _rigidbody.velocity = dir;

        if(curMovementInput != Vector2.zero)
        {
            _rigidbody.velocity = dir;
            beforeDir= dir;
        }
        else
        {
            if(curMovementInput != beforeDir)
            {
                _rigidbody.velocity = dir;
                beforeDir= dir;
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    void Look()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    void Jump()
    {
        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);        
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {   
            if(CharacterManager.Instance.Player.condition.isBuffMulteJump)
            {
                Jump();
            } 
            else if(IsGrounded())
            {
                Jump();
                CharacterManager.Instance.Player.condition.UseStamina(jumpStamina);
            }
        }        
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }
}
