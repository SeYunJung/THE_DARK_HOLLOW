using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: Header("Init Info")]
    [field: SerializeField] public Rigidbody2D Rigid { get; set; }
    private BoxCollider2D coll;

    [field: Header("Player Info")]
    [field: SerializeField] public Vector2 MovementInput { get; private set; }
    [field: SerializeField] public Vector2 MovementDirection { get; set; }
    [field: SerializeField] public bool CanJump { get; set; }
    [field: SerializeField] public bool IsMoving { get; set; }

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateMachine stateMachine;
    private PlayerStat playerStat;

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateMachine = GetComponent<StateMachine>();
        playerStat = GetComponent<PlayerStat>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Ű�� ������ ������ (���� �ֵ� ���� ���X -> �̵��� ������, ���߿��� �� ����)
        if(context.phase == InputActionPhase.Performed)
        {
            // �̵� ���̶�� �� ǥ��
            IsMoving = true;

            // �̵��� �� �ְ� ������ �ο� 
            MovementInput = context.ReadValue<Vector2>();

            // �̵����� ��ȯ 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
        }
        // Ű�� ���� 
        else if (context.phase == InputActionPhase.Canceled)
        {
            // �̵� �� X
            IsMoving = false;

            MovementInput = Vector2.zero;

            // Idle ���� ��ȯ
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ����Ű(z)�� ������ �������� �� 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            CanJump = true;

            // Jump ���·� ��ȯ
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Jump));
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // �޸���Ű(shift)�� ������ �ְ�, ���� ������
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // Run ���·� ��ȯ -> ���� �ӵ��� 50%�� ������ �̵��� �� �ְ� �ϱ� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
        }

        // �޸���Ű(shift)�� ���� �� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            // Idle ���·� ��ȯ -> SpeedModifier = 1.0f;
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public bool IsGrounded()
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
