using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Attack,
    walk,
    Damaged,
    Die,
    End
}


public class Enemy : MonoBehaviour
{
    public float hp;
    public float dmg;
    public float speed;
    public float attackSpeed;
    public bool isDie = false;
    protected EnemyState enemyState;

    public virtual void AttackTrigger()
    {

    }
    public virtual void DamagedTrigger(float dmg)
    {

    }
    public virtual void MovementTrigger()
    {

    }
    public virtual void DieTrigger()
    {

    }

    public virtual void IdleTrigger()
    {

    }

    public virtual void SwitchState(EnemyState state, float val = 0) // ���� ��ȯ�� �ѹ�����Ǿ���ϴ� �Լ� ex.) ����, �ִϸ��̼� ��ȯ��
    {
        enemyState = state;
        switch (enemyState)
        {
            case EnemyState.Idle:
                IdleTrigger();
                break;
            case EnemyState.walk:
                MovementTrigger();
                break;
            case EnemyState.Attack:
                AttackTrigger();
                break;
            case EnemyState.Damaged:
                DamagedTrigger(val);
                break;
            case EnemyState.Die:
                DieTrigger();
                break;
        }
    }
}
