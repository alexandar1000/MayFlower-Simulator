using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Specialized;

public class GPS : MonoBehaviour
{
    public Vector3 GPS_P1;
    public Transform P1;

    void Update()
    {
        Vector3 P1Pos = new Vector3(P1.position.x, P1.position.y, P1.position.z);
        GPS_P1 = GameObject.Find("RosConnectors").GetComponent<GPSSensor>().getGPSFromUnityPos(P1Pos);
    }
}
