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
    public bool isWalking;
    public string curStateName;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        curStateName = "idle";
    }

    void Update()
    {
        updateIsAttacking();
    }

    void updateIsAttacking()
    {
        isAttacking = isBasicAttacking || isSkill;
    }
}
