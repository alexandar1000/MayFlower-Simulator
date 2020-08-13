using System;
using UnityEngine;

namespace Simulation
{
    [System.Serializable]
    public class ObjectPosition : System.Object
    {
        public ObjectPosition(Vector3 pos, String objectName)
        {
            this.pos = pos;
            this.objectName = objectName;
        }

        public Vector3 pos;
        public String objectName;
    }
}