using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSensor : MonoBehaviour
{
    public Light lit;
    // Start is called before the first frame update
    void Start()
    {
        lit = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("The intensity is"+lit.intensity);
    }
}
