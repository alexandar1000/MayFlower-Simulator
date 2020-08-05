using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public Text powerText;

    public void setMaxPower(float power)
    {
        slider.maxValue = 100;
        slider.value = power;
        fill.color = gradient.Evaluate(1f);

    }
    public void setPower(float power)
    {
        slider.value = power;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
