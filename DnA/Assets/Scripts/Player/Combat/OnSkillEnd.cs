using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSkillEnd : MonoBehaviour
{
    private PlayerStatus status;

    public void OnAnimationEnd()
    {
        status = GetComponent<PlayerStatus>();
        status.isSkill = false;
    }
}
