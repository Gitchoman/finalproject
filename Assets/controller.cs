using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    private Animator animator; 
    private Vector2 velocity;           //移動方向
    private float movespeed = 0.1f;     //移動速度
    private Vector2 move;               //方向&速度
    
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        //Debug.Log("Hello World!");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        //WASD入力より進行方向を決定
        velocity = Vector2.zero;

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movespeed = 0.1f;
        }
        else
        {
            movespeed = 0.05f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= 1;
            animator.SetBool("right",false);
            animator.SetBool("left",true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x += 1;
            animator.SetBool("right",true);
            animator.SetBool("left",false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //velocity.y += 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //velocity.y -= 1;
        }

        //移動先を決定
        move = velocity.normalized * movespeed;

        //位置の更新
        if(move.magnitude > 0){
            Debug.Log(velocity);
            position += move;
            animator.SetBool("walking",true);
            if(Input.GetKey(KeyCode.LeftShift))
            {
                movespeed = 0.3f;
                animator.SetBool("running",true);
            }
            else
            {
                movespeed = 0.1f;
                animator.SetBool("running",false);
            }
            transform.position = position;
        }
        else{
            animator.SetBool("walking",false);
            animator.SetBool("running",false);
        }
    }
}
