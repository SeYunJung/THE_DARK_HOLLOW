using UnityEngine;

// �߻� ���� Ŭ������ ��� 
// ��� �ʿ����. 
// concrte Ŭ�������� ���� 
public abstract class BaseState 
{
    public abstract void EnterState(StateManager stateManager);

    public abstract void UpdateState(StateManager stateManager);

    public abstract void FixedUpdateState(StateManager stateManager);

    public abstract void OnCollisionEnter(StateManager stateManager, Collision2D collision);
}
