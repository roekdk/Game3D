using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPad : MonoBehaviour
{
    Rigidbody pad;
    Vector3 pointA=new Vector3(1.4f, 0.2f,2f);
    Vector3 pointB=new Vector3(-6f, 0.2f,2f);
    public float speed;
    private Vector3 target;
    public bool moveToDestination;
    void Start()
    {
        pad=GetComponent<Rigidbody>();   
        //StartCoroutine(Moving());     
    }
    void FixedUpdate() 
    {
        target = moveToDestination ?  pointA: pointB;
        pad.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
        if(Vector3.Distance(transform.position, target) < 0.1f){moveToDestination= !moveToDestination;}
                
    }    
    // IEnumerator Moving()
    // { 
    //     yield return new WaitForSeconds(3.0f);
    //     while (Vector3.Distance(transform.position, target) > 0.001f)
    //     {
    //         pad.MovePosition(Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime));
    //         yield return null;
    //     }
    //     target = target == pointA ? pointB : pointA; 
    // }      
}
