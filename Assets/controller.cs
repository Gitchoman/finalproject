using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    private Animator animator; 
    public Vector2 velocity;                   //移動方向
    public float movespeed = 0.1f;     //移動速度
    private Vector2 move;               //方向&速度
    
    void Start()
    {
        Application.targetFrameRate = 60; //FPSを60に設定 
        //Debug.Log("Hello World!");
        animator = GetComponent<Animator>();
        velocity = Vector2.zero;
    }

    public void OnDownL(){
        velocity.x -= 1;
        animator.SetBool("right",false);
        animator.SetBool("left",true);
    }

    public void OnUpL(){
        velocity.x = 0;
    }

    public void OnDownR(){
        velocity.x += 1;
        animator.SetBool("right",true);
        animator.SetBool("left",false);
    }

    public void OnUpR(){
        velocity.x = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;

        //WASD入力より進行方向を決定
        

        if(Input.GetKey(KeyCode.LeftShift))
        {
            movespeed = 0.40f;
        }
        else
        {
            movespeed = 0.20f;
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
