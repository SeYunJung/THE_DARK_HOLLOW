using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [Tooltip("���ʡ������ ������, ���� ����ִ� ĭ���� ����ִ� �迭(�ִ� 5��)")]
    [SerializeField] private List<LifeIcon> lifeIcons = new List<LifeIcon>();

    [Tooltip("�÷��̾� ���� ����")]
    [SerializeField] private PlayerStatObserver observer;

    private void Awake()
    {
        if(!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void OnEnable()
    {
        if (observer != null) observer.OnHealthChanged += HandleHealthChanged;
    }

    private void OnDisable()
    {
        if (observer != null) observer.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        while (lifeIcons.Count > current)
        {
            int last = lifeIcons.Count - 1;
            var icon = lifeIcons[last];
            lifeIcons.RemoveAt(last);
            if (icon) icon.IconDestroy();
        }
        // ���� ����ִ� ������ ���� current�� �ǵ��� �����.
        // �پ�� ��ŭ ����(������ ��)���� �Ͷ߸���.
        // ��) 5��4�� ������ �ε��� 4�� �ı�

    }

}
