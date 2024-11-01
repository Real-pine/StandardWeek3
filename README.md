# StandardWeek3

##Q1 분석 문제

- 입문주차에선 InputSystem의 Behavior를 SendMassage를 통해 모두 코드를 통해 이벤트를 등록해줘야 했지만, 이번 챕터에서는 Invoke Unity Events를 사용해 이벤트 메서드만 구현해 놓으면
  유니티인스펙터창에서 관리할 수 있습니다. 하지만 동작원리는 거진 비슷하다고 생각합니다.
  
- `CharacterManager`와 `Player`는 서로 합성관계에 있습니다. `CharacterManager`는 전역에서 접근가능하기때문에, 마찬가지로 Player역시 전역에서 접근가능할 수 있습니다.
  따라서 다른 클래스에서 `Player`에 직접 접근하는 것이 아니라 CharacterManager를 통해 접근하게 됩니다. 지금 단계에서는 Player를 static선언하고 오히려 더 불편할 수 있겠지만
  나중에 Player가 destroy되더라도 데이터는 남아있고 새로 Player가 생성되면 데이터는 이어받을 수 있습니다.
  
- Move()분석
  ```cs
  private void Move()
    {
        Vector3 dir = (transform.forward * curMovementInput.y) + (transform.right * curMovementInput.x);
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }
  ```
  `(transform.forward * curMovementInput.y)`이건 앞뒤키입력을 받아 움직일때, Vector3에서 z축선상에서 움직이는 것을 계산
  `(transform.right * curMovementInput.x)`이건 좌우키입력을 받아 움직일때, Vector3에서 x축선상에서 움직이는 것을 계산
  `dir.y = _rigidbody.velocity.y;`이 코드는 추락이나 점프 시(Vector3에서 y축 선상의 움직임) 오브젝트에 속도(방향까지포함)를 넣어줌

  CameraLook()분석
  ```cs
  private void CameraLook()
    {
        // 위아래 바라볼 때 x축 자체가 회전하기때문에 마우스 y축 움직임을 x에 대입
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp ( camCurXRot, minXLook, maxXLook );
        // mouse가+가 될때 x축 자체는 음수값으로 회전해야 시야가 위쪽으로 돌아감
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        
        // 좌우로 바라볼 때
        transform.eulerAngles += new Vector3(0, mouseDelta.x*lookSensitivity, 0);
    }
  ```

  IsGrounded()분석
  ```cs
  private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            // 앞뒤좌우 기준 아래로 레이배열생성, 이 레이들은 캐릭터(오브젝트)의 중심이아니라 가장자리에서 접지여부를 확인하기 위함
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
  ```

- Move와 CameraLook 함수를 각각 FixedUpdate, LateUpdate에서 호출하는 이유에 대해 생각해보세요.
  물리연산(현재 rigidbody)은 일정한 시간 간격마다 호출되는 FixedUpdate에서 실행되어야 물리적 결과가 일관성 있게 유지된다.
  프레임률과 관계없이 일정한 시간 간격으로 호출되므로, 이동 연산이 프레임에 영향받지않고 안정적으로 처리된다.
  프레임레이트가 높은 환경에선 안정적이고, 낮은 환경에선 예측 가능한 이동이 가능해짐.
  LateUpdate는 모든 Update호출 후 호출되며, 카메라와 같이 종속적인 연산을 처리하기에 적합
  캐릭터의 이동이 완료된 뒤 카메라 시선이 계산되는게 유리함, 이로인해 카메라는 항상 캐릭터의 최신 위치를 기준으로 시선을 조정한다.

## Q2 분석문제
- 별도의 UI 스크립트를 만드는 이유에 대해 객체지향적 관점에서 생각해보세요.
  1. 각 클래스는 한 가지 역할에 집중하도록 되어있습니다. 'Condition'에서는 체력,허기같은 캐릭터 상태에 대한 값을 설정하거나 업데이트하는 역할을 수행합니다.
     `PlayerCondition`에서는 캐릭터와 관련된 고유의 로직을 처리하는데 집중합니다. `UICondition`은 UI에서 해당 상태를 시각적으로 나타내는 데 중점을 둡니다.
  2. `UICondition`클래스가 UI에서 `Condition`을 참조하고 있지만, 직접적으로 `Condition`내부 구조를 다루지 않고 `PlayerCondition`이 UI와 상태를 연결해주는
     역할을 맡습니다. 이를 통해 UI로직과 게임 로직이 서로 독립적으로 동작하며, UI가 변경되더라도 게임 로직에는 영향을 미치지 않습니다.

- `DamageIndicator`클래스는 캐릭터가 피해를 받을때마다 화면에 시각적 경고를 표시합니다. `Flash()`메서드가 `OnTakeDamage`이벤트에 연결되어 캐릭터가
  피해를 받으면 실행되고, `FadeAway()`코루틴을 통해 경고가 서서히 사라지도록 합니다
  `CampFire`클래스는 주기적으로 범위내 대상에게 데미지를 가하는 역할을 합니다. `OnTriggerEnter`와`OnTriggerExit`를 통해 범위내 객체를 감지하며 `_things`리스트에
  추가하거나 제거합니다. `DealDamage()`를 통해 리스트의 모든 객체에 정해진 데미지를 가합니다.
  
