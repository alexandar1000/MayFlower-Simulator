using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class GPSSensor : UnityPublisher<MessageTypes.Geometry.Vector3>
    {
        public Transform Boat;        
        private Vector3 StartingGPS;
        private Vector3 StartingPoint;
        public Transform End;
        private Vector3 EndGPS;
        private Vector3 EndPoint;
        public Vector3 currentWorldPos;
        private Vector3 GPSUnits;
        //For GPS
        private double currentX;
        private double currentY;
        private double currentZ;
        private MessageTypes.Geometry.Vector3 GPSMessage;

        void Start()
        {
            InitialiseMessage();
            InvokeRepeating("UpdateMessage", 1f, 1f);
        }

        void InitialiseMessage()
        {
            StartingPoint = new Vector3(Boat.position.x, Boat.position.y, Boat.position.z);
            StartingGPS = new Vector3(Convert.ToSingle(53.38455701638842), Convert.ToSingle(-1.4595508575439455), Convert.ToSingle(54)); //latitude, longitude, elevation(m)
            EndPoint = new Vector3(End.position.x, End.position.y, End.position.z);
            EndGPS = new Vector3(Convert.ToSingle(53.403750049393025), Convert.ToSingle(-1.4112067222595215), Convert.ToSingle(47));
            GPSUnits = new Vector3((EndGPS.x - StartingGPS.x) / (EndPoint.x - StartingPoint.x), (EndGPS.y - StartingGPS.y) / (EndPoint.y - StartingPoint.y), (EndGPS.z - StartingGPS.z) / (EndPoint.z - StartingPoint.z));

            currentWorldPos = new Vector3(StartingPoint.x, StartingPoint.y, StartingPoint.z);

            GPSMessage = new MessageTypes.Geometry.Vector3();
            GPSMessage.x = Math.Round(53.38455701638842 * 1.0, 8);
            GPSMessage.y = Math.Round(-1.4595508575439455 * 1.0, 8);
            GPSMessage.z = Math.Round(54 * 1.0, 8);
            UnityEngine.Debug.Log(String.Format("GPSMessage: ({0},{1},{2})", GPSMessage.x, GPSMessage.y, GPSMessage.z));
        }


        void UpdateMessage()
        {
            currentWorldPos = new Vector3(Boat.position.x, Boat.position.y, Boat.position.z);
            currentX = (currentWorldPos.x - StartingPoint.x) * GPSUnits.x + StartingGPS.x;
            currentY = (currentWorldPos.y - StartingPoint.y) * GPSUnits.y + StartingGPS.y;
            currentZ = (currentWorldPos.z - StartingPoint.z) * GPSUnits.z + StartingGPS.z;
            
            GPSMessage = new MessageTypes.Geometry.Vector3();
            GPSMessage.x = Math.Round(currentX, 8);
            GPSMessage.y = Math.Round(currentY, 8);
            GPSMessage.z = Math.Round(currentZ, 8);
            UnityEngine.Debug.Log(GPSMessage.x);

            Publish(GPSMessage);
        }
    }
}