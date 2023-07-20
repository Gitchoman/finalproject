using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private Animator animator;
    private GameObject capy;
    private controller script;
    public int capyhp;
    private int beforehp;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        capy = GameObject.Find("Capy");
        script = capy.GetComponent<controller>();
        capyhp = script.hp;
        beforehp = capyhp;
    }

    // Update is called once per frame
    void Update()
    {
        beforehp = capyhp;
        capyhp = script.hp;
        if(capyhp != beforehp){
            animator.SetBool("trans",true);
        }
        else{
            animator.SetBool("trans",false);
        }

        animator.SetInteger("hp",capyhp);
        Debug.Log(capyhp);
    }
}
