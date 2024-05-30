using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPad : MonoBehaviour
{
    public float jumpForce;    
    
    private void OnTriggerEnter(Collider other)
    {        
        if(other.gameObject.GetComponent<Rigidbody>())
        {   
            StartCoroutine(MoveLock());
            Vector3 forceDirection = (Vector3.forward/2 + Vector3.up).normalized;            
            other.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * jumpForce, ForceMode.Impulse); 
        }
    }
    IEnumerator MoveLock()
    {
        CharacterManager.Instance.Player.controller.isMove=false;
        yield return new WaitForSeconds(0.8f);
        CharacterManager.Instance.Player.controller.isMove=true;
    }
}


