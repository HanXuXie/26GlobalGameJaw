using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;

    /// <summary>
    /// 单例模式引用对象池管理器
    /// </summary>
    public static ObjectPoolManager Instance
    {

        get
        {
            instance = GameObject.FindObjectOfType<ObjectPoolManager>();
            if (instance == null)
            {
                GameObject obj = new GameObject("ObjectPoolManager");
                instance = obj.AddComponent<ObjectPoolManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = GameObject.FindObjectOfType<ObjectPoolManager>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Dictionary<Type, object> pools { private set; get; } = new Dictionary<Type, object>();

    /// <summary>
    /// 获取指定对象的对象池
    /// </summary>
    public ObjectPool<T> GetPool<T>(T _prefab, int _initialCapacity = 100, bool _expendable = true) where T : Component, IPoolable
    {
        Type type = typeof(T);
        if (pools.TryGetValue(type, out object pool))
        {
            return (ObjectPool<T>)pool;
        }

        ObjectPool<T> newPool = new ObjectPool<T>(_prefab, this.transform, _initialCapacity, _expendable);
        pools[type] = newPool;
        return newPool;
    }

    /// <summary>
    /// 将指定对象从指定对象池中取出
    /// </summary>
    public T Spawn<T>(T _prefab, int _initialCapacity = 100, bool _expendable = true) where T : Component, IPoolable
    {
        ObjectPool<T> objectPool = GetPool<T>(_prefab, _initialCapacity, _expendable);
        return objectPool.Get();
    }

    /// <summary>
    /// 将对象放回到对应的对象池中
    /// </summary>
    public void DeSpawn<T>(T obj) where T : Component, IPoolable
    {
        Type type = typeof(T);
        if (pools.TryGetValue(type, out object pool))
        {
            ((ObjectPool<T>)pool).ReturnPool(obj);
        }
        else
        {
            GameObject.Destroy(obj.gameObject);
        }
    }

    /// <summary>
    /// 预热指定类型对象池,创建对象
    /// </summary>
    public void WarpUp<T>(T _prefab, int _initialCount) where T : Component, IPoolable
    {
        ObjectPool<T> pool = GetPool(_prefab);
        pool.WarmUp(_initialCount);
    }

}
