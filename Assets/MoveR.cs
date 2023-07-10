using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveR : MonoBehaviour
{
    GameObject obj;
    controller script;
    public Vector2 velocity;
    
    void Start(){
        GameObject obj = GameObject.Find("Capy");
        script = obj.GetComponent<controller>();
    }
    public void PushDown()
    {
        //velocity = script.velocity;
        Debug.Log("押された!");
        script.velocity.x += 1;
        script.movespeed = 0.40f;
    }
}
