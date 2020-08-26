﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarPulse : MonoBehaviour
{
    private Transform pulseTransform;
    private float range;
    private float rangeMax;

    private void Awake()
    {
        pulseTransform = transform.Find("Pulse");
        rangeMax = 15f;
    }

    // Update is called once per frame
    private void Update()
    {
        float rangeSpeed = 0.30f;
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax)
        {
            range = 0f;
        }

        pulseTransform.localScale = new Vector3(range, range);
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position, range / 2f, Vector2.zero);
        Debug.Log("Beacon Sending Signal");

        // if (raycastHit2D.collider != null)
        // {
        //     Debug.Log("Beacon Sending Signal");
        //     //Publish(PrepareMessage(Signal));
        // }
        
    }
}