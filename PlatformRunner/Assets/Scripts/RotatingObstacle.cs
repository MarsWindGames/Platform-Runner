using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    public float rotateSpeed = 20f;

    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    public bool invertRotation = false;
    // Update is called once per frame

    private void Start()
    {
        if (invertRotation)
        {
            rotateSpeed = -rotateSpeed;
        }
    }
    void Update()
    {
        if (rotateX)
        {
            transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
        }
        if (rotateY)
        {
            transform.Rotate(0, rotateSpeed * Time.deltaTime, 0, Space.Self);
        }

        if (rotateZ)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime, Space.Self);
        }


    }
}
