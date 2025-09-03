using UnityEngine;

// �߻� ���� Ŭ������ ��� 
// ��� �ʿ����. 
// concrte Ŭ�������� ���� 
public abstract class BaseState 
{
    public abstract void EnterState(StateMachine stateMachine);

    public abstract void UpdateState(StateMachine stateMachine);

    public abstract void FixedUpdateState(StateMachine stateMachine);

    public abstract void OnCollisionEnter(StateMachine stateMachine, Collision2D collision);

    public abstract void OnTriggerEnter(StateMachine stateMachine, Collision2D collision);
}
