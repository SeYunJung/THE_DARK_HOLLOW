using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIcon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string damagedBoolName = "IsDamaged";
    [SerializeField] private float destroyDelay = 0.8f;

    // ���� �� ȣ���
    public void IconDestroy()
    {
        if (animator != null)
        {
            animator.SetBool(damagedBoolName, true);
        }
        // �ִϸ��̼��� ��ü�� ���¿��� 0.8�� �� �ı�
        Destroy(gameObject, destroyDelay);
    }
}
