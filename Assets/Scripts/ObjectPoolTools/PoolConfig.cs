using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 池配置类
/// </summary>
[System.Serializable]
public class PoolConfig
{
    public int InitialCapacity = 10;
    public bool Expandable = true;
    public bool Preload = false;
    public int PreLoadCount = 5;
}
