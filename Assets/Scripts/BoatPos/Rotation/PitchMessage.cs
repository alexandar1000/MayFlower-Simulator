using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class PitchMessage : UnityPublisher<MessageTypes.Std.Float64>
    {
        public float MeasurementFrequency = 2f;
        private static float pitch;
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            InvokeRepeating("MeasurePitch", MeasurementFrequency, MeasurementFrequency);  //1s delay, repeat every 1s
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Get the current roll tilting angle
        private void MeasurePitch()
        {
            pitch = Rotate.pitch;
            UnityEngine.Debug.Log(pitch);
            Publish(PrepareMessage(pitch));
        }

        private MessageTypes.Std.Float64 PrepareMessage(float pitchAngle)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = pitchAngle;

            return message;
        }
    }
}