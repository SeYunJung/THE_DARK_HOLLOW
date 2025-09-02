using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
     * ��������. 
     * 
     * �����̷��� ���� �ʿ���? 
     * - ������ �ٵ�
     * - �̵� �ӵ� 
     * - �ִϸ��̼�
     * 
     * �÷��̾ ���̳� ������Ʈ�� �浹�ϸ� �����ߵ�. 
     * �浹ó���� �Ϸ��� ���� �ʿ���? 
     * - �ݶ��̴�
     * 
     * �̵���Ű���� �÷��̾��� �Է��� �ʿ���. 
     * �÷��̾� �Է��� ���� ó���ұ�? 
     * 
     */

    [Header("Init Info")]
    private Rigidbody2D rigid;
    private BoxCollider2D coll;

    [Header("Player Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedModifier;
    [SerializeField] private float speedModifierInput;
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool canJump;

    [Header("Etc")]
    [SerializeField] private LayerMask groundLayer;
    private StateManager stateManager;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        stateManager = GetComponent<StateManager>();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();

        // Draw ray
        //Debug.DrawRay(transform.position, Vector3.down, new Color(1, 0, 0)); // �ȵ�.
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Ű�� ������ �ְ�, ���� ������ 
        if (context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // �̵��� �� �ְ� ������ �ο� 
            movementInput = context.ReadValue<Vector2>();

            // �̵����� ��ȯ 
            stateManager.SwitchState(stateManager.moveState);
        }
        // Ű�� ���� 
        else if (context.phase == InputActionPhase.Canceled)
        {
            movementInput = Vector2.zero;

            // Idle ���� ��ȯ
            stateManager.SwitchState(stateManager.idleState);
        }
    }

    private void Move()
    {
        // �̵� ���� ����
        movementDirection = movementInput;

        // �̵� �ӵ� ����
        movementDirection *= (moveSpeed * speedModifier);

        // �߷��� velocity.y������ ����
        movementDirection.y = rigid.velocity.y;

        // �̵� ó��
        rigid.velocity = movementDirection;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // ����Ű(z)�� ������ �������� �� 
        if (context.phase == InputActionPhase.Started && IsGrounded())
            canJump = true;
    }

    private void Jump()
    {
        if (canJump)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            canJump = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        // �޸���Ű(shift)�� ������ �ְ�, ���� ������
        if(context.phase == InputActionPhase.Performed && IsGrounded())
        {
            // �޸��� ó�� 
            // ���� �ӵ��� 50%�� ������ �̵��� �� �ְ� �ϱ� 
            speedModifier = speedModifierInput;
        }

        // �޸���Ű(shift)�� ���� �� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            speedModifier = 1.0f;
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
