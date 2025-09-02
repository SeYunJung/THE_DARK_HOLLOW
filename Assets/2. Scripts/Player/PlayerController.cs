using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("Init Info")]
    //private Rigidbody2D rigid;
    [field: SerializeField] public Rigidbody2D Rigid { get; set; }
    private BoxCollider2D coll;

    [field: Header("Player Info")]
    //[SerializeField] private float moveSpeed;
    [field: SerializeField] public float MoveSpeed { get; private set; }
    //[SerializeField] private float speedModifier;
    [field: SerializeField] public float SpeedModifier { get; set; }
    //[SerializeField] private float speedModifierInput;
    [field: SerializeField] public float SpeedModifierInput { get; private set; }
    //[SerializeField] private Vector2 movementInput;
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    //[SerializeField] private Vector2 movementDirection;
    [field: SerializeField] public Vector2 MovementDirection { get; set; }
    //[SerializeField] private float jumpPower;
    [field: SerializeField] public float JumpPower { get; private set; }
    //[SerializeField] private bool canJump;
    [field: SerializeField] public bool CanJump { get; set; }

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateManager stateManager;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateManager = GetComponent<StateManager>();
    }

    private void FixedUpdate()
    {
        //Move();
        //Jump();

        // Draw ray
        //Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0)); // �ȵ�.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Ű�� ������ �ְ�, ���� ������ 
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // �̵��� �� �ְ� ������ �ο� 
            MovementInput = context.ReadValue<Vector2>();

            // �̵����� ��ȯ 
            //stateManager.SwitchState(stateManager.moveState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Move));
        }
        // Ű�� ���� 
        else if (context.phase == InputActionPhase.Canceled)
        {
            MovementInput = Vector2.zero;

            // Idle ���� ��ȯ
            //stateManager.SwitchState(stateManager.idleState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Idle));
        }
    }

    //private void Move()
    //{
    //    // �̵� ���� ����
    //    MovementDirection = MovementInput;

    //    // �̵� �ӵ� ����
    //    MovementDirection *= (MoveSpeed * SpeedModifier);

    //    // �߷��� velocity.y������ ����
    //    Vector2 dir = MovementDirection;
    //    dir.y = Rigid.velocity.y;
    //    MovementDirection = dir;
    //    //MovementDirection.y = Rigid.velocity.y;

    //    // �̵� ó��
    //    Rigid.velocity = MovementDirection;
    //}

    public void OnJump(InputAction.CallbackContext context)
    {
        // ����Ű(z)�� ������ �������� �� 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CanJump = true;

            // Jump ���·� ��ȯ
            //stateManager.SwitchState(stateManager.jumpState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Jump));
        }
    }

    //private void Jump()
    //{
    //    if (CanJump)
    //    {
    //        Rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    //        CanJump = false;
    //    }
    //}

    public void OnRun(InputAction.CallbackContext context)
    {
        // �޸���Ű(shift)�� ������ �ְ�, ���� ������
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // �޸��� ó�� 
            // ���� �ӵ��� 50%�� ������ �̵��� �� �ְ� �ϱ� 
            //SpeedModifier = SpeedModifierInput;

            // Run ���·� ��ȯ 
            //stateManager.SwitchState(stateManager.runState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Run));
        }

        // �޸���Ű(shift)�� ���� �� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            //SpeedModifier = 1.0f;

            // Idle ���·� ��ȯ
            //stateManager.SwitchState(stateManager.idleState);
            stateManager.SwitchState(stateManager.Getstates(PlayerStateType.Idle));
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        if(hit.collider != null)
            return true;

        return false;
    }

    // test
    public void AddHealth()
    {
        Debug.Log("Add Health!!");
    }

    // test
    public void DetractHealth()
    {
        Debug.Log("Detract Health!!");
    }
}
