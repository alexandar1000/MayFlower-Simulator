using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using MayflowerSimulator.Environment.Battery;


namespace Tests
{
    public class BatteryTest
    {

        [UnityTest]
        public IEnumerator InitialCharge()
        {
            var gameObject = new GameObject();
            var battery = gameObject.AddComponent<Battery>();
            yield return new WaitForSeconds(((battery.BatteryChargeMinutesDuration * 60f) / 100f)-1);
            Assert.AreEqual(battery.InitialBatteryChargePercentage, battery.GetCharge());
        }

        [UnityTest]
        public IEnumerator ChargePower1()
        {
            var gameObject = new GameObject();
            var battery = gameObject.AddComponent<Battery>();
            battery.SetChargingStatus(true);
            battery.InitialBatteryChargePercentage = 50;
            yield return new WaitForSeconds((battery.BatteryChargeMinutesDuration * 60f) / 100f);
            Assert.Greater(battery.GetCharge(), 50);

        }

        [UnityTest]
        public IEnumerator ChargePower2()
        {
            var gameObject = new GameObject();
            var battery = gameObject.AddComponent<Battery>();
            battery.SetChargingStatus(true);
            battery.InitialBatteryChargePercentage = 100;
            yield return new WaitForSeconds((battery.BatteryChargeMinutesDuration * 60f) / 100f);
            Assert.AreEqual(battery.GetCharge(), 100);

        }


  
        [UnityTest]
        public IEnumerator Discharge1()
        {
            var gameObject = new GameObject();
            var battery = gameObject.AddComponent<Battery>();
            battery.InitialBatteryChargePercentage = 50;
            yield return new WaitForSeconds((battery.BatteryDischargeMinutesDuration * 60f) / 100f);
            Assert.AreEqual(battery.GetCharge(), 49);

        }
        public IEnumerator Discharge2()
        {
            var gameObject = new GameObject();
            var battery = gameObject.AddComponent<Battery>();
            battery.InitialBatteryChargePercentage = 0;
     
            battery.SetChargingStatus(false);
            yield return new WaitForSeconds((battery.BatteryDischargeMinutesDuration * 60f) / 100f);
            Assert.AreEqual(battery.GetCharge(), 0);

        }
    }
}
