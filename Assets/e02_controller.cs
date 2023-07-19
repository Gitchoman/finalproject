using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class e02_controller : MonoBehaviour
{
    private Animator animator; 
    private Rigidbody2D body;
    private BoxCollider2D bodycollider;
    private Vector2 collideroffset;
    private Vector2 velocity;        //移動方向
    private float movespeed = 0.20f; //移動速度
    private Vector2 move;            //方向&速度
    private bool right = true;       //向き
    private bool attack = false;     //攻撃フラグ

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        bodycollider = GetComponent<BoxCollider2D>();
        collideroffset = bodycollider.offset;
        Debug.Log(collideroffset);
        velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D trigger){
        if(trigger.gameObject.CompareTag("PlatformR")){
            Debug.Log(right);
            right = false;
            bodycollider.offset = new Vector2(0.13f,-0.18f);
        }
        else if(trigger.gameObject.CompareTag("PlatformL")){
            Debug.Log(right);
            right = true;
            bodycollider.offset = new Vector2(-0.13f,-0.18f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        movespeed = 0.01f;

        //移動方向を定める部分
        if(!attack){
            if(!right){
                velocity.x = -1;
                animator.SetBool("right",false);
            }
            else{
                velocity.x = 1;
                animator.SetBool("right",true);
            }
        }

        //移動先を決定
        move = velocity.normalized * movespeed;

        if(move.magnitude > 0&&!attack){
            Debug.Log(velocity);
            position += move;
            transform.position = position;
        }
    }
}