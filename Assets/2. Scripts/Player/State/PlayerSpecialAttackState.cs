using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpecialAttackState : BaseState
{
    private PlayerController playerController;
    private bool specialAttack;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Special Attack State");
        this.playerController = stateMachine.PlayerController;

        // �������� ������� �˻� 
        if(stateMachine.PlayerController.PlayerStat.Gauge == Constants.SpecialAttack.GUAGE)
        {
            specialAttack = true;
        }
        else
        {
            specialAttack = false;
        }
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �������� ����ϰ�, Ư�� ����Ű(space)�� �������� 
        if(specialAttack)
        {
            // Ư�� ���� �ִϸ��̼� ���� 
            stateMachine.PlayerController.AnimationController.SpecialAttack();

            // ������ ���̱� 
            stateMachine.PlayerController.PlayerStat.Gauge = 0;

            // �ִϸ��̼� �ߺ� ���� ���� 
            specialAttack = false;
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        Move();
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    private void Move()
    {
        // �̵� ���� ����
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� ����
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // �߷��� velocity.y������ ����
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;

        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
