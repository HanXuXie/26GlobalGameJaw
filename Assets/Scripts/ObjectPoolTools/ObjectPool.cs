using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;

/// <summary>
/// 标准泛型对象池模板
/// </summary>
public class ObjectPool<T> where T : Component, IPoolable
{
    private readonly Queue<T> pools = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;
    private readonly int initialCapacity;
    private readonly bool expendable;

    private int perInitialCount = 10;
    private bool isWarmUp = false;
    private int Count => pools.Count;
    public int TotalCreated { private set; get; }

    public ObjectPool(T _Prefab, Transform _parent, int _initialCapacity, bool _expendable)
    {
        prefab = _Prefab;
        parent = _parent;
        initialCapacity = _initialCapacity;
        expendable = _expendable;

        WarmUp(perInitialCount);
    }


    /// <summary>
    /// 获取对象池内的对象，如对象不足则创建新的对象
    /// </summary>
    public T Get()
    {
        T obj;
        if (pools.Count > 0)
        {
            obj = pools.Dequeue();

        }
        else if (expendable)
        {
            obj = CreateNewObject();
        }
        else
        {
            obj = null;
        }
        if (obj != null)
        {
            obj.gameObject.SetActive(true);
            obj.OnGetFromPool();
        }
        Debug.Log(obj.ToString());
        return obj;
    }

    /// <summary>
    /// 将该对象返回对象池中，并执行复位
    /// </summary>
    public void ReturnPool(T obj)
    {
        if (obj == null)
            return;

        obj.OnReset();
        obj.OnReturnToPool();
        obj.gameObject.SetActive(false);
        pools.Enqueue(obj);
    }


    /// <summary>
    /// 调用按照初始值生成池对象
    /// </summary>
    public void WarmUp(int _perInitialCount)
    {
        if (isWarmUp)
            return;

        ObjectPoolManager.Instance.StartCoroutine(WarmUpCorotine(_perInitialCount));

        isWarmUp = true;
    }

    /// <summary>
    /// 按照设定好的参数每帧创建对象
    /// </summary>
    private IEnumerator WarmUpCorotine(int framesPerStep)
    {
        int created = 0;
        while (created < initialCapacity)
        {
            for (int i = 0; i < framesPerStep && created < initialCapacity; i++)
            {
                T obj = CreateNewObject();
                ReturnPool(obj);
                created++;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 创建新的池对象
    /// </summary>
    private T CreateNewObject()
    {
        T newObj = GameObject.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        TotalCreated++;
        return newObj;
    }

    /// <summary>
    /// 清除该对象池内的所有对象
    /// </summary>
    public void ClearAllPool()
    {
        while (pools.Count > 0)
        {
            T obj = pools.Dequeue();
            if (obj != null)
            {
                GameObject.Destroy(obj.gameObject);
            }
        }

        TotalCreated = 0;
    }
}
