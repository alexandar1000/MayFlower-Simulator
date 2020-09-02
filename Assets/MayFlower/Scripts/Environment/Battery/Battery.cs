using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MayflowerSimulator.Environment.Battery
{
    public class Battery : MonoBehaviour
    {
        public float BatteryChargeMinutesDuration;
        public float BatteryDischargeMinutesDuration;
        public float InitialBatteryChargePercentage;
        public float Voltage { get; } = 3f;
        protected float Charge;
        protected bool isCharging;
        protected GameObject homeZone;

        protected const byte POWER_SUPPLY_STATUS_UNKNOWN = 0;
        protected const byte POWER_SUPPLY_STATUS_CHARGING = 1;
        protected const byte POWER_SUPPLY_STATUS_DISCHARGING = 2;
        protected const byte POWER_SUPPLY_STATUS_NOT_CHARGING = 3;
        protected const byte POWER_SUPPLY_STATUS_FULL = 4;

        void Start()
        {
            Charge = InitialBatteryChargePercentage;

            float batteryChargePercentDuration = (BatteryChargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("ChargeBattery", batteryChargePercentDuration, batteryChargePercentDuration);

            float batteryDischargePercentDuration = (BatteryDischargeMinutesDuration * 60f) / 100f;
            InvokeRepeating("DischargeBattery", batteryDischargePercentDuration, batteryDischargePercentDuration);

        }

        // Update the battery state when the Battery is Discharging
        protected void DischargeBattery()
        {
            if (!isCharging && Charge > 0f)
            {
                Charge = Mathf.Max(0, Charge - 1);
            }
        }

        // Update the battery state when the Battery is Charging
        protected void ChargeBattery()
        {
            if (isCharging && Charge < 100)
            {
                Charge = Mathf.Min(100, Charge + 1);
            }
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.CompareTag("HomeArea"))
            {
                homeZone = coll.gameObject;
                isCharging = true;
            }
        }

        private void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.CompareTag("HomeArea"))
            {
                isCharging = false;
            }

        }

        // Get current charge returns the battery percentage normalised to the scale of 0 to 1
        public float GetCurrentCharge()
        {
            return Charge / 100f;
        }

        public byte GetCurrentChargingStatus()
        {
            if (isCharging)
            {
                if (Charge == 100f)
                {
                    return POWER_SUPPLY_STATUS_FULL;
                }

                return POWER_SUPPLY_STATUS_CHARGING;
            }
            else
            {
                if (Charge == 0f)
                {
                    return POWER_SUPPLY_STATUS_NOT_CHARGING;
                }

                return POWER_SUPPLY_STATUS_DISCHARGING;
            }
        }
    }
}
