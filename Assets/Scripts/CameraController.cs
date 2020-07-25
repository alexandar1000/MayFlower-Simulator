using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject boat;

    public Transform target;
    private Vector3 offset;
    
    
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - boat.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        transform.position = boat.transform.position + offset;
    }
}
