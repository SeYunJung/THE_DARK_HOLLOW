using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScroller : MonoBehaviour
{
    public float moveSpeed = 2f;       // ���� �̵� �ӵ�
    public float resetX = -15f;        // �������� ���ġ�� ��ġ (ȭ�� ��)
    public float endX = 15f;           // ���������� �������� Ȯ���� ��ġ (ȭ�� ��)

    void Update()
    {
        // ���������� �̵�
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);

        // ȭ�� ��(endX)���� ������ ������ ����(resetX)���� �̵�
        if (transform.position.x > endX)
        {
            Vector3 pos = transform.position;
            pos.x = resetX;
            transform.position = pos;
        }
    }
}
