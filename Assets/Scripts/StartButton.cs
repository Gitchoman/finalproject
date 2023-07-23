using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    private Animator animator; 
    public GameObject panel;
    public GameObject loadcapy;

    void Start(){
        Application.targetFrameRate = 60; //FPSを60に設定
        animator = GetComponent<Animator>();
    }

    //Changeボタンが押されたときの処理
    public void ChangeScene(){
        animator.SetBool("start",true);
    }

    private void Load(){
        panel.SetActive(true);
        loadcapy.SetActive(true);
        animator.SetBool("start",false);
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene() {
        var async = SceneManager.LoadSceneAsync("Scene01");

        async.allowSceneActivation = false;
        yield return new WaitForSeconds(2);
        async.allowSceneActivation = true;
    }
}
