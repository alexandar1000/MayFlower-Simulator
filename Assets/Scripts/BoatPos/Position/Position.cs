using System.Security.Cryptography;
using UnityEngine;

public class Position : MonoBehaviour
{
    GameObject boat;
    private int frame = 0;
    public static float pX;    
    public static float pY; //height
    public static float pZ;

    // Start is called before the first frame update
    void Start()
    {
        boat = GameObject.Find("Boat");
    }

    // Update is called once per frame
    void Update()
    {
        pX = transform.position.x;
        pY = transform.position.y;
        pZ = transform.position.z;
       if (frame % 100 == 0)
        {
            Debug.Log("Current position is: " + transform.position.x + ", "+ transform.position.y + ", " + transform.position.z);
        }
        frame++;
    }
}
