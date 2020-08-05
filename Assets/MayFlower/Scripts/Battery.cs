using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public Slider slider; 
    public Gradient gradient;
    public Image fill;



    public void setMaxPower(int power){
        slider.maxValue = power;
        slider.value =  power;
        // fill.color = gradient.Evaluate(1f);
    }
    public void setPower(int power){
        slider.value = power;
 
    }

}