using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegacyInput : MonoBehaviour
{
    // 레거시 방식은 키입력에 따라 오브젝트의 벡터값을 일일히 변화시키는 로직으로 구성되어있다
    float direction;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            direction = -1.0f;
            transform.position += Vector3.right * direction;
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            direction = 1.0f;
            transform.position += Vector3.right * direction;
        }
        else
        {
            direction = 0.0f;
            transform.position += Vector3.right * direction;
        }
    }
}
