using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("�ִϸ��̼ǿ�")]
    [Tooltip("������ �ִϸ��̼� ������")]
    public GameObject preAttack;
    [Tooltip("�ǰ� �ִϸ��̼� ������")]
    public GameObject hitEffect;


    [Header("���� ����(�ν�, ����)")]
    [Tooltip("�ν� ����(�����)")]
    public float detectRange = 10f;   // (�� ������ ����� Idle ����)
    [Tooltip("�������� ����(�ʷϻ�)")]
    public float nearRange = 1.8f;
    [Tooltip("�߰Ÿ� ���� ����(������)")]
    public float midRange = 3.0f;
    [Tooltip("���Ÿ� ���� ����(�Ķ���)")]
    public float farRange = 5.5f;

    [Header("���� �ν� �ֱ�")]
    [Tooltip("�þ�/�Ÿ� ��� �ֱ�(Hz). 8~10 ����")]
    public float perceptionHz = 8f; // �������� �ֱ�

    [Header("�������� �Ӽ� ����")]
    public float dashSpeed = 12f;
    public float dashWindup = 0.5f;
    public float dashActive = 0.28f;    // ���� ���ӽð�
    public float dashRecover = 0.35f;
    public float dashCooldown = 8f;

    [Header("�߰Ÿ� ���� �Ӽ� ����")]
    public float midWindup = 0.25f;
    public float midActive = 0.0f;
    public float midRecover = 0.35f;
    public float midCooldown = 5f;
    [Tooltip("�߰Ÿ� ���� ������(��ġ��ȯ��)")]
    public GameObject midAttackPrefab;
    [Tooltip("�߰Ÿ� ���� ������ ��ġ ������")]
    public float midOffsetX = 2.2f;

    [Header("���Ÿ� ���� ����")]
    public float rangedWindup = 0.20f;
    public float rangedActive = 0.00f;     // ����ü�� 0�� OK
    public float rangedRecover = 0.35f;
    public float rangedCooldown = 1.4f;
    public float bulletSpeed = 10f;
    public float bulletLifetime = 3f;       // ���� �̵��Ÿ� = speed * lifetime
    [Tooltip("���Ÿ����� ����Ʈ(���� ��ġ��)")]
    public Transform firePoint;

    [Header("HP")]
    [Range(1, 100)] public int hp01;

    // ���� ��ȯ ����ġ 
    [Header("Utility")]
    [Range(0f, 1f)] public float minScoreToAct = 0.35f; // �� ���� �̸��̸� �������� �ʰ� �Ÿ� ����(Chase)
    [Range(0f, 0.5f)] public float stickiness = 0.15f;  // ���� ���� ���Ͽ� �ִ� ���ʽ�(�ø�Ŀ ����)
    [Range(0f, 0.25f)] public float utilityNoise = 0.02f; // �̼� ����(���� ����)

    [Header("����þ� ��(��)�� ��ȣ�Ÿ��� �� ��� ����")]
    [Range(0.05f, 2f)] public float nearSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float midSigmaFrac = 0.6f;
    [Range(0.05f, 2f)] public float farSigmaFrac = 0.6f;

    [Header("���ݺ� ����ġ(�⺻=1). ������/������ �ǵ��� ���� ����")]
    public float weightDash = 1f;
    public float weightMid = 1f;
    public float weightRanged = 1f;

    [Header("�ٴ� ����(�־ ���� 0�� ����)")]
    [Range(0f, 0.5f)] public float baseBiasDash = 0.05f;
    [Range(0f, 0.5f)] public float baseBiasMid = 0.05f;
    [Range(0f, 0.5f)] public float baseBiasRanged = 0.05f;

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
