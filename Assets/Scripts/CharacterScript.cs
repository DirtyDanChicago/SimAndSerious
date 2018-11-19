﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterScript : MonoBehaviour
{
    //Rigid body variable. 
    [SerializeField]
    private Rigidbody2D myRigidBody;

    //Force of jump variable.
    [SerializeField]
    private float jumpForce = 10;

    //Acceleration variable.
    [SerializeField]
    private float accelerationForce = 5;

    //Max speed variable.
    [SerializeField]
    private float maxSpeed = 5;

    //Player ground collider variable.
    [SerializeField]
    private Collider2D playerGroundCollider;

    //Input for moving and stopping physics.
    [SerializeField]
    private PhysicsMaterial2D playerMovingPhysicsMaterial, playerStoppingPhysicsMaterial;

    //Ground detect trigger.
    [SerializeField]
    private Collider2D groundDetectTrigger;

    [SerializeField]
    private ContactFilter2D groundContactFilter;

    //Health Variable
    [SerializeField]
    private int health = 3;

    private Collider2D[] groundHitDetectionResults = new Collider2D[16];

	private Animator myAnimator;

    private CircleCollider2D circleCollider2D;

    private bool isOnGround;

    private bool facingRight = true;

    //Horizontal, and vertical input variables.
    private float horizontalInput;

	private void Start()
	{
		myAnimator = GetComponent<Animator>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        
	}

	private void Update()
    {
        //Stops the rotation.
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.z = 0;
        transform.localEulerAngles = currentRotation;

        //On Ground Update.
        UpdateIsOnGround();

        //Horizontal Movement Function.
        UpdateHorizontalMovement();

        //Jump Input Function.
        HandleJumpInput();
    }

    //Fixed update for movement.
    void FixedUpdate()
    {
        UpdatePhysicsMaterial();

        //Calls Move function.
        Move();

        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

    }

    private void UpdatePhysicsMaterial()
    {
        if (Mathf.Abs(horizontalInput) > 0)
        {
            playerGroundCollider.sharedMaterial = playerMovingPhysicsMaterial;
        }

        else
        {
            playerGroundCollider.sharedMaterial = playerStoppingPhysicsMaterial;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void UpdateIsOnGround()
    {
        isOnGround = groundDetectTrigger.OverlapCollider(groundContactFilter, groundHitDetectionResults) > 0;

		//Debug.Log("Is On Ground?: " + isOnGround);
        //Changes from run to jump if in the air.
        if (isOnGround == true)
        {
            myAnimator.SetBool("isOnGround", true);
        }
        else
        {
            myAnimator.SetBool("isOnGround", false);
        }



    }


    private void UpdateHorizontalMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    //Movement function.
    private void Move()
    {
        myRigidBody.AddForce(Vector2.right * horizontalInput * accelerationForce);

        Vector2 clampedVelocity = myRigidBody.velocity;

        clampedVelocity.x = Mathf.Clamp(myRigidBody.velocity.x, -maxSpeed, maxSpeed);

        myRigidBody.velocity = clampedVelocity;

		myAnimator.SetFloat("speed", Mathf.Abs(horizontalInput));

    }


    //Jump function.
    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            myRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    

    //Kills the player upon entering it, reseting the level.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "DeathCollider")
        {
            Debug.Log("Player entered death collider.");
            SceneManager.LoadScene("Level1");
        }
    }


}
