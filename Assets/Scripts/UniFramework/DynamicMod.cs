using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniFramework
{
    public abstract class DynamicMod
    {
        public DynamicMod() { }

        public abstract void OnLoad();
        public abstract void OnUnLoad();
    }

}