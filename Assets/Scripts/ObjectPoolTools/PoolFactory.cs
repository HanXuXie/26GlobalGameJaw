using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 对象池工厂
/// </summary>
public static class PoolFactory
{
    public static ObjectPool<T> Create<T>(T _prefab, int _initialCapacity = 10, bool _expandable = true) where T : Component, IPoolable
    {
        return ObjectPoolManager.Instance.GetPool<T>(_prefab, _initialCapacity, _expandable);
    }

    public static ObjectPool<T> Create<T>(T _prefab, PoolConfig _config) where T : Component, IPoolable
    {
        return ObjectPoolManager.Instance.GetPool<T>(_prefab, _config.InitialCapacity, _config.Expandable);
    }
}
