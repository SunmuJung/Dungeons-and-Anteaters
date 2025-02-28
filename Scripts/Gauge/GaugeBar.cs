using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GaugeBar : MonoBehaviour
{
    private Slider gaugeSlider;
    private bool m_isMax;

    void Start()
    {
        gaugeSlider = GetComponent<Slider>();
    }

    public bool isMax()
    {
        return m_isMax;
    }
    
    public void setMax(int maxVal)
    {
        gaugeSlider.maxValue = maxVal;
    }

    // sets gauge's value to the given int value.
    // sets m_isMax to true if the gauge is full.
    public void setVal(int val)
    {
        gaugeSlider.value = val;
        m_isMax = gaugeSlider.value >= gaugeSlider.maxValue ? true : false;
    }
}
