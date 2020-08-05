using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    private const float MAX_POWER = 100;
    private const float MIN_POWER = 0;
    public static float power;
    public float consumeRate = 1f;
    private int boatStatus = 0;

    public PowerBar PowerBar;
    
    public Text PowerText;


    /* public bool isCharging;
     public int chargeRate = 3;
     private float chargeStart;
     private float chargeEnd;*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject battery = GameObject.Find("Battery");
        power = MAX_POWER;
        PowerBar.setMaxPower(MAX_POWER);
        PowerText.text = Math.Round(power).ToString() + "%";
        Debug.Log("Battery start, and power is " + power);
    }

    // Update is called once per frame
    void Update()
    {
        PowerBar.setPower(power);
        PowerText.text = Math.Round(power).ToString() + "%";
        //update boat status
        if (power > 0)
        {
            boatStatus = 0;
            power = MAX_POWER - Time.time * consumeRate; //Time.time: number of seconds from the start of game
            Debug.Log(power);
        }
        else if (boatStatus <= 1)
        {
            power = MIN_POWER;
            boatStatus++;
        }

        //boat stop condition
        if (boatStatus == 1)
        {
            Debug.Log("The battery runs out off power, boat stopped.");
            //Change Boat color to red. 
            /*GameObject boatHull = GameObject.Find("BoatHull");
            var boatRenderer = boatHull.GetComponent<Renderer>();
            boatRenderer.material.SetColor("_Color", Color.red);*/
        }
      
        //battery recharge
        /*if (isCharging)
       {
       }*/

    }

   
}