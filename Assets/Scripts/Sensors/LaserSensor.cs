using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


public class LaserSensor : MonoBehaviour
{
    [Header("Laser Sensor")]
    public GameObject env;
    public GameObject frontCenterLaser;
    public GameObject frontRightLaser;
    public GameObject frontLeftLaser;
    public GameObject rightLaser;
    public GameObject leftLaser;
    

    //public GameObject backLaser;
    //public float distance;

    [Header("Laser Settings")]
    public float sensorLength = 5f;
    public float frontSensorAngle = 30;


    // Update is called once per frame
    void Update()
    {
        laserSensors();
    }

    private void laserSensors()
    {
        RaycastHit hit;

        //Front Centre Sensor
        if (Physics.Raycast(frontCenterLaser.transform.position, frontCenterLaser.transform.forward, out hit, sensorLength))
        {
            Debug.Log("front center laser"+hit.transform.name);
            

        }
        Debug.DrawLine(frontCenterLaser.transform.position, hit.point);

        //Front Right Side Sensor
        if (Physics.Raycast(frontRightLaser.transform.position, frontRightLaser.transform.forward, out hit, sensorLength))
        {
            Debug.Log("front right laser" + hit.transform.name);
        }
        Debug.DrawLine(frontRightLaser.transform.position, hit.point);


        //Front Left side Sensor
        if (Physics.Raycast(frontLeftLaser.transform.position, frontLeftLaser.transform.forward, out hit, sensorLength))
        {
            Debug.Log("front left laser" + hit.transform.name);
        }
        Debug.DrawLine(frontLeftLaser.transform.position, hit.point);

        //Right Angle Sensor
        if (Physics.Raycast(rightLaser.transform.position, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.Log("left angle laser" + hit.transform.name);
        }

        //Left Angle Sensor
        if (Physics.Raycast(leftLaser.transform.position, Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward, out hit, sensorLength))
        {
            Debug.Log("right angle laser" + hit.transform.name);
        }


        //Publish(Util.PrepareMessage(distance));

    }

}

