using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [Tooltip("���ʡ������ ������, ���� ����ִ� ĭ���� ����ִ� �迭(�ִ� 5��)")]
    [SerializeField] private List<LifeIcon> lifeIcons = new List<LifeIcon>();

    [Tooltip("�÷��̾� ���� ������(�ڵ� Ž����)")]
    [SerializeField] private PlayerStatObserver observer;

    // ====== �߰�: �ֱ� Ž�� ���� ======
    [SerializeField] private float findInterval = 0.5f;

    private bool subscribed;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
    }

    private void OnEnable()
    {
        // ��� ���� �õ�
        TrySubscribe(observer);
        // �׸��� �ֱ� ���� ����
        StartCoroutine(ObserverWatchdog());
    }

    private void OnDisable()
    {
        TryUnsubscribe();
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator ObserverWatchdog()
    {
        var wait = new WaitForSeconds(findInterval);

        while (enabled)
        {
            // ������ ���ų� �ı��Ǿ����� ��Ž��
            if (observer == null)
            {
                var found = FindObjectOfType<PlayerStatObserver>();
                if (found != null)
                {
                    observer = found;
                    TrySubscribe(observer); // ���� ã���� ��� ����
                }
            }
            else
            {
                // ������ �ִµ� ������ �������� ����
                if (!subscribed)
                    TrySubscribe(observer);

                // �������� ��Ȱ��/�ı��Ǿ����� ����
                if (!observer || !observer.isActiveAndEnabled)
                    TryUnsubscribe();
            }

            yield return wait;
        }
    }

    private void TrySubscribe(PlayerStatObserver target)
    {
        if (target == null || subscribed) return;
        target.OnHealthChanged += HandleHealthChanged;
        subscribed = true;
    }

    private void TryUnsubscribe()
    {
        if (!subscribed) return;
        if (observer != null)
            observer.OnHealthChanged -= HandleHealthChanged;
        subscribed = false;
        observer = null;
    }

    private void HandleHealthChanged(int current, int max)
    {
        // ���� ���ô� ü���� �پ�� ���� �����ʺ��� �����ϴ� ������ ����־���.
        // �ʿ��ϸ� ü�� ����(������ ����)�� ���⼭ �ٷ�� ��(������/Ǯ �ʿ�).
        while (lifeIcons.Count > current)
        {
            int last = lifeIcons.Count - 1;
            var icon = lifeIcons[last];
            lifeIcons.RemoveAt(last);
            if (icon) icon.IconDestroy();
        }
    }
}
