using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;       // ���� �Ƿ翧 �̵� �ӵ�
    public float setY = -8f;        // �Ʒ��� ������ (ȭ�� ��)
    public float endY = 0f;           // ���� ������ (ȭ�� ��)

    void Update()
    {
        // ���� �̵�
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // ������(endY)�� �����ϸ� ����
        if (transform.position.y >= endY)
        {
            Vector3 pos = transform.position;
            pos.y = endY;
            transform.position = pos;
        }
    }
}
