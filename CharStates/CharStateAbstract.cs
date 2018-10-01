using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharStateAbstract : MonoBehaviour {
    //Abstract class that is the general state that will be swapt out for player movement behavior
    Animator anim;
    public abstract void run();

    public void setAnimator(){anim = GetComponent<Animator>();}
    public void setBoolIsMovingToTrue(){anim.SetBool("isWalking", true);}
    public void setBoolIsMovingToFalse(){anim.SetBool("isWalking", false);}
    public void setAnimInputYTo1(){anim.SetFloat("inputY", 1);}
    public void setAnimInputYToNeg1(){anim.SetFloat("inputY", -1);}
    public void setAnimInputXToNeg1(){anim.SetFloat("inputX", -1);}
    public void setAnimInputXTo0() { anim.SetFloat("inputX",0);}
    public void setAnimInputYTo0(){anim.SetFloat("inputY",0);}
    public void debugAnim()
    {
        Debug.Log("XAnim:" + anim.GetFloat("inputX"));
        Debug.Log("YAnim:" + anim.GetFloat("inputY"));
        Debug.Log("isWalking:" + anim.GetBool("isWalking"));
    }
}
