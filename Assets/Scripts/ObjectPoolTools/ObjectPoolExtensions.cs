using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectPoolExtensions
{
    /// <summary>
    /// 从池中对象设置位置和旋转
    /// </summary>
    public static T SpawnAt<T>(this ObjectPool<T> _pool, Vector3 _position, Quaternion rotation = default) where T : Component, IPoolable
    {
        T obj = _pool.Get();
        obj.transform.position = _position;
        obj.transform.rotation = rotation == default ? Quaternion.identity : rotation;
        return obj;
    }

    /// <summary>
    /// 从池中获取对象并设置父对象
    /// </summary>
    public static T SpawnUnder<T>(this ObjectPool<T> _pool, Transform _parent) where T : Component, IPoolable
    {
        T obj = _pool.Get();
        obj.transform.parent = _parent;
        return obj;
    }

}
