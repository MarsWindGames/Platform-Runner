using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy Abilities")]
    public float moveSpeed = 10f;
    public float jumpForce = 8f;

    [Header("Instances")]
    public Transform finishLine;

    //Unity
    public bool moveLeft = false;
    bool onGround = true;
    bool jumped = false;
    bool gameStarted = false;
    public bool move = false;
    bool hitByStick = false;
    bool controllable = true;
    bool dead = false;
    public bool moveable = true;

    Animator animator;

    Rigidbody rb;

    Vector3 targetPos = new Vector3();
    Vector3 currentPos = new Vector3();

    List<Enemy> enemies;
    List<GameObject> transforms;

    int directionToAvoid = -1;

    float minX, maxX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        minX = -7; // to prevent falling while avoiding, she can still fall.
        maxX = 7;
        moveSpeed = UnityEngine.Random.Range(7, 11);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameStarted)
        {
            Movement();
            CheckObstacles();
        }
    }

    private void CheckObstacles()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("StaticObstacle"))
            {
                moveable = true;
                Jump();
            }
            else if (hitColliders[i].CompareTag("DamageObstacle"))
            {
                if (Mathf.Abs(hitColliders[i].transform.position.x - transform.position.x) > 3f)
                {
                    moveable = true; //if enemy is far enough to obstacle they can move.
                }
                else if (hitColliders[i].transform.position.z > transform.position.z && !dead)
                {
                    TryToAvoid(); //if enemy is not passed the obstacle yet try to avoid.

                }
                else
                {
                    moveable = true;
                }
            }
            else if (hitColliders[i].CompareTag("StickObstacle"))
            {
                TryToAvoidStickObstacle(hitColliders[i].transform);
                moveable = true;
            }
        }
    }

    private void TryToAvoidStickObstacle(Transform trans)
    {
        if (transform.position.z < trans.position.z)
        {
            Vector3 dir = (transform.position - trans.position).normalized;

            rb.AddForce(dir * 12); //hard coded force
        }
    }
    private void TryToAvoid()
    {
        //enemy will select a direction and go there to avoid obstacle
        if (directionToAvoid == -1)
            directionToAvoid = UnityEngine.Random.Range(0, 2);

        if (directionToAvoid == 0)
        {
            Vector3 direction = (transform.position + Vector3.right * 2 - transform.position).normalized;
            if (transform.position.x > minX && transform.position.x < maxX) // try not to fall
                rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 direction = (transform.position + Vector3.left * 2 - transform.position).normalized;
            if (transform.position.x > minX && transform.position.x < maxX) // try not to fall
                rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Movement()
    {
        if (moveable && !dead)
        {
            if (transform.position.z < finishLine.position.z + 5)
            {
                Vector3 direction = (finishLine.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

                if (!jumped && !hitByStick)
                    animator.SetInteger("state", 1);
            }
        }

        if (transform.position.y <= -5f)
        {
            GameManager.instance.RespawnMe(transform);
            directionToAvoid = -1;
            moveable = true;
        }
    }

    public void finishEvents()
    {
        //ust uste binmemeleri icin
        float randomForce = UnityEngine.Random.Range(-2f, 5f);
        Vector3 newPos = transform.position + (Vector3.forward * randomForce + Vector3.left * randomForce);
        newPos.y = 0;
        transform.position = newPos;
        transform.Rotate(0, 180, 0);

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        moveable = false;
        animator.SetInteger("state", 5);
    }

    private void OnCollisionEnter(Collision other)
    {
        jumped = false;

        if (other.transform.CompareTag("DamageObstacle"))
        {
            StartCoroutine(playerDie());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "StickObstacle" && !hitByStick)
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;

            rb.AddForce(dir * 750);
            StartCoroutine(stickHit());
        }
    }

    IEnumerator stickHit()
    {
        hitByStick = true;
        animator.SetInteger("state", 3);
        moveable = false;

        yield return new WaitForSeconds(3);

        hitByStick = false;
        moveable = true;
    }

    IEnumerator playerDie()
    {
        moveable = false;
        hitByStick = true;
        dead = true;
        animator.SetInteger("state", 4);

        yield return new WaitForSeconds(3);
        GameManager.instance.RespawnMe(transform);
        moveable = true;
        hitByStick = false;
        directionToAvoid = -1;
        dead = false;
    }

    private void Jump()
    {
        if (!jumped)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumped = true;
            animator.SetInteger("state", 2);
        }
    }
}
