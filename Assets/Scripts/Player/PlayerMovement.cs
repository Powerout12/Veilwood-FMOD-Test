using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private float savedMoveSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    private bool isDoubleJumping;

    [Header("Powers")]
    public bool hasDoubleJumpPower;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;

    public Transform orientation;
  

    public static bool isStalled;
    public static bool accesingInventory;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    private void Start()
    {
        savedMoveSpeed = moveSpeed;
        accesingInventory = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

       /* ResetJump();
        if (hasDoubleJumpPower)
        {
            ResetDoubleJump();
        }*/
    }

    private void Update()
    {
        MyInput();
        if (isStalled)
            return;

        MyMovementInput();
        SpeedControl();
        CheckGrounded();

    }



    private void FixedUpdate()
    {
        if (isStalled)
            return;
        MovePlayer();
    }

    private void MyInput()
    {

        if (accesingInventory)
        {
            isStalled = true;
        }
        else if (!accesingInventory)
        {
            isStalled = false;
        }
    }

    private void MyMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        //uncomment for double jump or regular jump
       /* if (Input.GetKeyDown(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        else if (Input.GetKeyDown(jumpKey) && readyToJump && !grounded && hasDoubleJumpPower && !isDoubleJumping)
        {
            readyToJump = false;
            isDoubleJumping = true;
            Jump();
        }*/

        if (Input.GetKeyDown(KeyCode.LeftShift) && !accesingInventory)
        {
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !accesingInventory)
        {
            moveSpeed = savedMoveSpeed;
        }

    }

   /* private IEnumerator CheckIfGroundedAfterFirstJump()
    {
        yield return new WaitForSeconds(jumpCooldown);
        if (grounded)
        {
            ResetDoubleJump();
        }
    }*/

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void CheckGrounded()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f);

        // handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
            isDoubleJumping = false;
            ResetDoubleJump();
            //rb.mass = 1f; Can make for a funny power where you only shoot in a straight line
        }
        else
        {
            rb.drag = 0;
            //rb.mass = 100f;
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
    private void ResetDoubleJump()
    {
        readyToJump = true;
        isDoubleJumping = false;
    }
}