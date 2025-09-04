using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUIController : MonoBehaviour
{
    [Tooltip("���ʡ������ ������, ���� ����ִ� ĭ���� ����ִ� �迭(�ִ� 5��)")]
    [SerializeField] private List<LifeIcon> lifeIcons = new List<LifeIcon>();

    [Tooltip("�÷��̾� ���� ����")]
    [SerializeField] private PlayerStat playerStat;

    private void Awake()
    {
        if(!playerStat) playerStat = FindObjectOfType<PlayerStat>();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void HandleHealthChanged(int current, int max)
    {
        // ���� ����ִ� ������ ���� current�� �ǵ��� �����.
        // �پ�� ��ŭ ����(������ ��)���� �Ͷ߸���.
        // ��) 5��4�� ������ �ε��� 4�� �ı�
        
    }

}
