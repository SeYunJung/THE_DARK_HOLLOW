using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // �ν� ����(�� ������ ����� Idle ����)
    public float nearRange = 1.8f;  // ����(����) �Ӱ�
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
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Tooltip("����� �̸������(�߻� �ӵ�)")]
    public float bulletSpeed = 10f;         // FireProjectile������ ����ϵ��� ����
    [Tooltip("����� �̸������(����ü ���� �ð�)")]
    public float bulletLifetime = 2f;       // ���� �̵��Ÿ� = speed * lifetime

    [Header("Phase / HP")]
    [Range(100, 10000)] public float hp01 = 100;   

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
