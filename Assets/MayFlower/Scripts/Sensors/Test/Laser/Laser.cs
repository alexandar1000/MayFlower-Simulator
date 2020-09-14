using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

    public class Laser : MonoBehaviour
{
        public GameObject frontCenterLaser;
        public float sensorLength = 5f;
        public float frontSensorAngle = 30;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;

            //Front Centre Sensor
            if (Physics.Raycast(frontCenterLaser.transform.position, frontCenterLaser.transform.forward, out hit, sensorLength))
            {
                //Debug.Log("front center laser: "+ hit.transform.name + " distance is "+ hit.distance);    
            }

            Debug.DrawLine(frontCenterLaser.transform.position, hit.point);

            if (Time.time > nextActionTime)
            {
                nextActionTime += period;
            }

        }
    }


