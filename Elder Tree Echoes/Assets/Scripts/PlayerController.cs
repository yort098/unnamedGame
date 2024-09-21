using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 3f, jumpForce = 3f;

    private Vector2 direction = Vector3.zero;

    // Represents all collidable platforms the player
    // can use/land on
    [SerializeField]
    LayerMask groundLayer;

    // Using rigid bodies for now, can change to a more robust system later
    private Rigidbody2D body; 
    private bool isFacingRight = true;

    /// <summary>
    /// Returns the player's speed
    /// </summary>
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Makes the player move
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // Makes the player start moving based on which key is pressed
        // A = (-1, 0), D = (1, 0)
        direction = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded() && context.performed) // Can only jump when touching the ground
        {
            Vector3 jumpVelocity = new Vector3(body.velocity.x, jumpForce);
            body.velocity = jumpVelocity;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector3(direction.x * speed, body.velocity.y);

        // Changing the direction the character is facing
        // based on the direction the player is moving
        if (!isFacingRight && direction.x > 0f)
        {
            Flip();
        }
        else if (isFacingRight && direction.x < 0f)
        {
            Flip();
        }
    }

    /// <summary>
    /// Determines whether or not the player is touching the ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        //  The pivot of the empty game player object should be at the BOTTOM of the player sprite
        //  or else this will not work as intended
        return Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
    }

    /// <summary>
    /// Changes the current direction the player is facing
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Reverses the sprite on the x-axis (horizontal)
        transform.localScale = localScale;
    }
}