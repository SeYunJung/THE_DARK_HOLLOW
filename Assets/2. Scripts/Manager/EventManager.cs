using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ʿ��� �̺�Ʈ�� ���� - Good
public enum EventType
{
    Boss_Hit,      // �ǰ�/����
    Boss_Stun,     // ���� ����
    Boss_Die,      // ���
    Boss_Phase,    // ������(����) ����
               
}
public class EventManager : Singleton<EventManager>
{
    // �̺�Ʈ ������ ����Ʈ�� Dictionary�� ���� ���� EventType�� IN�� OUT���� �ΰ��� �з��� ����Ʈ�� ����
    private Dictionary<EventType, List<IEventListener>> listeners = new Dictionary<EventType, List<IEventListener>>();

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManagerSceneLoaded;

    }
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SceneManagerSceneLoaded;
    }
    private void SceneManagerSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        //���� �ٲ� ���� �̺�Ʈ �������� �������ش�.
        RefreshListeners();
    }

    public void AddListener(EventType eventType, IEventListener listener)       // �̺�Ʈ �޴� ����
    {
        List<IEventListener> ListenList = null;

        if (listeners.TryGetValue(eventType, out ListenList))
        {
            //�ش� �̺�Ʈ Ű���� �����Ѵٸ�, �̺�Ʈ�� �߰����ش�.
            ListenList.Add(listener);
            return;
        }
        ListenList = new List<IEventListener>();
        ListenList.Add(listener);
        listeners.Add(eventType, ListenList);

    }

    // object Param �� ��üȭ �ϴ� ���� ����
    public void PostNotification(EventType eventType, Component Sender, object Param = null) // �̺�Ʈ �߻�����
    {
        List<IEventListener> ListenList = null;
        //�̺�Ʈ ������(�����)�� ������ �׳� ����.
        if (!listeners.TryGetValue(eventType, out ListenList))
            return;
        //��� �̺�Ʈ ������(�����)���� �̺�Ʈ ����.
        for (int i = 0; i < ListenList.Count; i++)
        {
            if (ListenList[i] != null)
            {
                ListenList[i].OnEvent(eventType, Sender, Param);
            }
        }
    }


    public void RemoveEvent(EventType eventType)        // ����̺�Ʈ ����
    {
        listeners.Remove(eventType);
    }

    public void RemoveListener(EventType evt, IEventListener listener)
    {
        if (!listeners.TryGetValue(evt, out var set)) return;
        set.Remove(listener);
        if (set.Count == 0) listeners.Remove(evt);
    }

    // �����ʰ� �ڽ��� ��ϵ� ��� �̺�Ʈ���� ���ŵ� �� ���
    public void RemoveTarget(IEventListener listener)
    {
        var keys = new List<EventType>(listeners.Keys);
        foreach (var k in keys)
        {
            var set = listeners[k];
            set.Remove(listener);
            if (set.Count == 0) listeners.Remove(k);
        }
    }

    private void RefreshListeners()     // Scene��ȯ�� ��� �̺�Ʈ �ʱ�ȭ
    {
        //�ӽ� Dictionary ����
        Dictionary<EventType, List<IEventListener>> TmpListeners = new Dictionary<EventType, List<IEventListener>>();

        //���� �ٲ� ���� �����ʰ� Null�� �� �κ��� �������ش�. 
        foreach (KeyValuePair<EventType, List<IEventListener>> Item in listeners)
        {
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                if (Item.Value[i] == null)
                    Item.Value.RemoveAt(i);
            }

            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }
        //����ִ� �����ʴ� �ٽ� �־��ش�.
        listeners = TmpListeners;
    }


}

