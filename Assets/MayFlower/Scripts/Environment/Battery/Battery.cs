using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Environment.Temperature
{
    public class Battery : MonoBehaviour
    {
        public float BatteryChargeDuration;
        public float BatteryDischargeDuration;
        public float InitialBatteryChargePercentage;
        protected float charge;
        protected bool isCharging;

        // Start is called before the first frame update
        void Start()
        {
            float batteryChargePercentDuration = BatteryChargeDuration / 100f;
            InvokeRepeating("ChargeBattery", batteryChargePercentDuration, batteryChargePercentDuration);

            float batteryDischargePercentDuration = BatteryDischargeDuration / 100f;
            InvokeRepeating("DischargeBattery", batteryDischargePercentDuration, batteryDischargePercentDuration);
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Update the battery state when the Battery is Discharging
        protected void DischargeBattery()
        {
            if (!isCharging)
            {
                charge -= 1;
            }
        }

        // Update the battery state when the Battery is Charging
        protected void ChargeBattery()
        {
            if (isCharging)
            {
                charge += 1;
            }
        }

        public float GetCurrentCharge()
        {
            return charge;
        }
    }
}