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
    protected EnemyState enemyState;

    public virtual void Attack()
    {

    }
    public virtual void Damaged()
    {

    }
    public virtual void Movement()
    {

    }
    public virtual void Die()
    {

    }

    public virtual void SwitchState(EnemyState state) // ���� ��ȯ�� �ѹ�����Ǿ���ϴ� �Լ� ex.) ����, �ִϸ��̼� ��ȯ��
    {
        enemyState = state;
        switch (enemyState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.walk:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Die:
                break;
        }
    }
}
