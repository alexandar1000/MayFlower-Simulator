﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS_Checkpoint : MonoBehaviour
{
    public Vector3 GPS_P1;
    public Transform P1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 P1Pos = new Vector3(P1.position.x, P1.position.y, P1.position.z);
        GPS_P1 = GameObject.Find("RosConnectors").GetComponent<GPSSensor>().getGPSFromUnityPos(P1Pos);
    }
}
