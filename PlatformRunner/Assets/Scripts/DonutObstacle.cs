using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutObstacle : MonoBehaviour
{

    public float obstacleSpeed = 5f;
    public float stickOffset = 5f;
    Vector3[] positions;

    int posIndex = 0;
    bool move = true;

    private void Start()
    {
        positions = new Vector3[2];
        positions[0] = transform.position - transform.right * stickOffset;
        positions[1] = transform.position; 
    }

    IEnumerator moveObstacle()
    {
        float interval = Random.Range(0, 5);
        yield return new WaitForSeconds(interval);
        move = true;
    }

    void Update()
    {
        if (move)
        {
            if (Vector3.Distance(transform.position, positions[posIndex]) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, positions[posIndex], Time.fixedDeltaTime * obstacleSpeed);
            }
            else if (posIndex < positions.Length - 1)
            {
                posIndex++;
            }
            else
            {
                posIndex = 0;
                move = false;
                StartCoroutine(moveObstacle());

            }
        }
    }
}
