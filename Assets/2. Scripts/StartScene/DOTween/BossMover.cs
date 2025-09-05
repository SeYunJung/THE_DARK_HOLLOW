using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;       // ���� �Ƿ翧 �̵� �ӵ�
    public float setY = -8f;        // �Ʒ��� ������ (ȭ�� ��)
    public float endY = 0f;           // ���� ������ (ȭ�� ��)

    [Header("DOTween")]
    public float distance = 0.5f;
    public float duration = 1f;

    private bool isFloating = false;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = setY;
        transform.position = pos;
    }

    void Update()
    {
        if (isFloating) return; // �ݺ� � ���� Update ����

        // ���� �̵�
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // ������(endY)�� �����ϸ� ����
        if (transform.position.y >= endY)
        {
            Vector3 pos = transform.position;
            pos.y = endY;
            transform.position = pos;

            StartFloating();
            isFloating = true;
        }
    }

    void StartFloating()
    {
        // ���Ʒ��� �ݺ��ϴ� DOTween ����
        transform.DOMoveY(endY + distance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ� (��/�Ʒ�)
    }
}
