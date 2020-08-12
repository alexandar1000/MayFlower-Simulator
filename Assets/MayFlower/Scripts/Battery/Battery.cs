using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    private const float MAX_POWER = 100;
    public static float power;
    public float consumeRate = 1f;
    public static int boatStatus = 0;

    public PowerBar powerBar;
    
    public Text powerText;

    public bool atHomeArea = false;
    [NonSerialized] public GameObject homeZone;

    // Start is called before the first frame update
    void Start()
    {
        GameObject battery = GameObject.Find("Battery");
        power = MAX_POWER;
        powerBar.setMaxPower(MAX_POWER);
        powerText.text = Math.Round(power).ToString() + "%";
    }

    // Update is called once per frame
    void Update()
    {
        powerBar.setPower(power);
        powerText.text = Math.Round(power).ToString() + "%";

        if (atHomeArea){
            boatStatus = 0;
            power += Time.deltaTime * consumeRate;
            if (power > MAX_POWER)
            {
                power = MAX_POWER;
            }
        }

        else{
            //update boat status
            if (power > 0) {
                boatStatus = 0;
                power -= Time.deltaTime * consumeRate; //Time.time: number of seconds from the start of game                
            }
            else{
                boatStatus = 1;
            }

            //boat stop condition
            if (boatStatus == 1){
                Debug.Log("The battery runs out off power, boat stopped.");
            }

        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("HomeArea"))
        {
            homeZone = coll.gameObject;
            atHomeArea = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("HomeArea"))
        {
             atHomeArea = false;
        }

    }


}