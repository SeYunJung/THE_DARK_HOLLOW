using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingState : BaseState
{
    private PlayerController playerController;
    private CompositeCollider2D coll;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Climbing State");
        this.playerController = stateMachine.PlayerController;

        // ���� ���� ���� 
        stateMachine.SetPreState(stateMachine);

        // Climb Animation
        //playerController.AnimationController.Run(Vector2.zero);
        //playerController.AnimationController.Move(playerController.MovementInput);
        //playerController.AnimationController.Climb();
        playerController.AnimationController.Climb(true);

        // �߷� ����
        playerController.Rigid.gravityScale = 0.0f;

        playerController.PlayerStat.isMoved = false;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �ݶ��̴����� ����� Idle���·� ��ȯ 
        if (!playerController.IsClimbable)
        {
            Debug.Log("���̵� ���·� ��ȯ");

            playerController.Rigid.gravityScale = 1.0f;

            playerController.PlayerStat.isMoved = true;

            coll.isTrigger = false;

            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        
        Move(stateMachine);
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    private void Move(StateMachine stateMachine)
    {
        Debug.Log("��ٸ� Ÿ�� ���� ����");
        // �̵� ���� 
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� 
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;


        // ��ٸ� Ÿ�� �� �ٴڿ� ���, �Ʒ�����Ű�� ������ 
        if (playerController.IsGrounded() && Input.GetKey(KeyCode.LeftControl))
        {
            Debug.Log("��ٸ� Ÿ�� �� �ٴڿ� ����");

            playerController.IsClimbable = false;
        }

        Collider2D ceilingCollider = playerController.GetCeilingCollider();
        if (ceilingCollider != null)
        {
            Debug.Log("õ�忡 ����");
            Debug.Log($"{ceilingCollider}");
            coll = ceilingCollider.GetComponent<CompositeCollider2D>();

            if (coll != null)
            {
                coll.isTrigger = true;
            }
        }
    }
}
