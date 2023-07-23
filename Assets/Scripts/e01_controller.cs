using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e01_controller : MonoBehaviour
{
    private Animator animator; 
    private Rigidbody2D body;
    public AudioClip sword;
    public AudioClip arm;
    private AudioSource audioSource;
    private Vector2 initialPOS;
    private Vector2 velocity;        //移動方向
    private float movespeed = 0.20f; //移動速度
    private Vector2 move;            //方向&速度
    private bool right = true;       //向き
    private int status = 1;          //1:徘徊 2:追跡
    private bool find = false;        //追跡フラグ
    private bool attack = false;     //攻撃フラグ
    private GameObject attackDTC;
    private BoxCollider2D attackDTCCol;
    private bool damage = false;
    public bool capyright;
    private GameObject capy;
    private Vector2 capyPOS;
    private controller script;
    private int capyhp;
    private Vector2 dist;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        attackDTC = transform.GetChild(0).gameObject;
        attackDTC.transform.position = transform.position;
        attackDTCCol = attackDTC.GetComponent<BoxCollider2D>();
        attackDTCCol.enabled = false;

        initialPOS = transform.position;
        velocity = Vector2.zero;

        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(status == 1){
            if(collision.gameObject.CompareTag("Enemy")){
                if(right){
                    right = false;
                }
                else{
                    right = true;
                }
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger){
        capyright = script.right;
        if(trigger.gameObject.CompareTag("AttackDetection")){
            damage = true;
            animator.SetBool("damage",true);
            if(!capyright){
                this.body.AddForce(transform.right*-300f);
            }
            else{
                this.body.AddForce(transform.right*300f);
            }
        }
        
        if(status == 1){
            if(trigger.gameObject.CompareTag("PlatformR")){
                Debug.Log(right);
                right = false;
            }
            else if(trigger.gameObject.CompareTag("PlatformL")){
                Debug.Log(right);
                right = true;
            }
        }
    }

    private void Damage(){
        this.gameObject.SetActive(false);
        Invoke("Respawn",5);
    }

    private void Attack(){
        attack = true;
        if(dist.x<0){
            right = true;
            animator.SetBool("right",true);
        }
        else{
            right = false;
            animator.SetBool("right",false);
        }
        animator.SetBool("attack",true);
    }

    public void ArmSE(){
        audioSource.PlayOneShot(arm);
    }    

    public void AttackSE(){
        audioSource.PlayOneShot(sword);
    }

    

    public void ENAAttackDTCFW(){
        Vector2 position = transform.position; 
        if(right){
            attackDTC.transform.position = position + new Vector2(0.85f,-0.4f);
        }
        else{
            attackDTC.transform.position = position + new Vector2(-0.85f,-0.4f);
        }

        attackDTCCol.enabled = true; 
    }

    public void ENAAttackDTCBK(){
        Vector2 position = transform.position; 
        if(right){
            attackDTC.transform.position = position + new Vector2(-0.7f,-0.4f);
        }
        else{
            attackDTC.transform.position = position + new Vector2(0.7f,-0.4f);
        }

        attackDTCCol.enabled = true;
    }

    public void DISAttackDTC(){
        attackDTCCol.enabled = false;
    }

    private void AttackCXL(){
        attack = false;
        animator.SetBool("attack",false);
    }

    private void Find(){
        find = true;
        status = 2;
        if(dist.x<0){
            right = true;
            animator.SetBool("right",true);
        }
        else{
            right = false;
            animator.SetBool("right",false);
        }
        animator.SetBool("find",true);
    }

    private void FindCXL(){
        find = false;
        animator.SetBool("find",false);
    }

    private void Respawn(){
        damage = false;
        attack = false;
        find = false;
        right = true;
        status = 1;
        transform.position = initialPOS;
        this.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        capyPOS = capy.transform.position;

        //capyとe01の距離、xが負なら右にcapy、yが負なら上にcapy
        dist = position - capyPOS;

        movespeed = 0.02f;

        if(Mathf.Abs(dist.x)<2.5&&Mathf.Abs(dist.y)<1){
            Attack();
        }
        else if(Mathf.Abs(dist.x)<10&&Mathf.Abs(dist.y)<3){
            if(status == 1){
                Find();
            }
        }
        else if(Mathf.Abs(dist.x)>30){
            status = 1;
        }

        //移動方向を定める部分
        if(!attack&&!damage){
            switch (status)
            {
                case 1:
                    if(!right){
                        velocity.x = -1;
                        animator.SetBool("right",false);
                    }
                    else{
                        velocity.x = 1;
                        animator.SetBool("right",true);
                    }
                break;
                case 2:
                    if(dist.x>0){
                        velocity.x = -1;
                        animator.SetBool("right",false);
                    }
                    else{
                        velocity.x = 1;
                        animator.SetBool("right",true);
                    }
                break;
                default:
                break;
            }
            
        }

        //移動先を決定
        move = velocity.normalized * movespeed;

        if(move.magnitude > 0&&!attack&&!damage&&!find){
            Debug.Log(velocity);
            position += move;
            transform.position = position;
        }

        capyhp = script.hp;
        if(capyhp<1){
            this.gameObject.SetActive(false);   
        }
    }
}
