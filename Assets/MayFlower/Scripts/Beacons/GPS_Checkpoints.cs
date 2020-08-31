﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RosSharp.RosBridgeClient
{
    public class GPS_Checkpoints : UnityPublisher<MessageTypes.Std.Float64>

    {
        public GameObject[] Beacons;
        private Animator anim;
        public float rotSpeed = 0.8f;
        public float Speed = 4f;
        public float degree = 0;
        private float accuracyBeacon = 2.0f;
        private int currentBeacon = 0;
        public Transform _destination;
        List<Transform> path = new List<Transform>();
        private GameObject next_beac;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;
       
        protected MessageTypes.Sensor.NavSatFix nextBeaconMessage;

        [System.Serializable]
        public class next_Beacon
        {
            public int nextBeacon;
            public string beacontype;
            public Vector3 location;
        }
        
        [SerializeField]
        next_Beacon nxtBeac = new next_Beacon();
        
        
        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            foreach (GameObject go in Beacons)
            {
                path.Add(go.transform);
            }
            nextBeaconMessage = new MessageTypes.Sensor.NavSatFix();
            currentBeacon = FindNextBeacon();
            anim.SetBool("isWalking", true);
        }
        
        // Update is called once per frame
        int FindNextBeacon()
        {
            if (path.Count == 0) return -1;
            int closest = 0;
            float lastDist = Vector3.Distance(this.transform.position, path[0].position);
            for (int i = 1; i < path.Count; i++)
            {
                float thisDist = Vector3.Distance(this.transform.position, path[i].position);
                if (lastDist > thisDist && i != currentBeacon)
                {
                    closest = i;

                    float BeaconInfoReading;

                    if (i == 0)
                    {
                        Debug.LogError("The component is not attached to " + gameObject.name);
                    }
                    else
                    {
                        if (currentBeacon == closest)
                        {
                            setDestination();
                            Debug.Log("The boat has arrived to the destination" + gameObject.name);
                        }
                        else
                        {
                            string json = JsonUtility.ToJson(nxtBeac);

                            nxtBeac.nextBeacon = i + 1;
                            nxtBeac.beacontype  = next_beac.GetComponent<GPS_Checkpoint>().gameObject.name;
                            nxtBeac.location = next_beac.GetComponent<GPS_Checkpoint>().GPS_P1;
                            
                            
                            
                            Debug.Log("The next beacon ID:" + nxtBeac.nextBeacon  + "," + "  Type:" +  nxtBeac.beacontype + ","  + "  Location:" +
                                      nxtBeac.location);
                            //Publish(PrepareMessage(json));
                            
                        }
                    }
                }
            }
            return closest;
        }
        
        void Update()
        {
            Vector3 direction = path[currentBeacon].position - transform.position;
            //this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
            //    rotSpeed * Time.deltaTime);
            //this.transform.Translate(0, 0, Time.deltaTime * Speed);
            if (direction.magnitude < accuracyBeacon)
            {
                path.Remove(path[currentBeacon]);
                currentBeacon = FindNextBeacon();
            }

            //Get the beacons angle
            Vector3 dir = path[currentBeacon].transform.position - this.transform.position;
            dir = path[currentBeacon].transform.InverseTransformDirection(dir);
            degree = (float)(Mathf.Atan2(dir.z, -dir.x) * Mathf.Rad2Deg);

            if (Time.time > nextActionTime ) 
            {
                nextActionTime += period;
                Publish(PrepareMessage(degree));
            }
        }
        
        private void setDestination()
        {
            if (_destination != null)
            {
                Vector3 targetVector = _destination.transform.position;
            }
        }
        
        /*
        private MessageTypes.Std.String PrepareMessage(string NextBeaconInfo)
        {
            MessageTypes.Std.String message = new MessageTypes.Std.String();
            message.data = NextBeaconInfo;

            return message;
        }      
        */
        
        private MessageTypes.Std.Float64 PrepareMessage(float angle)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = angle;

            return message;
        }
        
    }
}     