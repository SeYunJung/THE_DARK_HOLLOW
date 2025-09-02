using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    [Header("Aggro / Perception")]
    public float detectRange = 10f;   // �ν� ����(�� ������ ����� Idle ����)
    public float nearRange = 1.8f;    // ����(����) �Ӱ�
    public float farRange = 5.5f;    // ���Ÿ� �Ӱ�

    [Header("Move Speeds")]
    public float moveSpeed = 3.5f;
    public float dashSpeed = 12f;

    [Header("Dash Timings")]
    public float dashWindup = 0.20f;
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

    [Header("Phase / HP")]
    [Range(0, 1)] public float hp01 = 1f;    // 0~1 ����ȭ HP (���ϸ� �ܺο��� ����)
}
