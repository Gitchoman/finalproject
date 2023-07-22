using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private Animator animator; 

    void Start(){
        Application.targetFrameRate = 60; //FPSを60に設定
        animator = GetComponent<Animator>();
    }

    public void ChangeScene(){
        animator.SetBool("start",true);
        Invoke("Change",1);
    }

    private void Change(){
        SceneManager.LoadScene("Scene01");
    }
}
