using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeadWatcher : MonoBehaviour
{
    public string StartScene;

    public static BossDeadWatcher I { get; private set; }
    public bool IsBossCleared { get; private set; } = false;

    // ���� ���� Ű
    private const string PPKey = "BossCleared";

    //BossController cachedBoss;
    float pollAcc = 0f;
    const float pollHz = 1f; // �ʴ� ���� (�ʿ�� ����)

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        // ���� ���� �ҷ�����
        IsBossCleared = PlayerPrefs.GetInt(PPKey, 0) == 1;
    }

    void Update()
    {
        if (IsBossCleared) return; // �̹� Ŭ����� �� �� �� ����

        pollAcc += Time.deltaTime;
        if (pollAcc >= 1f / pollHz)
        {
            pollAcc = 0f;

            // TODO: ���� ���� ó�� ����(������ : �̿���)
            /* // ������ �� ã������ ã�Ƶΰ�
             if (cachedBoss == null)
                 cachedBoss = FindObjectOfType<BossController>();*/

            // ������ ã�Ұ�, ���� ���¶�� Ŭ���� ó��
            if (CharacterManager.instance.Boss.controller != null && CharacterManager.instance.Boss.controller.IsDead)
            {
                ReportBossDead();
            }
        }
    }

   // ������ �׾����� �ܺο��� ���� �뺸�� �� ȣ��
    public void ReportBossDead()
    {
        if (IsBossCleared) return;
        IsBossCleared = true;

        // ���� ����
        PlayerPrefs.SetInt(PPKey, 1);
        PlayerPrefs.Save();
        Debug.Log("[BossDeadWatcher] Boss Cleared ��ϵ�");
    }

    // �� ���� ��� �ʱ�ȭ�� �� ȣ��
    public void ResetClear()
    {
        IsBossCleared = false;
        PlayerPrefs.DeleteKey(PPKey);
        Debug.Log("[BossDeadWatcher] Boss Cleared �ʱ�ȭ");
        SceneManager.LoadScene(StartScene);
    }
}
