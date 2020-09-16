using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Collections.Specialized;

public class TestGPS : MonoBehaviour
{
    public Transform P1;
    public Vector3 P1Pos;

    void Update()
    {
        Vector3 P1Pos = new Vector3(P1.position.x, P1.position.y, P1.position.z);
    }
}
