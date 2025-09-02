using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BossStateBase
{
    public ChaseState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        //ctx.Play("Run"); // �ִ� Ʈ���Ÿ� ���߱�
    }
    public override void Tick(float dt)
    {
        if (!ctx.CanSeePlayer()) { fsm.Change(ctx.SIdle); return; }

        float d = ctx.GetComponent<BossStat>().nearRange; 
        float far = ctx.GetComponent<BossStat>().farRange;

        // ��Ÿ�(����/���Ÿ�) �Ӱ迡 ������ ��������
        if (ctx.CanSeePlayer() && (ctx.Dist <= d || ctx.Dist >= far))
        { fsm.Change(ctx.SChoose); return; }

        // �߰� �̵�
        if (ctx.player) ctx.MoveTowards(ctx.player.position, ctx.stat.moveSpeed);
        ctx.FaceToPlayer();
    }
    public override void OnExit() 
    { 
        ctx.StopMove(); 
    }
}
