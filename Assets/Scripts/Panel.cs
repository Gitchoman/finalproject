using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panel : MonoBehaviour
{
    private Animator animator;
    private GameObject capy;
    private controller script;
    private int hp;
    public GameObject loadcapy;

    void Start(){
        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
        animator = GetComponent<Animator>();
    }

    void Update(){
        hp = script.hp;
        if(hp<1){
            animator.SetBool("death",true);
        }
    }

    private void Load(){
        StartCoroutine(LoadScene());
        loadcapy.SetActive(true);
    }

    private IEnumerator LoadScene() {
        var async = SceneManager.LoadSceneAsync("Title");

        async.allowSceneActivation = false;
        yield return new WaitForSeconds(2);
        async.allowSceneActivation = true;
    }
    
    private void DisPanel()
    {
        this.gameObject.SetActive(false);    
    }
}
