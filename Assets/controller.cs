using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    private Animator animator; 
    private Rigidbody2D capybody;
    public Vector2 velocity;        //移動方向
    public float movespeed = 0.20f; //移動速度
    public float chargespeed = 20f; //突進速度
    public Vector2 move;            //方向&速度
    public int jumpcnt;             //ジャンプ回数
    public bool right = true;       //向き
    public bool charge = false;     //チャージ中フラグ
    public bool inair = false;      //空中にいるかどうか
    
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        animator = GetComponent<Animator>();
        capybody = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }

    //Colliderどうしが衝突したときに呼び出される
    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground")){
            Debug.Log(inair);
            inair = false;
            animator.SetBool("inair",false);
            animator.SetBool("jump",false);
            jumpcnt = 0;
        }
    }

    //Colliderどうしが離れたときに呼び出される
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground")){
            Debug.Log(inair);
            animator.SetBool("inair",true);
            inair = true;
        }
    }

    //Button_Move_Lを押したとき
    public void OnDownL(){
        if(!charge){
            velocity.x -= 1;
            animator.SetBool("right",false);
            animator.SetBool("left",true);
            right = false;
        }
    }

    //Button_Move_Lを離したとき
    public void OnUpL(){
        velocity.x = 0;
    }

    //Button_Move_Rを押したとき
    public void OnDownR(){
        if(!charge){
            velocity.x += 1;
            animator.SetBool("right",true);
            animator.SetBool("left",false);
            right = true;
        }
    }

    //Button_Move_Rを離したとき
    public void OnUpR(){
        velocity.x = 0;
    }

    //Button_Chargeを押したとき
    public void Charge(){
        if(!charge){
            capybody.velocity = new Vector2(velocity.x,0);
            charge = true;
            animator.SetBool("charge",true);
            animator.SetBool("running",false);
            animator.SetBool("jump",false);
        }
    }

    public void ChargeAT(){
        if(right){
            chargespeed = 20f;
        }
        else{
            chargespeed = -20f;
        }
    }

    public void ChargeDC(){
        if(right){
            chargespeed = 10f;
        }
        else{
            chargespeed = -10f;
        }

    }

    //Chargeのアニメーションが終了したときに呼び出される
    public void ChargeCXL(){
        Debug.Log("chargeキャンセル");
        capybody.velocity = new Vector2(velocity.x,0);
        charge = false;
        animator.SetBool("charge",false);
    }

    //Button_Jumpを押したとき
    public void Jump(){
        if(jumpcnt<2){
            charge = false;
            animator.SetBool("charge",false);
            capybody.velocity = new Vector2(velocity.x,0);
            this.capybody.AddForce(transform.up*700f);
            animator.SetBool("jump",true);
            jumpcnt += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        
        movespeed = 0.20f;

        //移動先を決定
        move = velocity.normalized * movespeed;

        //位置の更新
        if(move.magnitude > 0&&!charge){
            Debug.Log(velocity);
            position += move;
            animator.SetBool("running",true);
            transform.position = position;
        }
        else if(charge){
            capybody.velocity = new Vector2(chargespeed,0);
        }
        else{
            animator.SetBool("running",false);
        }

        if(capybody.velocity.y<0){
            Debug.Log("落下");
            animator.SetBool("fall",true);
        }
        else{
            animator.SetBool("fall",false);
        }
    }

    
}
