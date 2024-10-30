using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputSystem controller;
    public PlayerCondition condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerInputSystem>();
        condition = GetComponent<PlayerCondition>();
    }
}
