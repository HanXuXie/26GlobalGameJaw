using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMonoBehaviour : MonoBehaviour, IPoolable
{
    public virtual void OnGetFromPool()
    {
    }
    public virtual void OnReset()
    {

    }
    public virtual void OnReturnToPool()
    {

    }
}
