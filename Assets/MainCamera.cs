using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject capy;
    private controller script;
    public bool capyright;
    private Transform playermove;
    private Vector2 lastMCPOS;
    private Vector2 targetMCPOS;
    private float SHMscale;

    // Start is called before the first frame update
    void Start()
    {
        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
        playermove = capy.transform;
    }

    public void SaveLastPOS(){
        capyright = script.right;
        lastMCPOS = transform.position;
        if(capyright){
            targetMCPOS = new Vector2(playermove.position.x + 3,playermove.position.y);
        }
        else{
            targetMCPOS = new Vector2(playermove.position.x - 3,playermove.position.y);
        }
        SHMscale = Mathf.Abs(lastMCPOS.x - targetMCPOS.x);
        
        Debug.Log("SHMscale"+ SHMscale);
        Debug.Log("カメラ"+ lastMCPOS);
    }

    //Updateの後に呼び出される
    void LateUpdate()
    {
        MoveMC();
    }

    void MoveMC(){
        Debug.Log("SHMscale"+ SHMscale);
        transform.position = new Vector3(playermove.position.x,playermove.position.y,-10);
    }
}
