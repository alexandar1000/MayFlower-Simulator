using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
//using RosSharp.RosBridgeClient;

namespace Tests
{

    public class GPSTest
    {
        [UnityTest]
        public IEnumerator GPS()
        {
            GameObject gameGameObject =
            MonoBehaviour.Instantiate(Resources.Load<GameObject>("Mayflower/Scripts/Sensors/Test/GPS"));
            TestGPS game = gameGameObject.GetComponent<TestGPS>();
            yield return new WaitForSeconds(2f);
            Assert.AreEqual(game.P1Pos, game.transform.position);

          
        }

    }

}
