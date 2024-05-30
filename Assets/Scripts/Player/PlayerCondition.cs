using System;
using System.Collections;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina;}}
    
    public float noHungerHealthDecay;
    public bool isBuff;
    public bool isBuffBoost;
    public bool isBuffMulteJump;
    public bool isBuffInvincibility;
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if(hunger.curValue <= 0.0f)
        {
            if(!isBuffInvincibility)
            {health.Subtract(noHungerHealthDecay * Time.deltaTime);}
        }
        if(health.curValue <= 0.0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("쥬금");
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
    public void buffCheck(int num,float time)
    {
        if(isBuff==false)
        {
            isBuff=true;
            if(num==0) {Boost(time);}
            else if(num==1) {MulteJump(time);}
            else if(num==2) {Invincibility(time);}            
        }
        else
        {
            Debug.Log("다른버프가 유지중입니다");
        }
    }
    
    public void Boost(float time)
    {        
        isBuffBoost=true;    
        CharacterManager.Instance.Player.controller.moveSpeed =10f;
        StartCoroutine(BuffTime(time));
        
    }
    public void MulteJump(float time)
    {
        isBuffMulteJump=true;
        StartCoroutine(BuffTime(time));        
    }
    public void Invincibility(float time)
    {
        isBuffInvincibility=true;
        StartCoroutine(BuffTime(time));
    }

    IEnumerator BuffTime(float time)
    {   
        yield return new WaitForSeconds(time);
        CharacterManager.Instance.Player.controller.moveSpeed =5f;
        isBuff=false;
        isBuffBoost=false;
        isBuffMulteJump=false;
        isBuffInvincibility=false;
    }
}
