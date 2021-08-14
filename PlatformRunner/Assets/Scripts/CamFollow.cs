using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;

    public float offset;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = new Vector3();
        newPos.x = target.position.x;
        newPos.y = transform.position.y;
        newPos.z = target.position.z + offset;

        transform.position = newPos;
    }
}
