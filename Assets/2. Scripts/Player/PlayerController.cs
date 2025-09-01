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
    [SerializeField] private Vector2 movementInput;
    [SerializeField] private Vector2 movementDirection;
    [SerializeField] private float jumpPower;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Ű�� ������ ������
        if(context.phase == InputActionPhase.Performed)
        {
            movementInput = context.ReadValue<Vector2>();
        }
        // Ű�� ���� 
        else if(context.phase == InputActionPhase.Canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    private void Move()
    {
        // �̵� ���� ����
        movementDirection = movementInput;

        // �̵� �ӵ� ����
        movementDirection *= moveSpeed;

        // �߷��� velocity.y������ ����
        movementDirection.y = rigid.velocity.y;

        // �̵� ó��
        rigid.velocity = movementDirection;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Ű�� ������ �������� �� 
        if(context.phase == InputActionPhase.Started)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
}
