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

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log("����");
    }
}
