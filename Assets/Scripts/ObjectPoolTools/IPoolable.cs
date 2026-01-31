using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 可池化对象的接口
/// </summary>
public interface IPoolable
{
    /// <summary>
    /// 从池中获取时调用
    /// </summary>
    void OnGetFromPool();

    /// <summary>
    /// 返回池中时调用
    /// </summary>
    void OnReturnToPool();

    /// <summary>
    /// 重置对象参数时调用
    /// </summary>
    void OnReset();
}
