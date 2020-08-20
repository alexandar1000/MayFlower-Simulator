using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Sensors.Compass
{
    public class Compass : MonoBehaviour
    {
        //public Transform player;
        public Vector3 currentRotation;
        public Quaternion rotation; 

        // Update is called once per frame
        void Update()
        {
            //new Rect(player.eulerAngles.y / 360f, 0f, 1f, 1f);
            currentRotation = this.transform.forward;
            rotation = Quaternion.Euler(currentRotation);
        }
    }
}
