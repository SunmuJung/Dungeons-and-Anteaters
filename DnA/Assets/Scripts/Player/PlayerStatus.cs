using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Animator anim;

    public bool isBasicAttacking;
    public bool isSkill;
    public bool isAir;
    public bool isAttacking;
    public string curStateName;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        curStateName = "idle";
    }

    void Update()
    {
        updateIsAttacking();
        /*

        Debug.Log(!anim.GetCurrentAnimatorStateInfo(0).IsTag("skill"));
        // if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && isSkill == true)
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("skill") && isSkill == true)
        {
            isSkill = false;
        }
        //*/
    }

    void updateIsAttacking()
    {
        isAttacking = isBasicAttacking || isSkill;
    }
}
