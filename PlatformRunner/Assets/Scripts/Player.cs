using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Player : MonoBehaviour, IPlayer
{
    [Header("Player Abilities")]
    public float moveSpeed = 10f;
    public float jumpForce = 8f;

    [Header("Instances")]
    public FixedJoystick fixedJoystick;
    public Transform finishLine;
    public TMP_Text rankText;

    public int myRank = 0;


    //Unity
    public bool moveLeft = false;
    bool onGround = true;
    bool jumped = false;
    bool gameStarted = false;
    public bool move = false;
    bool hitByStick = false;
    bool controllable = true;

    Animator animator;
    Rigidbody rb;

    Vector3 targetPos = new Vector3();
    Vector3 currentPos = new Vector3();

    List<GameObject> transforms;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        //we get all enemy transforms to calculate our rank later.
        GetEnemyTransforms();

        StartCoroutine(CheckRank());
    }

    void Update()
    {
        if (GameManager.instance.gameStarted)
        {
            SetAnimation();
            Movement();
        }
    }

    private void Movement()
    {
        if (!hitByStick && controllable)
        {
            rb.velocity = new Vector3(fixedJoystick.Horizontal * moveSpeed, rb.velocity.y, fixedJoystick.Vertical * moveSpeed);

            if (fixedJoystick.Horizontal != 0f || fixedJoystick.Vertical != 0f)
            {
                if (rb.velocity != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(rb.velocity);
            }
        }

        if (transform.position.y <= -4f)
        {
            GameManager.instance.RespawnMe(transform);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }


    }

    public void jumpButton()
    {
        Jump();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.transform.CompareTag("RotatingObstacle"))
        {
            rb.AddForce(Vector3.left * Time.fixedDeltaTime * 360, ForceMode.Impulse); // rotate 
        }
        if (other.transform.CompareTag("RotatingObstacle2"))
        {
            rb.AddForce(-Vector3.left * Time.fixedDeltaTime * 360, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        onGround = true;
        jumped = false;

        if (other.transform.CompareTag("DamageObstacle"))
        {
            controllable = false;
            hitByStick = true;
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
            controllable = false;
            hitByStick = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        onGround = false;
    }

    private void Jump()
    {
        if (!jumped)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumped = true;
        }
    }

    private void StartGame()
    {
        gameStarted = true;
    }

    public void SetAnimation()
    {
        if (!hitByStick)
        {
            if (jumped)
            {
                animator.SetInteger("state", 2);
            }
            else if (rb.velocity.z >= 0.1f)
            {
                animator.SetInteger("state", 1);
            }
            else if (rb.velocity.z == 0)
            {
                animator.SetInteger("state", 0);
            }
        }
    }

    private void GetEnemyTransforms()
    {
        transforms = new List<GameObject>();
        foreach (Enemy gameObject2 in FindObjectsOfType<Enemy>())
        {
            transforms.Add(gameObject2.gameObject);

        }
        transforms.Add(gameObject);
    }

    private IEnumerator CheckRank()
    {
        yield return new WaitForSeconds(0.5f);
        transforms = transforms.OrderBy(
              x => Vector3.Distance(finishLine.transform.position, x.transform.position)
             ).ToList();

        for (int i = 0; i < transforms.Count; i++)
        {
            if (transforms[i] == gameObject)
            {
                myRank = i + 1;
                rankText.text = myRank.ToString();
            }
        }
        yield return CheckRank();
    }

    IEnumerator stickHit()
    {
        animator.SetInteger("state", 3);

        yield return new WaitForSeconds(3);

        hitByStick = false;
        controllable = true;
    }

    IEnumerator playerDie()
    {
        animator.SetInteger("state", 4);
        yield return new WaitForSeconds(3);
        GameManager.instance.RespawnMe(transform);
        hitByStick = false;
        controllable = true;
    }


}
