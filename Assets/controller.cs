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
    public bool movel = false;
    public bool mover = false;
    public bool right = true;       //向き
    public bool charge = false;     //突進中フラグ
    public bool attack = false;     //攻撃フラグ
    public bool aircharge = false;  //空中突進
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
            aircharge = false;
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
            if(jumpcnt==0){
                jumpcnt += 1;
            }
        }
    }

    //Button_Move_Lを押したとき(同時押しに対応するためにフラグのみを操作)
    public void OnDownL(){
        Debug.Log("Lmove");
        movel = true;
    }

    //Button_Move_Lを離したとき
    public void OnUpL(){
        movel = false;
    }

    //Button_Move_Rを押したとき
    public void OnDownR(){
        Debug.Log("Rmove");
        mover = true;
    }

    //Button_Move_Rを離したとき
    public void OnUpR(){
        mover = false;
    }

    //Button_Chargeを押したとき
    public void Charge(){
        if(!charge&&!aircharge&&!attack){
            capybody.velocity = new Vector2(velocity.x,0);
            charge = true;
            animator.SetBool("charge",true);
            animator.SetBool("running",false);
            animator.SetBool("jump",false);
            if(inair){
                aircharge = true;
            }
        }
    }

    //chargeアニメーション中に呼び出される（加速）
    public void ChargeAT(){
        if(right){
            chargespeed = 20f;
        }
        else{
            chargespeed = -20f;
        }
    }

    //chargeアニメーション中に呼び出される（減速）
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

    //Button_Attackを押したとき
    public void Attack(){
        if(!attack&&!inair&&!charge){
            attack = true;
            animator.SetBool("attack",true);
            animator.SetBool("running",false);
            animator.SetBool("jump",false);
        }
    }

    //attackアニメーション終了時
    public void AttackCXL(){
        Debug.Log("attackキャンセル");
        attack = false;
        animator.SetBool("attack",false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        
        movespeed = 0.20f;

        //移動方向を定める部分
        if(!charge&&!attack){
            if(movel){
                velocity.x = -1;
                animator.SetBool("right",false);
                animator.SetBool("left",true);
                right = false;
            }
            else if(mover){
                velocity.x = 1;
                animator.SetBool("right",true);
                animator.SetBool("left",false);
                right = true;
            }
            else{
                velocity.x = 0;
            }
        }

        //移動先を決定
        move = velocity.normalized * movespeed;

        //位置の更新
        if(move.magnitude > 0&&!charge&&!attack){
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
