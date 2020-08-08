using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    GameObject boat;
    Vector3 eulerAngles;
    /*float startAngleX;
    float startAngleY;
    float startAngleZ;*/
    public float Rotate_X;
    public float Rotate_Y;
    public float Rotate_Z;
    //public float speed = 5f;
    public static float roll; //roll left (<0); roll right (>0).
    public static float pitch; //bow dive (<0); bow upturned (>0).
    public static float yaw; //left (<0); right(>0).

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
        /*eulerAngles = boat.transform.rotation.eulerAngles;
        //Get the starting eulerAngle on each axis: X - roll, Y - yaw, Z - pitch 
        startAngleX = eulerAngles.x;
        startAngleY = eulerAngles.y;
        startAngleZ = eulerAngles.z;
        Debug.Log("Starting eulerAngle roll: " + eulerAngles.x + "; yaw: " + eulerAngles.y + "; pitch: " + eulerAngles.z);*/
    }

    // Update is called once per frame
    void Update()
    {
        //boat.transform.Rotate(0, speed, 0);
        eulerAngles = boat.transform.rotation.eulerAngles;
        Rotate_X = Convert.ToSingle(Math.Round(eulerAngles.x, 3));
        Rotate_Y = Convert.ToSingle(Math.Round(eulerAngles.y, 3));
        Rotate_Z = Convert.ToSingle(Math.Round(eulerAngles.z, 3));
        //roll
        if(Rotate_X < 90)
        {
            roll = Rotate_X * (-1);
        }
        else if(Rotate_X > 270)
        {
            roll = 360 - Rotate_X;
        }

        //pitch
        if (Rotate_Z > 180)
        {
            pitch = -(360 - Rotate_Z);
        }else
        {
            pitch = Rotate_Z;
        }

        if (pitch >= 90 && pitch <= 270)
        {
            var boatRenderer = boat.GetComponent<Renderer>();
            boatRenderer.material.SetColor("_BaseColor", Color.red);
            UnityEngine.Debug.Log("boat capsized!");
        }
        else
        {
            var boatRenderer = boat.GetComponent<Renderer>();
            boatRenderer.material.SetColor("_BaseColor", Color.white);

        }

        //yaw
        if (Rotate_Y > 180)
        {
            yaw = -(360 - Rotate_Y);
        }
        else
        {
            yaw = Rotate_Z;
        }

    }
}
