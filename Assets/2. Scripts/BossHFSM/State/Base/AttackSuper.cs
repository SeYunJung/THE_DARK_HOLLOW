using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ����(Composite) ����: ���� ����������(Windup��Execute��Recover) ����
public abstract class AttackSuper : BossStateBase
{
    protected IBossState sub;
    protected AttackSuper(BossController c, BossStateMachine f) : base(c, f) { }

    protected abstract IBossState DefaultSub();
    protected void ChangeSub(IBossState next) { sub?.OnExit(); sub = next; sub?.OnEnter(); }
    public override void OnEnter() { ChangeSub(DefaultSub()); }
    public override void OnExit() { sub?.OnExit(); sub = null; }
    public override void Tick(float dt) { sub?.Tick(dt); }
    public override void FixedTick(float fdt) { sub?.FixedTick(fdt); }

    // ���� ����(��)���� �ܺ� �̺�Ʈ�ε� ���̸� ���� ������ locked ����,
    // �ν� ���� ��Ż�� ��Ʈ�ѷ����� ���� Idle ó��(Force)�� �ذ�.
}
