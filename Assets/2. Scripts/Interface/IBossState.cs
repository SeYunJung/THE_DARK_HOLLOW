using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    void OnEnter();
    void OnExit();
    void Tick(float dt);
    void FixedTick(float fdt);
    bool IsLocked { get; }
    string Name { get; }
}
