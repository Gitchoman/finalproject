using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    private Animator animator; 
    private Rigidbody2D capybody;
    public AudioClip bite;
    private int groundtag = 0;      //0:Ground,1:Platform
    public AudioClip dash01;
    public AudioClip dash02;
    public AudioClip dashplat01;
    public AudioClip dashplat02;
    private AudioSource audioSource;
    private GameObject attackDTC;
    private BoxCollider2D attackDTCCol;
    private GameObject MC;
    private MainCamera script;
    private Vector2 velocity;        //移動方向
    private float movespeed = 0.20f; //移動速度
    private float chargespeed = 20f; //突進速度
    private Vector2 move;            //方向&速度
    public int hp = 6;               //体力
    private int jumpcnt;             //ジャンプ回数
    private bool movel = false;      //左移動フラグ  
    private bool mover = false;      //右移動フラグ
    public bool right = true;        //向き
    private bool charge = false;     //突進中フラグ
    private bool attack = false;     //攻撃フラグ
    private bool aircharge = false;  //空中突進
    private bool inair = false;      //空中にいるかどうか
    private bool damage = false;
    
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        animator = GetComponent<Animator>();
        capybody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        attackDTC = transform.GetChild(0).gameObject;
        attackDTC.transform.position = new Vector2(0.14f,0.05f);
        attackDTCCol = attackDTC.GetComponent<BoxCollider2D>();
        attackDTCCol.enabled = false;
        MC = GameObject.Find("Main Camera");
        script = MC.GetComponent<MainCamera>();
        velocity = Vector2.zero;
    }

    //Colliderどうしが衝突したときに呼び出される
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")||collision.gameObject.CompareTag("Platform")){
            //Debug.Log(inair);
            inair = false;
            aircharge = false;
            animator.SetBool("inair",false);
            animator.SetBool("jump",false);
            jumpcnt = 0;
            if(collision.gameObject.CompareTag("Ground")){
                groundtag = 0;
            }
            else if(collision.gameObject.CompareTag("Platform")){
                groundtag = 1;
            }
        }

        if(!charge&&collision.gameObject.CompareTag("Enemy")){
            //Debug.Log("あたった");
            damage = true;
            animator.SetBool("damage",true);
            if(!right){
                this.capybody.AddForce(transform.right*300f);
            }
            else{
                this.capybody.AddForce(transform.right*-300f);
            }
        }
    }

    //Colliderどうしが離れたときに呼び出される
    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Ground")){
            //Debug.Log(inair);
            animator.SetBool("inair",true);
            inair = true;
            if(jumpcnt==0){
                jumpcnt += 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger){
        if(trigger.gameObject.CompareTag("Enemyhead")){
            //Debug.Log("踏んだ");
            capybody.velocity = new Vector2(velocity.x,0);
            this.capybody.AddForce(transform.up*700f);
            animator.SetBool("jump",true);
        }

        if(!charge&&trigger.gameObject.CompareTag("EnemyATDTC")){
            //Debug.Log("あたった");
            damage = true;
            animator.SetBool("damage",true);
            if(!right){
                this.capybody.AddForce(transform.right*300f);
            }
            else{
                this.capybody.AddForce(transform.right*-300f);
            }
        }
    }

    //Button_Move_Lを押したとき(同時押しに対応するためにフラグのみを操作)
    public void OnDownL(){
        //Debug.Log("Lmove");
        movel = true;
        right = false;
        script.SaveLastPOS();
    }

    //Button_Move_Lを離したとき
    public void OnUpL(){
        movel = false;
    }

    //Button_Move_Rを押したとき
    public void OnDownR(){
        //Debug.Log("Rmove");
        mover = true;
        right = true;
        script.SaveLastPOS();
    }

    //Button_Move_Rを離したとき
    public void OnUpR(){
        mover = false;
    }

    public void DashSE01(){
        if(groundtag == 0){
            audioSource.PlayOneShot(dash01);
        }
        else if(groundtag == 1){
            audioSource.PlayOneShot(dashplat01);
        }
        
    }

    public void DashSE02(){
        if(groundtag == 0){
            audioSource.PlayOneShot(dash02);
        }
        else if(groundtag == 1){
            audioSource.PlayOneShot(dashplat02);
        }
        
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
        //Debug.Log("chargeキャンセル");
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
            audioSource.PlayOneShot(bite);
        }
    }

    public void ENAAttackDTC(){
        Vector2 position = transform.position; 
        if(right){
            attackDTC.transform.position = position + new Vector2(0.85f,0.3f);
        }
        else{
            attackDTC.transform.position = position + new Vector2(-0.85f,0.3f);
        }

        attackDTCCol.enabled = true;
    }

    public void DISAttackDTC(){
        attackDTCCol.enabled = false;
    }



    //attackアニメーション終了時
    public void AttackCXL(){
        //Debug.Log("attackキャンセル");
        attack = false;
        animator.SetBool("attack",false);
    }

    public void Damage(){
        hp -= 1;
        //Debug.Log(hp);
        animator.SetBool("damage",false);
    }

    public void DamageCXL(){
        damage = false;
        //animator.SetBool("damage",false);
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
                //right = false;
            }
            else if(mover){
                velocity.x = 1;
                animator.SetBool("right",true);
                animator.SetBool("left",false);
                //right = true;
            }
            else{
                velocity.x = 0;
            }
        }

        //移動量を決定
        move = velocity.normalized * movespeed;

        //位置の更新
        if(move.magnitude > 0&&!charge&&!attack&&!damage){
            //Debug.Log(velocity);
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
            //Debug.Log("落下");
            animator.SetBool("fall",true);
        }
        else{
            animator.SetBool("fall",false);
        }

        //ジャンプできなくなるバグ対策
        if(!inair){
            jumpcnt = 0;
        }
    }

    
}
