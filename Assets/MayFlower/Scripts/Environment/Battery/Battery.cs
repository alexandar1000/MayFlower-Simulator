using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Environment.Temperature
{
    public class Battery : MonoBehaviour
    {
        public float BatteryChargeMinutesDuration;
        public float BatteryDischargeMinutesDuration;
        public float InitialBatteryChargePercentage;
        protected float Charge;
        protected bool IsCharging;

        // Start is called before the first frame update
        void Start()
        {
            float batteryChargePercentDuration = (BatteryChargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("ChargeBattery", batteryChargePercentDuration, batteryChargePercentDuration);

            float batteryDischargePercentDuration = (BatteryDischargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("DischargeBattery", batteryDischargePercentDuration, batteryDischargePercentDuration);
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        // Update the battery state when the Battery is Discharging
        protected void DischargeBattery()
        {
            // TODO: Adjust discharging and make sure not going bellow 0
            if (!IsCharging)
            {
                Charge -= 1;
            }
        }

        // Update the battery state when the Battery is Charging
        protected void ChargeBattery()
        {
            // TODO: Adjust charging and make sure not going above 100
            if (IsCharging)
            {
                Charge += 1;
            }
        }

        // Get current charge returns the battery percentage normalised to the scale of 0 to 1
        public float GetCurrentCharge()
        {
            return Charge / 100f;
        }
    }
}