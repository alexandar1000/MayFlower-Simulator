﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace RosSharp.RosBridgeClient
 {
     public class GPS_Checkpoint : UnityPublisher<MessageTypes.Std.String>
     {
         public Vector3 GPS_P1;

         // Start is called before the first frame update
         void Start()
         {

         }

         // Update is called once per frame
         void Update()
         {
             Vector3 P1Pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
             GPS_P1 = GameObject.Find("RosConnectors").GetComponent<GPSSensor>().getGPSFromUnityPos(P1Pos);
         }
     }
 }
