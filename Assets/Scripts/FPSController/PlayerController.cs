using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

interface IInteractable
{
    public void OnInteract();
    string GetPrompt();
}
public class PlayerController : MonoBehaviour, IDrinkEffect
{
    [Header("Motion")]
    public Transform orientation;
    [SerializeField] WeaponSway weaponSway;
    [SerializeField] ViewBobbing viewBob;
    [SerializeField] Animator handAnimator;
    [SerializeField] GameObject normalView;
    [SerializeField] GameObject selfView;

    [Header("Interaction")]
    public Transform interactorSource;
    public float interactRange;
    public GameObject interactPanel;
    [SerializeField] TextMeshProUGUI interactText;
    Player player;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public bool canMove;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        InteractWith();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    public void ApplyEffect(Drink drink)
    {
        switch (drink)
        {
            case SpeedDrink speedDrink:
                walkSpeed += speedDrink.speedBoost;
                sprintSpeed += speedDrink.speedBoost;
                break;
        }
    }
    private void MyInput()
    {
        if (!canMove) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            player.shootSelf = !player.shootSelf;
            normalView.SetActive(!player.shootSelf);
            selfView.SetActive(player.shootSelf);

            if (player.shootSelf)
            {
                //handAnimator.SetTrigger("ToSelf");
                selfView.GetComponent<CamShake>().canShake = true;
            }
            else
            {
                //handAnimator.SetTrigger("ToNormal");
                //normalView.gameObject.SetActive(false);
                //selfView.gameObject.SetActive(true);
            }
        }
    }

    private void StateHandler()
    {
        if (!canMove) return;

        if (player.sanity <= 0) canMove = false;
        else canMove = true;

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            weaponSway.multiplier = 10f;
            viewBob.EffectSpeed = 10f;
            viewBob.EffectIntensity = 0.1f;
            player.sanitySpeedMultiplier = 2f;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            weaponSway.multiplier = 5f;
            viewBob.EffectSpeed = 5f;
            viewBob.EffectIntensity = 0.03f;
            player.sanitySpeedMultiplier = 1f;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        if (!canMove) return;
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (!canMove) return;

        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        if (!canMove) return;

        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
    void InteractWith()
    {
        if (!FindObjectOfType<RevolverController>().canInteract)
        {
            interactPanel.SetActive(false);
            return;
        }

        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactPanel.SetActive(true);
                interactText.text = $"{interactObj.GetPrompt()} ('E')";
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.OnInteract();
                }
            }
            else
            {
                interactPanel.SetActive(false);
            }
        }
        else
        {
            interactPanel.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        if (interactorSource != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(interactorSource.position, interactorSource.position + interactorSource.forward * interactRange);
        }
    }
}
