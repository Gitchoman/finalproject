using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject capy;
    controller script;
    Transform playermove;

    // Start is called before the first frame update
    void Start()
    {
        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
        playermove = capy.transform;
    }

    //Updateの後に呼び出される
    void LateUpdate()
    {
        MoveMC();
    }

    void MoveMC(){
        transform.position = new Vector3(playermove.position.x,playermove.position.y,-10);
    }
}
