using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiDAR : MonoBehaviour
{
    private Laser _laser; 
    public float LaserLength = 25f;

    // Start is called before the first frame update
    void Start()
    {
        _laser = new Laser(LaserLength);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startingPosition = transform.position;
        Vector3 direction = transform.forward;
        _laser.ShootLaser(transform.position, transform.forward);
    }
}
