using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    // Player, ���� �� ĳ���� ����
    public Boss _boss;
    public Boss Boss { get { return _boss; } set { _boss = value; } }
}
