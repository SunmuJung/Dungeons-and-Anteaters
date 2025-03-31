using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolDown : MonoBehaviour
{
    // 0: frost, 1: flame, 2: dash
    public Image[] fill;
    public float[] cooldownTime;
    private float[] timeLeft = new float[3];

    // Start is called before the first frame update
    public void StartCooldown(int skillNum)
    {
        StartCoroutine(CooldownRoutine(skillNum));
    }

    IEnumerator CooldownRoutine(int skillNum)
    {
        timeLeft[skillNum] = cooldownTime[skillNum];

        while(timeLeft[skillNum] > 0)
        {
            timeLeft[skillNum] -= Time.deltaTime;
            fill[skillNum].fillAmount = timeLeft[skillNum] / cooldownTime[skillNum];
            yield return null;
        }

        fill[skillNum].fillAmount = 0f;
    }
}
