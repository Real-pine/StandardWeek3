using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    private bool canLook = true;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // 마우스를 게임 중앙 좌표에 고정시키고 마우스 커서 안보이게하기
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // 물리연산(현재 rigidbody)은 일정한 시간 간격마다 호출되는 FixedUpdate에서 실행되어야 물리적 결과가 일관성 있게 유지된다.
    // 프레임률과 관계없이 일정한 시간 간격으로 호출되므로, 이동 연산이 프레임에 영향받지않고 안정적으로 처리된다.
    // 프레임레이트가 높은 환경에선 안정적이고, 낮은 환경에선 예측 가능한 이동이 가능해짐
    private void FixedUpdate()
    {
        Move();
    }
    // LateUpdate는 모든 Update호출 후 호출되며, 카메라와 같이 종속적인 연산을 처리하기에 적합
    // 캐릭터의 이동이 완료된 뒤 카메라 시선이 계산되는게 유리함, 이로인해 카메라는 항상 캐릭터의 최신 위치를 기준으로 시선을 조정한다.
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void CameraLook()
    {
        // 위아래 바라볼 때 (x축 자체가 회전하기때문에 마우스 y축 움직임을 x에 대입)
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp ( camCurXRot, minXLook, maxXLook );
        // mouse가+가 될때 x축 자체는 음수값으로 회전해야 시야가 위쪽으로 돌아감
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        
        // 좌우로 바라볼 때
        transform.eulerAngles += new Vector3(0, mouseDelta.x*lookSensitivity, 0);
    }

    private void Move()
    {
        Vector3 dir = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // 앞뒤좌우 기준 아래로 레이배열생성, 이 레이들은 캐릭터 중심이아니라 가장자리에서 접지여부를 확인하기 위함
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        // 매우 짧은 레이캐스트 쏴서 레이어마스크(땅)에 충돌했는지 판단 하고 맞으면 트루값반환(발이 땅에 닿았냐 여부판단)
        for (int i = 0; i < rays.Length; ++i)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        // false의 경우 공중에 떠있다고 판단함
        return false;
    }
}
