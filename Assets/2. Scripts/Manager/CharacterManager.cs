using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float hp;
    Vector3 lastCheckPoint;
}

public class CharacterManager : Singleton<CharacterManager>
{
    public PlayerData playerData; // �÷��̾ ����ȯ�� �����Ǿ�� �ϴ� ������
    // Player, ���� �� ĳ���� ����
    public Boss _boss;
    public Boss Boss { get { return _boss; } set { _boss = value; } }

    // Player
    public PlayerStat _playerStat;
    public PlayerStat PlayerStat { get { return _playerStat; } set { _playerStat = value; } }


    public OnTriggerCamera _onTriggerCamera;
    public OnTriggerCamera OnTriggerCamera { get { return _onTriggerCamera; } set { _onTriggerCamera = value; } }


}
