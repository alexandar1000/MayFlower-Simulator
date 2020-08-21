using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Sensors.Compass
{
    public class Compass : MonoBehaviour
    {
        public Vector3 currentRotation;
        public Quaternion rotation; 

        void Update()
        {
            currentRotation = this.transform.forward;
            rotation = Quaternion.Euler(currentRotation);
        }
    }
}
