using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalObstacle : MonoBehaviour
{


    public float horizontalSpeed = 2f;
    public float offset = 5f;

    Vector3[] positions;
    int posIndex = 0;
    void Start()
    {
        positions = new Vector3[2];
        
        positions[0] = transform.position;
        positions[0].x = transform.position.x + offset;

        positions[1] = transform.position;
        positions[1].x = transform.position.x - offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, positions[posIndex]) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, positions[posIndex], 2 * Time.deltaTime);
        }
        else if (posIndex < positions.Length - 1)
        {
            posIndex++;
        }
        else
        {
            posIndex = 0;
        }
    }
}
