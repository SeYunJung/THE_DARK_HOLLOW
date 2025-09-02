using UnityEngine;

// �߻� ���� Ŭ������ ��� 
// ��� �ʿ����. 
// concrte Ŭ�������� ���� 
public abstract class BaseState 
{
    public abstract void EnterState(StateManager player);

    public abstract void UpdateState(StateManager player);

    public abstract void FixedUpdateState(StateManager player);

    public abstract void OnCollisionEnter(StateManager player, Collision2D collision);
}
