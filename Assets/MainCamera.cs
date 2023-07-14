using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject obj;
    controller script;
    Transform playermove;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Capy");
        script = obj.GetComponent<controller>();
        playermove = obj.transform;
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
