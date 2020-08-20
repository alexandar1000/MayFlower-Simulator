﻿/* 
Implementation of the GPS sensor which sends current GPS coordinates of the main vessel
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.Diagnostics;
using System.Security.Cryptography;

namespace RosSharp.RosBridgeClient
{
    public class GPSSensor : UnityPublisher<MessageTypes.Sensor.NavSatFix>
    {
        public Transform Boat;

        //GPS: x: lat(upper larger), y: lon(right larger), z: alt
        //Point: z(upper smaller), x(right smaller), y
        //To simulate the real map, the altitude goes up from 53m at StartP to 60m at P1, and down to 47m at EndP
        public Transform StartP;
        private Vector3 StartingGPS;
        public Transform P1;
        public Transform EndP;
        private Vector3 EndGPS;

        private Vector3 currentWorldPos;
        private Vector3 GPSUnits;

        //For GPS
        private double currentX; //latitude
        private double currentY; //longitude
        private double currentZ; //altitude
    
        private string FrameId = "Unity";
        private double[] zeroArr = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        protected MessageTypes.Sensor.NavSatFix gpsMessage;

        protected override void Start()
        {
            base.Start();
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        void InitialiseMessage()
        {
            StartingGPS = new Vector3(53.38455701638842f, -1.4595508575439455f, 54.0f); //latitude, longitude, elevation(m)
            EndGPS = new Vector3(53.403750049393025f, -1.4112067222595215f, 47.0f);

            GPSUnits = new Vector3((EndGPS.x - StartingGPS.x) / (EndP.position.z - StartP.position.z), (EndGPS.y - StartingGPS.y) / (EndP.position.x - StartP.position.x), (EndGPS.z - StartingGPS.z) / (EndP.position.y - StartP.position.y));

            gpsMessage = new MessageTypes.Sensor.NavSatFix();
            gpsMessage.header.frame_id = FrameId;
            gpsMessage.position_covariance = zeroArr;
            gpsMessage.latitude = StartingGPS.x;
            gpsMessage.longitude = StartingGPS.y;
            gpsMessage.altitude = StartingGPS.z;
        }

        void UpdateMessage()
        {
            currentWorldPos = new Vector3(Boat.position.x, Boat.position.y, Boat.position.z);
     
            currentX = (currentWorldPos.z - StartP.position.z) * GPSUnits.x + StartingGPS.x;
            currentY = (currentWorldPos.x - StartP.position.x) * GPSUnits.y + StartingGPS.y;

            if (currentWorldPos.x > P1.position.x)
            {
                currentZ = (currentWorldPos.x - StartP.position.x) / (P1.position.x - StartP.position.x) * (60 - 53) + 53;
            }
            else if(currentWorldPos.x <= P1.position.x)
            {
                currentZ = (EndP.position.x - currentWorldPos.x) / (EndP.position.x - P1.position.x) * (60 - 47) + 47;
            }
            else
            {
                currentZ = 47;
            }

            gpsMessage.header.Update();
            gpsMessage.latitude = Math.Round(currentX, 8);
            gpsMessage.longitude = Math.Round(currentY, 8);
            gpsMessage.altitude = Math.Round(currentZ, 8);

            //UnityEngine.Debug.Log("gpsMessage: (" + gpsMessage.latitude + "," + gpsMessage.longitude + ", "+ gpsMessage.altitude + ")");
            Publish(gpsMessage);
        }

    }
}