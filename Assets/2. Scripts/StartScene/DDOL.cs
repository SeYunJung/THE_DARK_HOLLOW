using UnityEngine;

public class DDOL : MonoBehaviour
{
    private static DDOL instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �ߺ� EventSystem ����
        }
    }
}
