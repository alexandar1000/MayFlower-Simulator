using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;

public class Floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;
    public float thrust = 0.5f;
    public float turnSpeed = 0.1f;
    public float turn = 0;
    public PoseStampedSubscriber pss;
    //PoseStampedSubscriber pss = GameObject.Find("Ros").GetComponent<PoseStampedSubscriber> ();

    private void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity/floaterCount, transform.position, ForceMode.Acceleration);
        float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (transform.position.y < waveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        float  h = Input.GetAxis("Horizontal");
        rigidBody.AddForce(transform.forward * thrust * Time.fixedDeltaTime);
        turn = pss.position.z;
        if (turn == 1)
        {
            rigidBody.AddTorque(0f, turnSpeed * Time.fixedDeltaTime, 0f);
        }
        else if (turn == -1)
        {
            rigidBody.AddTorque(0f, -turnSpeed * Time.fixedDeltaTime, 0f);
        }
        //Debug.Log(GameObject.Find("Ros").transform.position);
        //Debug.Log(GameObject.GetComponent<PoseStampedSubscriber>().GetPosition());
        //Debug.Log(pss.ReturnPosition());
    }
}
