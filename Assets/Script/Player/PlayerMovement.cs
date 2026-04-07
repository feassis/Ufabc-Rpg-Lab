using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovementData data;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 lastDir;
    private bool isSprinting;
    private bool isDashing;
    private float dashTimer = 0;
    private float dashTimerCoolDown = 0;

    public void SetMoveInput( Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    private void Awake()
    {
        PlayerInputHandler.OnMoveInput += OnMoveInput;
        PlayerInputHandler.OnSprintInput += OnSprintInput;
        PlayerInputHandler.OnDashInput += OnDashInput;
    }

    private void OnDashInput()
    {
        if(Mathf.Max(dashTimer, dashTimerCoolDown) <= 0)
        {
            isDashing = true;
            dashTimer = data.DashDuration;
            dashTimerCoolDown = data.DashCoolDown;
        }
    }

    private void OnSprintInput(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnMoveInput -= OnMoveInput;
        PlayerInputHandler.OnSprintInput -= OnSprintInput;
        PlayerInputHandler.OnDashInput -= OnDashInput;
    }

    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;


        if(input != Vector2.zero)
        {
            lastDir = moveInput.normalized;
        }
    }

    private void Update()
    {
        if(dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;

            if(dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        if(dashTimerCoolDown  > 0)
        {
            dashTimerCoolDown -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (isDashing && dashTimer > 0)
        {
            var velocity = lastDir * data.DashSpeed;
            rb.linearVelocity = velocity;
        }
        else
        {
            var velocity = moveInput.normalized * GetVelocity();

            rb.linearVelocity = velocity;
        }       
    }

    private void Rotate()
    {
        Vector3 direction = PlayerInputHandler.GetMousePosInWorld() - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.SetRotation(targetAngle);
    }

    private float GetVelocity() => isSprinting ? data.SprintSpeed : data.Speed;

    private void FixedUpdate()
    {
        Move();

        Rotate();
    }
}
