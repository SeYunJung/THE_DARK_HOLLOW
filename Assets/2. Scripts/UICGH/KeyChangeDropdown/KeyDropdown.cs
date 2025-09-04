using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class KeyDropdown : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("Target Binding")]
    [Tooltip("�ٲ� ��� �׼� (��: Jump, Attack, Move)")]
    [SerializeField] private InputActionReference actionRef;

    [Tooltip("���� ���ε��̸� 0. Composite��� �ش� ��Ʈ�� bindingIndex(��: left/right/up/down)")]
    [SerializeField] private int bindingIndex = 0;

    [Header("���� Ű")]
    [SerializeField] private string playerPrefsKey = "InputRebinds";

    // ��Ӵٿ ������ '��� Ű' ��� (���ϸ� �� �߰�)
    private static readonly Key[] AllowedKeys =
    {
        // ����Ű
        Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow,
        // ���ĺ�
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        // ����Ű (���)
        Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
        Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9,
        // ���� Ư��
        Key.Space, Key.LeftShift, Key.RightShift,
        Key.LeftCtrl, Key.RightCtrl,
        Key.LeftAlt, Key.RightAlt
    };

    // ��Ӵٿ� �ɼ�: ǥ�� ���ڿ� �� ���ε� path ����
    private readonly List<(string display, string path)> _options = new();

    private InputAction _action => actionRef != null ? actionRef.action : null;

    private void Awake()
    {
        if (dropdown == null) dropdown = GetComponent<TMP_Dropdown>();

        // ����� ���ε� �ҷ�����(������Ʈ ��ü���� �� ���� ȣ���ص� ��)
        LoadOverrides();

        BuildOptions();
        ReflectCurrentBindingToDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
        SaveOverrides();
    }

    private void BuildOptions()
    {
        dropdown.ClearOptions();
        _options.Clear();

        foreach (var key in AllowedKeys)
        {
            string controlPath = KeyToPath(key); // "<Keyboard>/space" ���� ����
            string label = Humanize(controlPath); // "Space", "Left Shift" ��
            _options.Add((label, controlPath));
        }

        dropdown.AddOptions(_options.Select(o => o.display).ToList());
    }

    private void ReflectCurrentBindingToDropdown()
    {
        if (_action == null) return;
        var bindings = _action.bindings;

        if (bindingIndex < 0 || bindingIndex >= bindings.Count)
        {
            Debug.LogWarning($"Binding index out of range: {bindingIndex} on action {_action.name}");
            return;
        }

        // overridePath�� ������ �켱, ������ default path ���
        var path = string.IsNullOrEmpty(bindings[bindingIndex].overridePath)
            ? bindings[bindingIndex].effectivePath
            : bindings[bindingIndex].overridePath;

        // Ű���� ���ε��� ������� ��
        int idx = IndexOfPath(path);
        dropdown.SetValueWithoutNotify(Mathf.Max(0, idx));
    }

    private void OnDropdownChanged(int optionIndex)
    {
        if (_action == null) return;
        if (optionIndex < 0 || optionIndex >= _options.Count) return;

        string newPath = _options[optionIndex].path;

        // �浹 �ڵ�����(����): ���� Ű�� ���� �ٸ� ���ε��� override�� ���
        AutoUnbindConflicts(newPath);

        // ��� ���ε��� override ����
        var bindings = _action.bindings;
        if (bindingIndex < 0 || bindingIndex >= bindings.Count) return;

        _action.ApplyBindingOverride(bindingIndex, new InputBinding { overridePath = newPath });

        // ���� ��� ����(���ϸ� ��� ��ȯ ������ �� ���� �����ص� ��)
        SaveOverrides();
    }

    private void AutoUnbindConflicts(string newPath)
    {
        if (_action == null) return;

        var asset = _action.actionMap?.asset;
        if (asset == null) return;

        foreach (var map in asset.actionMaps)
        {
            foreach (var act in map.actions)
            {
                for (int i = 0; i < act.bindings.Count; i++)
                {
                    var b = act.bindings[i];
                    var effective = string.IsNullOrEmpty(b.overridePath) ? b.effectivePath : b.overridePath;
                    if (string.Equals(effective, newPath, StringComparison.OrdinalIgnoreCase))
                    {
                        // ���� �츮�� �ٲٴ� �� ���ε��̸� ��ŵ
                        if (act == _action && i == bindingIndex) continue;

                        act.RemoveBindingOverride(i); // �浹 ����
                    }
                }
            }
        }
    }

    private void SaveOverrides()
    {
        var asset = _action?.actionMap?.asset;
        if (asset == null) return;

        string json = asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(playerPrefsKey, json);
        PlayerPrefs.Save();
    }

    private void LoadOverrides()
    {
        var asset = _action?.actionMap?.asset;
        if (asset == null) return;

        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string json = PlayerPrefs.GetString(playerPrefsKey);
            asset.LoadBindingOverridesFromJson(json);
        }
    }

    private static string KeyToPath(Key key)
    {
        // Key.X -> "<Keyboard>/x", Key.Space -> "<Keyboard>/space"
        string name = key.ToString(); // e.g., LeftShift, Digit1
        // ����Ű ó��
        if (name.StartsWith("Digit"))
            return $"<Keyboard>/{name.Substring("Digit".Length)}";

        // �Ϲ� Ű
        return $"<Keyboard>/{ToSnakeCase(name)}";
    }

    private static int IndexOfPath(string path)
    {
        // ��Ӵٿ� �ɼ� �� path�� ���� �׸��� �ε���
        for (int i = 0; i < AllowedKeys.Length; i++)
        {
            if (string.Equals(KeyToPath(AllowedKeys[i]), path, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return 0; // �� ã���� ù �ɼ�
    }

    private static string Humanize(string controlPath)
    {
        // "LeftShift" -> "Left Shift" �� ���ģȭ ǥ��
        return InputControlPath.ToHumanReadableString(controlPath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private static string ToSnakeCase(string keyName)
    {
        // Key enum �̸��� InputSystem ��ο� �°� ��ȯ
        // LeftShift -> "leftShift"�� �ƴ϶� "leftShift" ��� ���� ��δ� "leftShift"�� �ƴ� "leftShift"?!
        // �����ϰ� ��� lower + Ư�� ó��.
        // InputSystem�� ��κ� lower-case�� ��Ī��: "space", "leftShift", "rightShift" ��
        // ����ȭ�� ���� �Ʒ� ���� �Ϻ� ����
        switch (keyName)
        {
            case "LeftShift": return "leftShift";
            case "RightShift": return "rightShift";
            case "LeftCtrl": return "leftCtrl";
            case "RightCtrl": return "rightCtrl";
            case "LeftAlt": return "leftAlt";
            case "RightAlt": return "rightAlt";
            case "UpArrow": return "upArrow";
            case "DownArrow": return "downArrow";
            case "LeftArrow": return "leftArrow";
            case "RightArrow": return "rightArrow";
            case "Space": return "space";
        }
        return keyName.ToLower(); // A->"a", X->"x" ��
    }
}
