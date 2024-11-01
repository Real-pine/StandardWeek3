using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputSystem controller;
    public PlayerCondition condition;

    private void Awake()
    {
        // 이 오브젝트가 없어지고 새로운 캐릭터가 생성되더라도 charactermanager랑 이어지기때문에 데이터가 날라가지 않음
        // 그러니까 처음부터 
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerInputSystem>();
        condition = GetComponent<PlayerCondition>();
    }
}
