using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseState : BossStateBase
{
    public ChooseState(BossController c, BossStateMachine f) : base(c, f) { }
    public override void OnEnter()
    {
        if (!ctx.CanSeePlayer()) 
        { 
            fsm.Change(ctx.SIdle); 
            return; 
        }
        float d = ctx.Dist;
        var s = ctx.stat;

        //===============[��ƿ��Ƽ ���]===============
        // ���ο� ���� ������ ���涧 ���� ���⿡ �߰�
        // �Ÿ� �� ����þ� ����
        float sigmaNear = Mathf.Max(0.01f, s.nearRange * s.nearSigmaFrac);
        float sigmaMid = Mathf.Max(0.01f, s.nearRange * s.nearSigmaFrac);
        float sigmaFar = Mathf.Max(0.01f, s.farRange * s.farSigmaFrac);
        // ����þ� ���� * ��Ÿ�� ���� ����(0/1) * ����ġ
        float scoreDash = Gauss(d, s.nearRange, sigmaNear) * (ctx.CDReadyDash() ? 1f : 0f) * s.weightDash;
        float scoreMid = Gauss(d, s.nearRange, sigmaNear) * (ctx.CDReadyDash() ? 1f : 0f) * s.weightDash;
        float scoreRng = Gauss(d, s.farRange, sigmaFar) * (ctx.CDReadyRanged() ? 1f : 0f) * s.weightRanged;

        // ������ ���õ� ���� ������ �ٽ� ���õ� Ȯ�� ����
        if (ctx.lastChosen == BossController.AttackChoice.Dash) scoreDash *= (1f + s.stickiness);
        if (ctx.lastChosen == BossController.AttackChoice.Mid) scoreDash *= (1f + s.stickiness);
        if (ctx.lastChosen == BossController.AttackChoice.Ranged) scoreRng *= (1f + s.stickiness);

        // �̼��� �������� �߰��� ��ħ ����
        scoreDash += Random.Range(-s.utilityNoise, s.utilityNoise);
        scoreMid += Random.Range(-s.utilityNoise, s.utilityNoise);
        scoreRng += Random.Range(-s.utilityNoise, s.utilityNoise);

        float best = Mathf.Max(scoreDash, scoreRng, scoreMid);
        //===============[��ƿ��Ƽ ���]===============

        // ������ ��ŭ ������ ������ �Ÿ� ������ ����
        if (best < s.minScoreToAct)
        {
            fsm.Change(ctx.SRecover); 
            return;
        }
        // 7) ���� + ������ ���� ����
        if (best == scoreDash)
        {
            ctx.lastChosen = BossController.AttackChoice.Dash;
            fsm.Change(ctx.SAtkDash);
        }
        else if (best == scoreRng)
        {
            ctx.lastChosen = BossController.AttackChoice.Ranged;
            fsm.Change(ctx.SAtkRng);
        }
        else if (best == scoreMid)
        {
            ctx.lastChosen = BossController.AttackChoice.Mid;
            fsm.Change(ctx.SAtkMid);
        }

    }
    // ����þ� ��ƿ��Ƽ �Լ�: �� �߽�, �� ��
    // x�� �쿡 �������� 1�� ����� ����, �־������� 0�� ����� ����
    static float Gauss(float x, float mu, float sigma)
    {
        if (sigma <= 1e-4f) return (Mathf.Abs(x - mu) <= 1e-4f) ? 1f : 0f;
        float z = (x - mu) / sigma;
        return Mathf.Exp(-0.5f * z * z);
    }
}
