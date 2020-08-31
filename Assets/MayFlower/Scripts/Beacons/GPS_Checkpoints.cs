using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class GPS_Checkpoints : UnityPublisher<MessageTypes.Std.String>

    {
        public GameObject boat;
        public GameObject[] Beacons;
        public float Speed = 10f;
        private int currentBeacon = 0;
        protected int currentBeaconCheckpoint;
        private float nextActionTime = 0.0f;
        public float period = 0.1f;
        
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

        private RosSocket rosSocket;
        public string topic= "/beacon_gpsCheckpoint";
        string publicationId;
        
        
        
        // Start is called before the first frame update
        void Start()
        {
            rosSocket = GetComponent<RosConnector>().RosSocket;
            publicationId = rosSocket.Advertise<MessageTypes.Std.String>(topic);

            foreach (GameObject go in Beacons)
            {
                path.Add(go.transform);
            }
            currentBeaconCheckpoint = 0;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (Time.time > nextActionTime)
            {
                if (currentBeaconCheckpoint == Beacons.Length)
                {
                    Debug.Log("The boat has arrived to the destination" + gameObject.name);
                }
                else
                {
                    
                        // Vector3 direction = path[currentBeacon].position - transform.position;
                        // this.transform.Translate(0, 0, Time.deltaTime * Speed);
                        Vector3 dir = Beacons[currentBeacon].transform.position - boat.transform.position;
                        dir = Beacons[currentBeacon].transform.InverseTransformDirection(dir);
                        this.transform.Translate(0, 0, Time.deltaTime * Speed);
                        path.Remove(path[currentBeacon]);
                        currentBeacon = FindNextBeacon();

                }
                nextActionTime += period;
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
                currentBeaconCheckpoint += 1;
                nxtBeac.nextBeacon = 1 + currentBeaconCheckpoint;
                nxtBeac.beacontype  = Beacons[currentBeaconCheckpoint].GetComponent<GPS_Checkpoint>().gameObject.name;
                nxtBeac.location = Beacons[currentBeaconCheckpoint].GetComponent<GPS_Checkpoint>().GPS_P1;
                
                string json = JsonUtility.ToJson(nxtBeac);
                
                Debug.Log("The next beacon ID:" + nxtBeac.nextBeacon  + "," + "  Type:" +  nxtBeac.beacontype + ","  + "  Location:" +
                          nxtBeac.location);
                
                rosSocket.Publish(publicationId,PrepareMessage(json));
            }
            return currentBeacon;
        }

        private MessageTypes.Std.String PrepareMessage(string NextBeaconInfo)
        {
            MessageTypes.Std.String message = new MessageTypes.Std.String();
            message.data = NextBeaconInfo;
        
            return message;
        }
        
    }
} 