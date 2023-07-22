using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    private Animator animator; 

    void Start(){
        animator = GetComponent<Animator>();
    }

    public void ChangeScene(){
        animator.SetBool("credits",true);
        Invoke("Change",1);
    }

    private void Change(){
        SceneManager.LoadScene("Credits");
    }
}
