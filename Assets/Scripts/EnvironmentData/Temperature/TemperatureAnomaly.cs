using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureAnomaly : MonoBehaviour
{
    public float Temperature;
    public bool IsConstant;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!IsConstant) 
        {
            // Update so that the temperature adjusts to the surroundings
        }
    }
}
