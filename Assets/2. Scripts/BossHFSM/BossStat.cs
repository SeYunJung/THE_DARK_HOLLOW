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

    [Header("���� ���� ������")]
    [Tooltip("� ���� �Ŀ��� ���� ���ñ��� �ּ� ���")]
    public float decisionDelay = 1.5f;

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

}
