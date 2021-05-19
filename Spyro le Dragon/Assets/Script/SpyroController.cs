using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyroController : MonoBehaviour
{
    public CharacterController controller;
    public Animator anim;
    public Transform cam;

    public float speed = 6f;
    public float baseSpeed = 6f;
    public float chargeSpeed = 12f;

    public float gravity = -9.81f;
    public float glideGravity = -9.81f / 2f;
    public float baseGravity = -9.81f;

    public float jump = 3f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;
    public bool isGrounded;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
        Gravity();
        Jump();
        Glide();
        Charge();
        if (!isGrounded)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", true);
        }
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            anim.SetBool("isWalking", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else anim.SetBool("isWalking", false);
    }

    public void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -3f * gravity);
        }     
    }

    public void Glide()
    {
        if (Input.GetButton("Jump") && !isGrounded)
        {
                gravity = glideGravity;
        }
        else gravity = baseGravity;
    }

    public LayerMask destroyable;
    public RaycastHit hit;
    public float range;
    public void Charge()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            speed = chargeSpeed;

            if (Physics.Raycast(transform.position + Vector3.up * 0.2f,  transform.forward, out hit, range, destroyable))
            {
                Debug.DrawRay(transform.position + Vector3.up * 0.2f, transform.forward * range, Color.green, 0f);
                Destroy(hit.collider.gameObject);
            }
            else Debug.DrawRay(transform.position + Vector3.up * 0.2f, transform.forward * range, Color.red, 0f);

            anim.SetBool("isCharging", true);

        }
        else
        {
            speed = baseSpeed;
            anim.SetBool("isCharging", false);
        }
    }
}
