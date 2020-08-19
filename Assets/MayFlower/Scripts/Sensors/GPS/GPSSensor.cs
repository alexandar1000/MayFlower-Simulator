/* 
Implementation of the GPS sensor which sends current GPS coordinates of the main vessel
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.Diagnostics;

namespace RosSharp.RosBridgeClient
{
    public class GPSSensor : UnityPublisher<MessageTypes.Sensor.NavSatFix>
    {
        public Transform Boat;

        //GPS: lat(upper larger), lon(right larger), alt
        //Point: z(upper smaller), x(right smaller), y
        public Transform StartP;
        private Vector3 StartingGPS; 
        private Vector3 StartingPoint;
        public Transform EndP;
        private Vector3 EndGPS;
        private Vector3 EndPoint;
        public Vector3 currentWorldPos;
        public Vector3 GPSUnits;
        //For GPS
        private double currentX; //latitude
        private double currentY; //longitude
        private double currentZ; //altitude
        protected MessageTypes.Sensor.NavSatFix gpsMessage;
        public string FrameId = "GPS_Sensor";
        private double[] zeroArr = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        void InitialiseMessage()
        {
            StartingPoint = new Vector3(StartP.position.x, StartP.position.y, StartP.position.z);
            StartingGPS = new Vector3(53.38455701638842f, -1.4595508575439455f, 54.0f); //latitude, longitude, elevation(m)
            EndPoint = new Vector3(EndP.position.x, EndP.position.y, EndP.position.z);
            EndGPS = new Vector3(53.403750049393025f, -1.4112067222595215f, 47.0f);

            GPSUnits = new Vector3((EndGPS.x - StartingGPS.x) / (EndPoint.z - StartingPoint.z), (EndGPS.y - StartingGPS.y) / (EndPoint.x - StartingPoint.x), (EndGPS.z - StartingGPS.z) / (EndPoint.y - StartingPoint.y));

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
     
            currentX = (currentWorldPos.z - StartingPoint.z) * GPSUnits.x + StartingGPS.x;
            currentY = (currentWorldPos.x - StartingPoint.x) * GPSUnits.y + StartingGPS.y;
            currentZ = (currentWorldPos.y - StartingPoint.y) * GPSUnits.z + StartingGPS.z;
            UnityEngine.Debug.Log("StartingGPS.y = "+ StartingGPS.y);
            gpsMessage.header.Update();
            gpsMessage.latitude = Math.Round(currentX, 8);
            gpsMessage.longitude = Math.Round(currentY, 8);
            gpsMessage.altitude = Math.Round(currentZ, 8);

            UnityEngine.Debug.Log("gpsMessage: (" + gpsMessage.latitude + "," + gpsMessage.longitude + ", "+ gpsMessage.altitude + ")");
            Publish(gpsMessage);
        }

    }
}