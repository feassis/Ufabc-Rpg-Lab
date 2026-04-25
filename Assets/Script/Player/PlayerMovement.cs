using UnityEngine;

//classe que controla o movimento do jogador
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerMovementData data;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Stats stats;

    private Vector2 moveInput;
    private Vector2 lastDir;
    private bool isSprinting;
    private bool isDashing;
    private float dashTimer = 0;
    private float dashTimerCoolDown = 0;
    private float speedMultiplier = 1f;

    public Vector2 GetMoveInput() => moveInput;
    public PlayerMovementData Data => data;

    enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public void SetMoveInput( Vector2 moveInput)
    {
        this.moveInput = moveInput;
    }

    //se inscreve aos eventos o input handler
    private void Awake()
    {
        PlayerInputHandler.OnMoveInput += OnMoveInput;
        PlayerInputHandler.OnSprintInput += OnSprintInput;
        PlayerInputHandler.OnDashInput += OnDashInput;

        if (stats == null)
        {
            stats = GetComponent<Stats>();
        }
    }

    //metodo chamado ao apertar o botão de dash
    private void OnDashInput()
    {
        if(Mathf.Max(dashTimer, dashTimerCoolDown) <= 0)
        {
            isDashing = true;
            dashTimer = GetDashDuration();
            dashTimerCoolDown = GetDashCooldown();
        }
    }

    //metodo chamado ao apertar o botão de sprint
    private void OnSprintInput(bool isSprinting)
    {
        this.isSprinting = isSprinting;
    }

    //se remove aos eventos o input handler
    private void OnDestroy()
    {
        PlayerInputHandler.OnMoveInput -= OnMoveInput;
        PlayerInputHandler.OnSprintInput -= OnSprintInput;
        PlayerInputHandler.OnDashInput -= OnDashInput;
    }

    //metodo chamado ao usar os movimentos de movimento
    private void OnMoveInput(Vector2 input)
    {
        moveInput = input;


        if(input != Vector2.zero)
        {
            lastDir = moveInput.normalized;
        }
    }


    //nesse update os timers são atualizados com o framerate 
    private void Update()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }

        if (dashTimerCoolDown > 0)
        {
            dashTimerCoolDown -= Time.deltaTime;
        }
    }

    //metodo de movimentação do jogador
    private void Move()
    {
        var velocity = rb.linearVelocity;
        //se esta durante o dash
        if (isDashing && dashTimer > 0)
        {
            velocity = lastDir * GetDashSpeed();
            rb.linearVelocity = velocity;
        }
        // movimentação normal
        else
        {
            velocity = moveInput.normalized * GetVelocity();

            rb.linearVelocity = velocity;
        }       

        //flip do sprite
        if (velocity.x > 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }

        //flip do sprite
        if (velocity.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        
        
    }

    private void Rotate()
    {
        Vector3 direction = PlayerInputHandler.GetMousePosInWorld() - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        rb.SetRotation(targetAngle);
    }

    public void MultiplySpeed(float multiplier)
    {
        if (stats != null)
        {
            stats.MultiplyMoveSpeed(multiplier);
            return;
        }

        speedMultiplier *= Mathf.Max(0.1f, multiplier);
    }

    private float GetVelocity()
    {
        if (stats != null)
        {
            return isSprinting ? stats.SprintSpeed : stats.Speed;
        }

        return (isSprinting ? data.SprintSpeed : data.Speed) * speedMultiplier;
    }

    private float GetDashSpeed() => stats != null ? stats.DashSpeed : data.DashSpeed;

    private float GetDashDuration() => stats != null ? stats.DashDuration : data.DashDuration;

    private float GetDashCooldown() => stats != null ? stats.DashCooldown : data.DashCoolDown;

    private void FixedUpdate()
    {
        Move();

        //Rotate();
    }
}
