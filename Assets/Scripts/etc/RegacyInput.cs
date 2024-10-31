using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegacyInput : MonoBehaviour
{
    
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
