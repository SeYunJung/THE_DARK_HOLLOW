using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // �ν� ����(�� ������ ����� Idle ����)
    public float nearRange = 1.8f;  // ����(����) �Ӱ�
    public float midRange = 3.0f; // �߰Ÿ� �Ӱ�
    public float farRange = 5.5f;  // ���Ÿ� �Ӱ�

    [Tooltip("�þ�/�Ÿ� ��� �ֱ�(Hz). 8~10 ����")]
    public float perceptionHz = 8f; // �������� �ֱ�

    [Header("Move Speeds")]
    public float moveSpeed = 3.5f;
    public float dashSpeed = 12f;

    [Header("Dash Timings")]
    public float dashWindup = 0.5f;
    public float dashActive = 0.28f;
    public float dashRecover = 0.35f;
    public float dashCooldown = 1.8f;

    [Header("Ranged Timings")]
    public float rangedWindup = 0.20f;
    public float rangedActive = 0.00f;     // ����ü�� Active=0�� OK
    public float rangedRecover = 0.35f;
    public float rangedCooldown = 1.4f;

    [Header("Ranged Projectile")]
    public Transform firePoint;

    [Tooltip("����� �̸������(�߻� �ӵ�)")]
    public float bulletSpeed = 10f;         // FireProjectile������ ����ϵ��� ����
    [Tooltip("����� �̸������(����ü ���� �ð�)")]
    public float bulletLifetime = 3f;       // ���� �̵��Ÿ� = speed * lifetime

    [Header("Phase / HP")]
    [Range(1, 100)] public int hp01;

    // ���� ��ȯ ����ġ 
    [Header("Utility / Selection")]
    [Range(0f, 1f)] public float minScoreToAct = 0.35f; // �� ���� �̸��̸� �������� �ʰ� �Ÿ� ����(Chase)
    [Range(0f, 0.5f)] public float stickiness = 0.15f;  // ���� ���� ���Ͽ� �ִ� ���ʽ�(�ø�Ŀ ����)
    [Range(0f, 0.25f)] public float utilityNoise = 0.02f; // �̼� ����(���� ����)

    [Tooltip("����þ� ��(��)�� ��ȣ�Ÿ��� �� ��� ����")]
    [Range(0.05f, 2f)] public float nearSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float midSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float farSigmaFrac = 0.6f;

    [Tooltip("���ݺ� ����ġ(�⺻=1). ������/������ �ǵ��� ���� ����")]
    public float weightDash = 1f;
    public float weightMid = 1f;
    public float weightRanged = 1f;

    void OnValidate()
    {
        perceptionHz = Mathf.Max(0.1f, perceptionHz);
        detectRange = Mathf.Max(0f, detectRange);
        nearRange = Mathf.Max(0f, nearRange);
        farRange = Mathf.Max(0f, farRange);
        dashSpeed = Mathf.Max(0f, dashSpeed);
        bulletSpeed = Mathf.Max(0f, bulletSpeed);
        bulletLifetime = Mathf.Max(0f, bulletLifetime);
    }
}
