using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener
{
    //�̺�Ʈ �߻��� ���۵Ǵ� �̺�Ʈ ����.
    public void OnEvent(EventType eventType, Component Sender, object Param = null);
}
