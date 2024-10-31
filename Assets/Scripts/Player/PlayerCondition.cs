using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;
    
    Condition health {get{return uiCondition.health;}}
    Condition mana {get{return uiCondition.mana;}}
    Condition hunger {get{return uiCondition.hunger;}}
    Condition stamina {get{return uiCondition.stamina;}}

    public float noHungerHealthDecay;
    public event Action OnTakeDamage;

    public void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        mana.Add(mana.passiveValue * Time.deltaTime);
        
        if (hunger.curValue < 0.0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue < 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("death");
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        OnTakeDamage?.Invoke();
    }
}
