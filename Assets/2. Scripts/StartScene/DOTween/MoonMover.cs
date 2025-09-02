using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoonMover : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float setY = 3f;

    [Header("DOTween")]
    public float distance = 0.25f;
    public float duration = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = setY;
        transform.position = pos;

        // ���Ʒ��� �ݺ��ϴ� DOTween ����
        transform.DOMoveY(setY + distance, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // ���� �ݺ� (��/�Ʒ�)
    }

}
