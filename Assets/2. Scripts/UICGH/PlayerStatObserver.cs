using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class PlayerStatObserver : MonoBehaviour
{
    // ====== ���� �ʵ� ======
    public PlayerStat _playerStat;
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }
    [SerializeField] private float pollInterval = 0.05f; // 20Hz ����
    [SerializeField] private int maxGauge = 5;           // UI ����

    // ====== �߰�: �ֱ� Ž�� ���� ======
    [SerializeField] private float findInterval = 0.5f;

    public event Action<int, int> OnHealthChanged; // (����, �ִ�)
    public event Action<int, int> OnGaugeChanged;  // (����, �ִ�)

    private FieldInfo fiCurrentHealth;
    private FieldInfo fiMaxHealth;

    private int lastHealth, lastMaxHealth, lastGauge;

    // ====== ���� ����: ���������� ���ε��� Stat ĳ�� ======
    private PlayerStat lastBoundStat;

    private void Awake()
    {
        // ù �õ�
        if (!PlayerStat)
            PlayerStat = FindObjectOfType<PlayerStat>();

        if (PlayerStat != null)
            BindReflection(PlayerStat);
    }

    private void OnEnable()
    {
        StartCoroutine(PollLoop());
        StartCoroutine(FindLoop()); // ====== �߰�: �ֱ��� ��Ž��/����ε� ======
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void BindReflection(PlayerStat target)
    {
        var t = typeof(PlayerStat);
        // PlayerStat�� private 'maxHealth'�� �ʿ�. (currentHealth�� ���� CurrentHealth�κ��� �о ������,
        // �� �ڵ� ��Ÿ�� ������ ���÷��� ���� �״�� ��)
        fiCurrentHealth = t.GetField("CurrentHealth", BindingFlags.Public | BindingFlags.Instance)
                          ?? t.GetField("currentHealth", BindingFlags.NonPublic | BindingFlags.Instance);
        fiMaxHealth = t.GetField("maxHealth", BindingFlags.NonPublic | BindingFlags.Instance);

        lastBoundStat = target;

        // ���ε� ����, ��� �� �� ��ε�ĳ��Ʈ (UI�� �ʰ� �پ�� �ֽŰ� ����)
        lastMaxHealth = ReadMaxHealth();
        lastHealth = ReadHealth();
        lastGauge = ReadGauge();
        OnHealthChanged?.Invoke(lastHealth, lastMaxHealth);
        OnGaugeChanged?.Invoke(lastGauge, maxGauge);
    }

    private IEnumerator FindLoop()
    {
        var wait = new WaitForSeconds(findInterval);

        while (enabled)
        {
            // 1) ����� ����ų� �ı��Ǿ����� ��Ž��
            if (PlayerStat == null)
            {
                var ps = FindObjectOfType<PlayerStat>();
                if (ps != null)
                {
                    PlayerStat = ps;
                    BindReflection(ps);
                }
            }
            else
            {
                // 2) ������ �ٲ���ų�(�� ��ε� ��) ���÷��� �ڵ��� ���ư����� ����ε�
                if (PlayerStat != lastBoundStat || fiMaxHealth == null || fiCurrentHealth == null)
                {
                    BindReflection(PlayerStat);
                }
            }

            yield return wait;
        }
    }

    private IEnumerator PollLoop()
    {
        // �ʱ� ��ε�ĳ��Ʈ (BindReflection������ ��������, ���� Enable �� ������)
        lastMaxHealth = ReadMaxHealth();
        lastHealth = ReadHealth();
        lastGauge = ReadGauge();

        OnHealthChanged?.Invoke(lastHealth, lastMaxHealth);
        OnGaugeChanged?.Invoke(lastGauge, maxGauge);

        var wait = new WaitForSeconds(pollInterval);
        while (enabled)
        {
            int h = ReadHealth();
            int mh = ReadMaxHealth();
            int g = ReadGauge();

            if (h != lastHealth || mh != lastMaxHealth)
            {
                lastHealth = h;
                lastMaxHealth = mh;
                OnHealthChanged?.Invoke(h, mh);
            }

            if (g != lastGauge)
            {
                lastGauge = g;
                OnGaugeChanged?.Invoke(g, maxGauge);
            }

            yield return wait;
        }
    }

    private int ReadHealth()
    {
        if (PlayerStat == null) return lastHealth;

        // ���� ������Ƽ�� ������ �װ� �켱 ���
        var prop = typeof(PlayerStat).GetProperty("CurrentHealth");
        if (prop != null)
        {
            float f = Convert.ToSingle(prop.GetValue(PlayerStat));
            return Mathf.Clamp(Mathf.RoundToInt(f), 0, ReadMaxHealth());
        }

        if (fiCurrentHealth == null) return lastHealth;
        float v = Convert.ToSingle(fiCurrentHealth.GetValue(PlayerStat));
        return Mathf.Clamp(Mathf.RoundToInt(v), 0, ReadMaxHealth());
    }

    private int ReadMaxHealth()
    {
        if (PlayerStat == null) return lastMaxHealth > 0 ? lastMaxHealth : 5;
        if (fiMaxHealth == null) return lastMaxHealth > 0 ? lastMaxHealth : 5;

        float f = Convert.ToSingle(fiMaxHealth.GetValue(PlayerStat));
        return Mathf.Max(1, Mathf.RoundToInt(f));
    }

    private int ReadGauge()
    {
        if (PlayerStat == null) return lastGauge;
        return Mathf.Clamp(PlayerStat.Gauge, 0, maxGauge);
    }

    // �ܺ� ���ٿ�
    public PlayerStat Stat => PlayerStat;
    public int MaxGauge => maxGauge;
}