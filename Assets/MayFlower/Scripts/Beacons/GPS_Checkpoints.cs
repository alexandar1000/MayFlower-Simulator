﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    public class GPS_Checkpoints : UnityPublisher<MessageTypes.Std.String>

    {
        public GameObject[] Beacons;
        private Animator anim;
        public float rotSpeed = 0.8f;
        public float Speed = 10f;
        private float accuracyBeacon = 2.0f;
        private int currentBeacon = 0;
        public Transform _destination;
        protected int currentBeaconCheckpoint;

        List<Transform> path = new List<Transform>();
        
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
            anim.SetBool("isWalking", true);
            currentBeacon = FindNextBeacon();
            currentBeaconCheckpoint = 0;
        }
        
        // Update is called once per frame
        void Update()
        {
            Vector3 direction = path[currentBeacon].position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * Speed);
            if (direction.magnitude < accuracyBeacon)
            {
                path.Remove(path[currentBeacon]);
                currentBeacon = FindNextBeacon();
            }
        
        }
        int FindNextBeacon()
        {

            if (Beacons.Length == 0)
            {
                Debug.LogError("The component is not attached to " + gameObject.name);
            }
            else
            {
                if (currentBeaconCheckpoint == Beacons.Length)
                {
                    setDestination();
                    Debug.Log("The boat has arrived to the destination" + gameObject.name);
                }
                else
                {
                    currentBeaconCheckpoint += 1;
                    nxtBeac.nextBeacon = 1 + currentBeaconCheckpoint;
                    nxtBeac.beacontype  = Beacons[currentBeaconCheckpoint].GetComponent<GPS_Checkpoint>().gameObject.name;
                    nxtBeac.location = Beacons[currentBeaconCheckpoint].GetComponent<GPS_Checkpoint>().GPS_P1;
                    
                    string json = JsonUtility.ToJson(nxtBeac);
                    
                    Debug.Log("The next beacon ID:" + nxtBeac.nextBeacon  + "," + "  Type:" +  nxtBeac.beacontype + ","  + "  Location:" +
                              nxtBeac.location);
                    Publish(PrepareMessage(json));
                    
                }
            }
            return currentBeacon;
        }

        private void setDestination()
        {
            if (_destination != null)
            {
                Vector3 targetVector = _destination.transform.position;
            }
        }
        
        private MessageTypes.Std.String PrepareMessage(string NextBeaconInfo)
        {
            MessageTypes.Std.String message = new MessageTypes.Std.String();
            message.data = NextBeaconInfo;

            return message;
        }
        
    }
}     