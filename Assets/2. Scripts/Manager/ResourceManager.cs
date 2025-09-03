using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : Singleton<ResourceManager>
{
    AsyncOperationHandle<IList<GameObject>> objsHandle;
    AsyncOperationHandle<IList<AudioClip>> soundsHandle;
    //���� �� ���� ������Ʈ�Ŵ������� Insert�Լ��� ����Ͽ� �ε带��Ŵ
    // 1. ������ �ʿ��� ���ҽ��� �󺧷� ����
    // 2. ������ �ε��Ұ��� 
    // 3. AddObject PlaySound key���� �����հ� ������� 
    // 4. Release������� �޸𸮿��� ������ �ȱ׷� �޸𸮴��� ������ ���ٰ���
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ObjectManager.Instance.AddObject("Stage1", Vector3.zero, Quaternion.identity);
        }
    }
    public override void Release()
    {
    }

    public AsyncOperationHandle<IList<T>> LoadResource<T>(string label, Action<T> callback) where T : UnityEngine.Object
    {
        return Addressables.LoadAssetsAsync<T>(label, callback);
    }
}
