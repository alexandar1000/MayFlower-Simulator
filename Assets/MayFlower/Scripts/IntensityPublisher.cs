using System;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class IntensityPublisher : UnityPublisher<MessageTypes.Sensor.Illuminance>
    {
        private MessageTypes.Sensor.Illuminance message;
        public string FrameId = "Unity";

        protected override void Start()
        {
            base.Start();
            message = new MessageTypes.Sensor.Illuminance
            {
                header = new MessageTypes.Std.Header
                {
                    frame_id = FrameId
                },
                illuminance = 400,
                variance = 0,
            };
            
        }

        void Update()
        {

            message.header.Update();
            message.illuminance = 400;
            message.variance = 0;

            Publish(message);
        }

    }
}