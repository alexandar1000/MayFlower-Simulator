using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public static class Util 
    {
        public static MessageTypes.Std.Float64 PrepareMessage(float mes)
        {
            MessageTypes.Std.Float64 message = new MessageTypes.Std.Float64();
            message.data = mes;

            return message;
        }
    }
}
