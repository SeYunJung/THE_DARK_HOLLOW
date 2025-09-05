using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialInput : MonoBehaviour
{
    [SerializeField] private PlayerStatObserver observer;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && observer != null && observer.Stat != null)
        {
            // ���� á�� ���� Ư������ ���
            if (observer.Stat.Gauge >= observer.MaxGauge)
            {
                observer.Stat.Gauge = 0;
                // GaugeUI�� Observer�� ��ȭ �����ؼ� �׵θ� ��
            }
        }
    }
}
