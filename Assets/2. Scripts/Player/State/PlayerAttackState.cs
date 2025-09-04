using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : BaseState
{
    private float coolTime;
    private float lastAttackTime;
    private float lastInputTime;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello From The Attack State");
        coolTime = Constants.CoolTime.ATTACK;
        lastAttackTime = 0.0f;

        // Gauge count
        stateMachine.PlayerController.PlayerStat.Gauge += 1;
        if(stateMachine.PlayerController.PlayerStat.Gauge > 5)
            stateMachine.PlayerController.PlayerStat.Gauge = 5;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // ���� ��Ÿ���� ������ 
        if (Time.time - lastAttackTime > coolTime)
        {
            lastAttackTime = Time.time;

            // ���� �ִϸ��̼� ����
            Debug.Log("���� �ִϸ��̼� ����");
            stateMachine.PlayerController.AnimationController.Attack();
            lastInputTime = Time.time; // ���� �ִϸ��̼� ���� �ð��� ��� 
            Debug.Log($"������ �Է� �ð� = {lastInputTime}");
        }

        // ���� �ִϸ��̼� ���Ŀ� ����Ű(x)�� �Է��ϸ� 
        if (Input.GetKeyDown(KeyCode.X))
        {
            // �� �ð��� ��� 
            lastInputTime = Time.time;
        }

        Debug.Log($"���� �ð� - ������ �Է� �ð� = {Time.time - lastAttackTime}");
        // 0.5�ʰ� �����µ� �ƹ��� �Է��� ������
        if (Time.time - lastInputTime > coolTime)
        {
            Debug.Log("0.5�ʰ� �������� �ƹ��� �Է��� �����ϴ�. Idle ���·� ��ȯ�մϴ�.");
            stateMachine.PlayerController.AnimationController.CancelAttack();
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            return;
        }
    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {

    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

}
