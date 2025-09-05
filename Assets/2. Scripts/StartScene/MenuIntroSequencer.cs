using System.Collections;
using UnityEngine;

public class MenuIntroSequencer : MonoBehaviour
{
    [Header("Title Objects (in order)")]
    public ObjectGlitchReveal[] titleObjects; // Ÿ��Ʋ ������Ʈ���� ������� �巡��

    [Header("Buttons Reveal Together")]
    public ObjectGlitchReveal[] buttonObjects; // ��ư 2�� ��Ʈ �巡��

    [Header("Delays")]
    public float delayBetweenTitles = 0.12f;  // Ÿ��Ʋ ������Ʈ ���� ��
    public float delayBeforeButtons = 0.2f;   // Ÿ��Ʋ �� ���� �� ��ư ��Ÿ���� �� ��

    IEnumerator Start()
    {
        // ���� �� ���� ����
        foreach (var t in titleObjects) if (t) t.InstantHide();
        foreach (var b in buttonObjects) if (b) b.InstantHide();

        // Ÿ��Ʋ ������Ʈ���� ���� ���
        foreach (var t in titleObjects)
        {
            if (!t) continue;
            yield return StartCoroutine(t.Co_Reveal());
            yield return new WaitForSeconds(delayBetweenTitles);
        }

        yield return new WaitForSeconds(delayBeforeButtons);

        // ��ư �ΰ� ���ÿ� ���
        var coros = new Coroutine[buttonObjects.Length];
        for (int i = 0; i < buttonObjects.Length; i++)
            if (buttonObjects[i]) coros[i] = StartCoroutine(buttonObjects[i].Co_Reveal());

        // ��� ���� ������ ���
        foreach (var c in coros) if (c != null) yield return c;
    }
}