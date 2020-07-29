using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private const int MAX_POWER = 100;
    private const int MIN_POWER = 0;
    public static float power;
    public float consumeRate = 1f;
    public bool boatCanMove;
    private int boatStatus = 0;     

    /* public bool isCharging;
     public int chargeRate = 3;
     private float chargeStart;
     private float chargeEnd;*/

    // Start is called before the first frame update
    void Start()
    {
        GameObject battery = GameObject.Find("Battery");
        power = MAX_POWER;
        UnityEngine.Debug.Log("Battery start, and power is " + power);
    }

    // Update is called once per frame
    void Update()
    {
        //update boat status
        if (power > 0)
        {
            boatStatus = 0;
            power = MAX_POWER - Time.time * consumeRate; //Time.time: number of seconds from the start of game
        }
        else if (boatStatus <= 1)
        {
            power = MIN_POWER;
            boatStatus++;
        }

        //boat stop condition
        if (boatStatus == 1)
        {
            UnityEngine.Debug.Log("The battery runs out off power, boat stopped.");
        }
        if (boatStatus == 0)
        {
            boatCanMove = true;
        }
        else
        {
            boatCanMove = false;
        }

        //battery recharge
        /*if (isCharging)
       {

       }*/

    }
}
