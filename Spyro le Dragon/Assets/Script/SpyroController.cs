using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyroController : MonoBehaviour
{

    public Rigidbody spyroRb;

    public float speed;
    public float jumpForce;

    private float horizontalAxis;
    private float verticalAxis;

    Vector3 movement;

    private void Start()
    {
        spyroRb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        verticalAxis = Input.GetAxisRaw("Vertical");

        Movement();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    public void Movement()
    {
        spyroRb.velocity = new Vector3(horizontalAxis * speed, spyroRb.velocity.y, verticalAxis * speed);
    }

    public void Jump()
    {
        spyroRb.velocity = new Vector3(spyroRb.velocity.x, jumpForce, spyroRb.velocity.z);
    }




}
