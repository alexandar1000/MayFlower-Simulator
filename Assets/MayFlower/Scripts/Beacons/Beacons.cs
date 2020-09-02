using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RosSharp.RosBridgeClient;
using MessageTypes = RosSharp.RosBridgeClient.MessageTypes;
using RosSharp.RosBridgeClient;

namespace MayflowerSimulator.Sensors.Beacons
{
    public class Beacons : UnityPublisher<MessageTypes.Std.String>
    {
        public GameObject boat;
        public GameObject[] beacons;

        private static int beaconCount = 6;

        private float nextActionTime = 0.0f;
        public float period = 0.1f;

        private string json;

        public int currentBeacon = 0;

        [Serializable]
        public class BeaconJson
        {
            public int id;
            public Vector3 location;
            public float distance;
        }

        BeaconJson[] beaconInstance = new BeaconJson[beaconCount];

        public static class JsonHelper
        {
            public static T[] FromJson<T>(string json)
            {
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
                return wrapper.Items;
            }

            public static string ToJson<T>(T[] array)
            {
                Wrapper<T> wrapper = new Wrapper<T>();
                wrapper.Items = array;
                return JsonUtility.ToJson(wrapper);
            }

            public static string ToJson<T>(T[] array, bool prettyPrint)
            {
                Wrapper<T> wrapper = new Wrapper<T>();
                wrapper.Items = array;
                return JsonUtility.ToJson(wrapper, prettyPrint);
            }

            [Serializable]
            private class Wrapper<T>
            {
                public T[] Items;
            }
        }

        void Update()
        {
            if (Time.time > nextActionTime ) 
            {
                for(int i = 0; i < beacons.Length; i++)
                {
                    Vector3 dir = beacons[i].transform.position - boat.transform.position;
                    dir = beacons[i].transform.InverseTransformDirection(dir);
                    float degree = (float)(Mathf.Atan2(dir.z, -dir.x) * Mathf.Rad2Deg);
                    float distance = Vector3.Distance (beacons[i].transform.position, boat.transform.position);
                    if(degree < 0) degree += 360f;

                    beaconInstance[i] = new BeaconJson();
                    beaconInstance[i].id = i;
                    beaconInstance[i].location = beacons[i].transform.position;
                    beaconInstance[i].distance = distance;
                }

                string json = JsonHelper.ToJson(beaconInstance, true);

                nextActionTime += period;
                Publish(PrepareMessage(json));
            }  
        }
        private MessageTypes.Std.String PrepareMessage(string json)
        {
            MessageTypes.Std.String message = new MessageTypes.Std.String();
            message.data = json;

            return message;
        }
    }
}