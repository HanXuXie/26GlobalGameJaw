using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace UniFramework
{

    /// <summary>
    /// 单例类管理类，用于动态管理单例类 (DynamicMod)
    /// </summary>
    public class DynamicModManager
    {
        private static DynamicModManager instance;

        private Dictionary<System.Type, DynamicMod> m_dynamicModPool;

        private DynamicModManager()
        {
            m_dynamicModPool = new Dictionary<System.Type, DynamicMod>();
        }

        #region 对外接口
        /// <summary>
        /// 获取 DynamicModManager 单例实例
        /// </summary>
        public static DynamicModManager GetInstance()
        {
            if (instance == null)
                DynamicModManager.instance = new DynamicModManager();

            return DynamicModManager.instance;
        }

        /// <summary>
        /// 卸载 DynamicModManager 单例
        /// </summary>
        public static void Unload()
        {
            DynamicModManager.instance?.UnloadAllMod();
            DynamicModManager.instance = null;
        }

        /// <summary>
        /// 获取 DynamicMod 子类单例实例
        /// </summary>
        public T GetDynamicMod<T>() where T : DynamicMod, new()
        {
            DynamicMod res = null;
            if (m_dynamicModPool.TryGetValue(typeof(T), out res))
            {
                // DoNothing
            }
            else
            {
                res = LoadMod<T>();
            }

            return (T)res;
        }

        /// <summary>
        /// 手动加载单例，推荐使用 GetDynamicMod 代替
        /// </summary>
        public T LoadMod<T>() where T : DynamicMod, new()
        {
            return (T)LoadMod(typeof(T));
        }
        private DynamicMod LoadMod(Type _type)
        {
            DynamicMod mod = null;

            if (m_dynamicModPool.TryGetValue(_type, out mod))
            {
                Debug.LogError($"{_type} is aready Load");
            }
            else
            {
                // 使用反射创建实例
                mod = (DynamicMod)Activator.CreateInstance(_type);
                mod.OnLoad();
            }
            return mod;
        }

        /// <summary>
        /// 手动卸载单例，在确定不再使用的时候可以手动卸载，关闭程序的时自动卸载
        /// </summary>
        public void UnloadMod<T>() where T : DynamicMod
        {
            UnLoadMod(typeof(T));
        }
        private void UnLoadMod(Type _type)
        {
            DynamicMod mod = null;
            if (m_dynamicModPool.TryGetValue(_type, out mod))
            {
                m_dynamicModPool.Remove(_type);
                mod.OnUnLoad();
            }
            else
            {
                Debug.LogError($"{_type} never Load");
            }

        }

        /// <summary>
        /// 卸载所有Mod
        /// </summary>
        public void UnloadAllMod()
        {
            m_dynamicModPool.Values.ForEach((mod) => { mod.OnUnLoad(); });
            m_dynamicModPool.Clear();
        }
        #endregion
    }

}