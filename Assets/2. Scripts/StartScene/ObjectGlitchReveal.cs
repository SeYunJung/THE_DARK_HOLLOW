using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ObjectGlitchReveal : MonoBehaviour
{
    [Header("Timing")]
    public float totalTime = 0.5f;   // ��ü ���� �ð�
    public int flickerCount = 2;     // ������ Ƚ��

    [Header("Jitter")]
    public float jitterPos = 4f;     // ��ġ ��鸲 (UI ����: px)
    public float jitterRot = 4f;     // Z ȸ�� ��鸲 (deg)
    public float jitterScale = 0.06f;// ������ ��鸲 ����

    CanvasGroup group;
    RectTransform rt;        // UI�� RectTransform ���
    Transform tr;            // ��UI�� Transform
    Vector3 basePos3D;
    Quaternion baseRot;
    Vector3 baseScale;
    bool isUI;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        rt = GetComponent<RectTransform>();
        isUI = rt != null;
        tr = transform;

        if (isUI)
            basePos3D = rt.anchoredPosition3D;
        else
            basePos3D = tr.localPosition;

        baseRot = tr.localRotation;
        baseScale = tr.localScale;
    }

    public void InstantHide()
    {
        group.alpha = 0f;
        ResetTRS();
        gameObject.SetActive(true);
    }

    void ResetTRS()
    {
        if (isUI) rt.anchoredPosition3D = basePos3D;
        else tr.localPosition = basePos3D;

        tr.localRotation = baseRot;
        tr.localScale = baseScale;
    }

    public IEnumerator Co_Reveal()
    {
        InstantHide();

        float step = Mathf.Max(0.01f, totalTime / Mathf.Max(1, flickerCount));
        for (int i = 0; i < flickerCount; i++)
        {
            // ON (���� ��鸲)
            group.alpha = 1f;

            Vector3 jPos = new Vector3(
                Random.Range(-jitterPos, jitterPos),
                Random.Range(-jitterPos, jitterPos),
                0f
            );
            float jRot = Random.Range(-jitterRot, jitterRot);
            float jScale = 1f + Random.Range(-jitterScale, jitterScale);

            if (isUI) rt.anchoredPosition3D = basePos3D + jPos;
            else tr.localPosition = basePos3D + jPos;

            tr.localRotation = Quaternion.Euler(0, 0, jRot) * baseRot;
            tr.localScale = baseScale * jScale;

            yield return new WaitForSeconds(step * 0.5f);

            // OFF (����ġ/����)
            group.alpha = 0f;
            ResetTRS();
            yield return new WaitForSeconds(step * 0.5f);
        }

        // �������� ���������� ǥ��
        group.alpha = 1f;
        ResetTRS();
    }
}
