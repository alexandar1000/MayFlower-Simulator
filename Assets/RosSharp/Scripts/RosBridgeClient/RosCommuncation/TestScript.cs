using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class TestScript : UnitySubscriber<MessageTypes.Std.String>
    {
        public int counter = 0;

        protected override void Start()
        {
            base.Start();
        }
        
        protected override void ReceiveMessage(MessageTypes.Std.String msg)
        {
            counter++;

            Debug.Log("Message " + counter++ + ": " + msg.data);
        }
    }
}
