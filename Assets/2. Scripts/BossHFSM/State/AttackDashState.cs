using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��������������������������������������������������������������������������������������������������������������������������
// Attack: ����(Dash)
// ����������: Windup -> Execute(dash move) -> Recover
// ��������������������������������������������������������������������������������������������������������������������������
public class AttackDashState : AttackSuper
{
    public AttackDashState(BossController c, BossStateMachine f) : base(c, f) { }
    public override string Name => "Attack.Dash";

    protected override IBossState DefaultSub() => new A_Windup(ctx, fsm, this);

    //A_Windup : ���� �غ� �ܰ�
    // - �̵��� ����, �ִ� Ʈ����, ������ �̸� ���/����(��ġ)
    // - dashWindup ��� �� Execute�� ��ȯ
    class A_Windup : BossStateBase
    {
        readonly AttackDashState sup; float t;
        public A_Windup(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0f;
            sup.locked = true; ctx.StopMove();
            ctx.OnPreAttackEffect();
            SoundManager.Instance.PlayEFXSound("BossRush_EFX");
        } 
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.dashWindup)
                sup.ChangeSub(new A_Execute(ctx, fsm, sup));
        }
    }

    class A_Execute : BossStateBase
    {
        readonly AttackDashState sup; float t; Vector2 dir;
        public A_Execute(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter()
        {
            t = 0;
            ctx.FaceToPlayer();
            if (ctx.player) dir = (ctx.player.position - ctx.transform.position).normalized;
            else dir = new Vector2(ctx.transform.localScale.x, 0f);
            //ctx.Play("Dash");
        }
        public override void Tick(float dt)
        {
            t += dt;
            // �̵�(����/���� ���ϸ� ����)
            ctx.rb.velocity = dir * ctx.stat.dashSpeed;
            if (t >= ctx.stat.dashActive)
            {
                ctx.StopMove();
                sup.ChangeSub(new A_Recover(ctx, fsm, sup));
            }
        }
    }

    class A_Recover : BossStateBase
    {
        readonly AttackDashState sup; float t;
        public A_Recover(BossController c, BossStateMachine f, AttackDashState s) : base(c, f) { sup = s; }
        public override void OnEnter() 
        { 
            t = 0; 
            //ctx.Play("Dash_Recover"); 
        } 
        public override void Tick(float dt)
        {
            t += dt;
            if (t >= ctx.stat.dashRecover)
            {
                sup.locked = false;
                ctx.StartCD_Dash();
                // ��������
                if (!ctx.CanSeePlayer()) 
                    fsm.Change(ctx.SIdle);

                else if (ctx.Dist <= ctx.stat.nearRange || ctx.Dist >= ctx.stat.farRange) 
                    fsm.Change(ctx.SChoose);
            }
        }
    }
}
