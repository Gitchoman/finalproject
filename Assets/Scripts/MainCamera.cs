using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private GameObject capy;
    private controller script;
    public bool capyright;
    private bool lastcapyright;
    private Transform playermove;
    private Vector2 lastMCPOS;
    private Vector2 MCPOS;
    private Vector2 targetMCPOS;
    private float SHMscale;
    private Vector2 halfway;
    private Vector2 move;
    private float rad;

    // Start is called before the first frame update
    void Start()
    {
        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
        playermove = capy.transform;
    }

    //移動ボタンが押されたときに現在のカメラ位置を記録
    public void SaveLastPOS(){
        Debug.Log("SaveLastPOS");

        capyright = script.right;

        //現在のカメラ位置
        lastMCPOS = transform.position;

        //カメラの目標位置を決定
        if(capyright){
            targetMCPOS = new Vector2(playermove.position.x + 2,playermove.position.y);
        }
        else{
            targetMCPOS = new Vector2(playermove.position.x - 2,playermove.position.y);
        }

        //目標位置と現在地から単振動のスケールを決定
        SHMscale = Mathf.Abs(lastMCPOS.x - targetMCPOS.x)/2;

        //回転中心
        halfway = lastMCPOS/2 + targetMCPOS/2 - new Vector2(playermove.position.x,playermove.position.y);

        //角度のリセット
        rad = -Mathf.PI/2;

        lastcapyright = capyright;
        
        //Debug.Log("SHMscale:"+ SHMscale);
        //Debug.Log("halfway:"+halfway);
        //Debug.Log("lastMCPOS:"+lastMCPOS);
        //Debug.Log("targetMCPOS:"+targetMCPOS);
        //Debug.Log("カメラ"+ lastMCPOS);
    }

    void Update(){
        capyright = script.right;
    }

    //Updateの後に呼び出される
    void LateUpdate()
    {
        MoveMC();
    }

    void MoveMC(){
        MCPOS = transform.position;
        
        if(rad <= Mathf.PI/2){
            rad += Mathf.PI/80;
        }

        if(capyright){
            move = halfway + new Vector2(SHMscale * Mathf.Sin(rad),0);
        }
        else{
            move = halfway + new Vector2(SHMscale * -Mathf.Sin(rad),0);
        }

        

        transform.position = new Vector3(playermove.position.x,playermove.position.y,-10) + new Vector3(move.x,0,0);

        //transform.position = new Vector3(playermove.position.x,playermove.position.y,-10);
    }
}
