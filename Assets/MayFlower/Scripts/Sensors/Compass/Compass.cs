using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Sensors.Compass
{
    public class Compass : MonoBehaviour
    {
        //public Transform player;
        public Vector3 currentRotation;
        // Start is called before the first frame update
        

        // Update is called once per frame
        void Update()
        {
            //new Rect(player.eulerAngles.y / 360f, 0f, 1f, 1f);
            currentRotation = this.transform.forward;
            Debug.Log("x"+currentRotation.x);
        }
    }
}
