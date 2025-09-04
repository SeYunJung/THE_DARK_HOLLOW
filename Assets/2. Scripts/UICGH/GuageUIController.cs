using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageUIController : MonoBehaviour
{
    [SerializeField] private PlayerStatObserver observer;

    [Header("������ ǥ�� Ÿ��")]
    [SerializeField] private Image uiImage;                 // Canvas UI��
    [SerializeField] private SpriteRenderer spriteRenderer; // ���� �������Ʈ��

    [Header("������ �ܰ� ��������Ʈ (0=�� ������, ... , Ǯ ������)")]
    [SerializeField] private List<Sprite> gaugeSprites = new();

    [Tooltip("���� ���� Ȱ��ȭ�� �׵θ� (�ʱ� ��Ȱ��ȭ)")]
    [SerializeField] private GameObject borderHighlight;

    private int lastIndex = -1;

    private void Awake()
    {
        if (!observer) observer = FindObjectOfType<PlayerStatObserver>();
        if (borderHighlight) borderHighlight.SetActive(false);
    }

    private void OnEnable()
    {
        if (observer != null)
            observer.OnGaugeChanged += OnGaugeChanged;
    }

    private void OnDisable()
    {
        if (observer != null)
            observer.OnGaugeChanged -= OnGaugeChanged;
    }

    private void OnGaugeChanged(int current, int max)
    {
        // ������ �� �� ��������Ʈ �ε��� ����
        int idx = MapToSpriteIndex(current, max);
        if (idx == lastIndex) return; // ��ȭ ������ ��ŵ

        // ��������Ʈ ��ü
        Sprite s = (idx >= 0 && idx < gaugeSprites.Count) ? gaugeSprites[idx] : null;
        if (s != null)
        {
            if (uiImage) uiImage.sprite = s;
            if (spriteRenderer) spriteRenderer.sprite = s;
            lastIndex = idx;
        }

        // Ǯ�������� ���� �׵θ� ON
        if (borderHighlight) borderHighlight.SetActive(current >= max);
    }

    // gaugeSprites ������ max+1�� ��Ȯ�� ��ġ�ϸ� 1:1 ����,
    // �׷��� ������ ������ ���� ����� �������� ����.
    private int MapToSpriteIndex(int current, int max)
    {
        if (gaugeSprites == null || gaugeSprites.Count == 0) return -1;

        current = Mathf.Clamp(current, 0, max);
        if (max <= 0) return 0;

        if (gaugeSprites.Count == max + 1)
        {
            // ��: max=5, current=3 -> index=3
            return current;
        }
        else
        {
            // ��: ��������Ʈ�� 6���� �ƴ� ��� ������ �ٻ� ����
            float t = (float)current / max; // 0~1
            int idx = Mathf.RoundToInt(t * (gaugeSprites.Count - 1));
            return Mathf.Clamp(idx, 0, gaugeSprites.Count - 1);
        }
    }
}
